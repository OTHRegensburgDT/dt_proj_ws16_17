/*
 * Sensor_Temperature.c
 *
 *  Created on: Nov 19, 2016
 *      Author: Andreas Lackner
 */

/*********************************************************************************
 * Includes
 *********************************************************************************/
#include "Sensor_Temperature.h"

#include <xmc_vadc.h>
#include <xmc_gpio.h>

/*********************************************************************************
 * Local macros
 *********************************************************************************/

#define ADC_MODULE (XMC_VADC_GLOBAL_t*)VADC
#define Adc_Measurement_Handler IRQ_Hdlr_16

#define ADC_CHANNEL_COUNT (1U)
#define ADC_MEASUREMENT_ICLASS_NUM (0U)

#define SENSOR_CFG_A &Temperature_Sensors[0]

#define SENSOR_REF_VOLTAGE 5
#define ADC_RESOLUTION 0.00085
#define R1 10000

#define NTC_LOOKUP_ENTRIES 151

/*********************************************************************************
 * Local datatypes
 *********************************************************************************/

typedef struct ADC_MEASUREMENT_ISR
{
  uint32_t node_id; 		/**< This holds the Node ID of the NVIC.*/
  uint32_t priority; 		/**< This holds the NVIC priority.*/
  uint32_t sub_priority; 	/**< This holds the SubPriority of the NVIC. for Only XMC4x Devices*/
} ADC_MEASUREMENT_ISR_t;

typedef struct ADC_MEASUREMENT
{
  const XMC_VADC_BACKGROUND_CONFIG_t *const backgnd_config_handle; /**< This holds the LLD Background Scan Init Structure*/
  const XMC_VADC_GLOBAL_CLASS_t *const iclass_config_handle;  /**< This holds the adc global ICLASS 0 configuration*/

  const ADC_MEASUREMENT_ISR_t *const req_src_intr_handle; 	 /**< This has the NVIC configuration structure*/
  const ADC_MEASUREMENT_ISR_t *const result_intr_handle; 	 /**< This has the NVIC configuration structure*/

  const XMC_VADC_SR_t srv_req_node; 	  /**< Service Request Line selected*/
  const bool start_conversion; 			  /**< This indicates whether to start at initialization of the APP*/
} ADC_MEASUREMENT_t;

typedef struct SensorAnalogPort{
  XMC_VADC_CHANNEL_CONFIG_t *ch_handle; /**< This holds the VADC Channel LLD struct*/
  XMC_VADC_RESULT_CONFIG_t *res_handle; /**< This hold the VADC LLD Result handler*/
  XMC_VADC_GROUP_t *group_handle; 		/**< This holds the group to which the channel belongs*/

  XMC_GPIO_PORT_t *analog_port;         /**< Port number used */
  uint8_t analog_pin;                   /**< Pin number used */

  uint8_t group_index; 					/**< This holds the group index*/
  uint8_t ch_num;
} SensorAnalogPort_t;

typedef struct NTCLookupEntry{
	int temperature;
	uint32_t resistence;
}NTCLookupEntry_t;

/*********************************************************************************
 * Local data
 *********************************************************************************/

/******************************* GLOBAL ADC CONFIG *******************************/

const XMC_VADC_GLOBAL_CONFIG_t global_config =
{
  .boundary0    = (uint32_t) 0, /* Lower boundary value for Normal comparison mode*/
  .boundary1    = (uint32_t) 0, /* Upper boundary value for Normal comparison mode*/

  .clock_config = {
	                .analog_clock_divider  = (uint32_t) 4, /*Divider Factor for the Analog Internal Clock*/
	                .arbiter_clock_divider = (uint32_t) 0, /*Divider Factor for the Arbiter Clock*/
	                .msb_conversion_clock  = (uint32_t) 0, /*Double Clock for the MSB Conversion */
	              },

  .class0 = {
     	      .sample_time_std_conv            = (uint32_t) 0,  		  /*The Sample time is (2*tadci)*/
     	      .conversion_mode_standard        = XMC_VADC_CONVMODE_12BIT, /* 12bit conversion Selected*/

     	      .sampling_phase_emux_channel     = (uint32_t) 0,			  /*The Sample time is (2*tadci)*/
			  .conversion_mode_emux            = XMC_VADC_CONVMODE_12BIT, /* 12bit conversion Selected*/

		    },  /* !<ICLASS-0 */
  .class1 = {
     	      .sample_time_std_conv = (uint32_t) 0,			  /*The Sample time is (2*tadci)*/
     	      .conversion_mode_standard        = XMC_VADC_CONVMODE_12BIT, /* 12bit conversion Selected*/

     	      .sampling_phase_emux_channel     = (uint32_t) 0,			  /*The Sample time is (2*tadci)*/
			  .conversion_mode_emux            = XMC_VADC_CONVMODE_12BIT, /* 12bit conversion Selected*/

            }, /* !< ICLASS-1 */

  .data_reduction_control         = (uint32_t) 0, /* Data Reduction disabled*/
  .wait_for_read_mode             = (uint32_t) 0, /* GLOBRES Register will not be overwriten untill the previous value is read*/
  .event_gen_enable               = (uint32_t) 0, /* Result Event from GLOBRES is disabled*/
  .disable_sleep_mode_control     = (uint32_t) 0  /* Sleep mode is enabled*/
};

const XMC_VADC_GROUP_CONFIG_t group_init_handle0 =
{
  .emux_config	= {
					.stce_usage                = (uint32_t) 0, 					 /*Use STCE when the setting changes*/
					.emux_mode                 = XMC_VADC_GROUP_EMUXMODE_SWCTRL, /* Mode for Emux conversion*/
					.emux_coding               = XMC_VADC_GROUP_EMUXCODE_BINARY, /*Channel progression - binary format*/
					.starting_external_channel = (uint32_t) 0,                   /* Channel starts at 0 for EMUX*/
					.connected_channel         = (uint32_t) 0                    /* Channel connected to EMUX*/
				   },
  .class0 		= {
             	    .sample_time_std_conv            = (uint32_t) 0,                /*The Sample time is (2*tadci)*/
             	    .conversion_mode_standard        = XMC_VADC_CONVMODE_12BIT,     /* 12bit conversion Selected*/
             	    .sampling_phase_emux_channel     = (uint32_t) 0,                /*The Sample time is (2*tadci)*/
					.conversion_mode_emux            = XMC_VADC_CONVMODE_12BIT      /* 12bit conversion Selected*/
		    	  },  /* !<ICLASS-0 */
  .class1   	= {
             	    .sample_time_std_conv = (uint32_t) 0,                /*The Sample time is (2*tadci)*/
             	    .conversion_mode_standard        = XMC_VADC_CONVMODE_12BIT,     /* 12bit conversion Selected*/
             	    .sampling_phase_emux_channel     = (uint32_t) 0,                /*The Sample time is (2*tadci)*/
					.conversion_mode_emux            = XMC_VADC_CONVMODE_12BIT      /* 12bit conversion Selected*/
             	  }, /* !< ICLASS-1 */
  .boundary0    = (uint32_t) 0,  /* Lower boundary value for Normal comparison mode*/
  .boundary1	= (uint32_t) 0,  /* Upper boundary value for Normal comparison mode*/
  .arbitration_round_length = (uint32_t) 0,  /* 4 arbitration slots per round selected (tarb = 4*tadcd) */
  .arbiter_mode             = (uint32_t) XMC_VADC_GROUP_ARBMODE_ALWAYS,	/*Determines when the arbiter should run.*/
};

/******************************* CHANNEL CONFIG *******************************/

/* Global iclass0 configuration*/
const XMC_VADC_GLOBAL_CLASS_t global_iclass_config =
{
  .conversion_mode_standard  = (uint32_t) XMC_VADC_CONVMODE_12BIT,
  .sample_time_std_conv	     = (uint32_t) 0,
};

/*********************** Channel_A Configurations ************************************/


/*Channel_A ADC Channel configuration structure*/
XMC_VADC_CHANNEL_CONFIG_t  Channel_A_ch_config =
{
  .input_class                = (uint32_t) XMC_VADC_CHANNEL_CONV_GLOBAL_CLASS0,  /* Global ICLASS 0 selected */
  .lower_boundary_select 	  = (uint32_t) XMC_VADC_CHANNEL_BOUNDARY_GROUP_BOUND0,
  .upper_boundary_select 	  = (uint32_t) XMC_VADC_CHANNEL_BOUNDARY_GROUP_BOUND0,
  .event_gen_criteria         = (uint32_t) XMC_VADC_CHANNEL_EVGEN_NEVER, /*Channel Event disabled */
  .sync_conversion  		  = (uint32_t) 0,                            /* Sync feature disabled*/
  .alternate_reference        = (uint32_t) XMC_VADC_CHANNEL_REF_INTREF,  /* Internal reference selected */
  .result_reg_number          = (uint32_t) 15,                           /* GxRES[15] selected */
  .use_global_result          = (uint32_t) 0, 				             /* Use Group result register*/
  .result_alignment           = (uint32_t) XMC_VADC_RESULT_ALIGN_RIGHT,  /* Result alignment - Right Aligned*/
  .broken_wire_detect_channel = (uint32_t) XMC_VADC_CHANNEL_BWDCH_VAGND, /* No Broken wire mode select*/
  .broken_wire_detect		  = (uint32_t) 0,    		                 /* No Broken wire detection*/
  .bfl                        = (uint32_t) 0,                            /* No Boundary flag */
  .channel_priority           = (uint32_t) 0,                   		 /* Lowest Priority 0 selected*/
  .alias_channel              = (int8_t) 1  /* Channel is Aliased*/
};

/*Channel_A Result configuration structure*/
XMC_VADC_RESULT_CONFIG_t Channel_A_res_config =
{
  .data_reduction_control  = (uint8_t) 0,  /* No Accumulation */
  .post_processing_mode    = (uint32_t) XMC_VADC_DMM_REDUCTION_MODE,
  .wait_for_read_mode  	   = (uint32_t) 0,  /* Disabled */
  .part_of_fifo       	   = (uint32_t) 0 , /* No FIFO */
  .event_gen_enable   	   = (uint32_t) 0   /* Disable Result event */
};


/* Background request source interrupt handler : End of Measurement Interrupt configuration structure*/
const ADC_MEASUREMENT_ISR_t backgnd_rs_intr_handle=
{
  .node_id      = 16U,
  .priority    	= 63U,
  .sub_priority = 0U,
};

/* LLD Background Scan Init Structure */
const XMC_VADC_BACKGROUND_CONFIG_t backgnd_config =
{
  .conv_start_mode   = (uint32_t) XMC_VADC_STARTMODE_CIR, 		/* Conversion start mode selected as cancel inject repeat*/
  .req_src_priority  = (uint32_t) XMC_VADC_GROUP_RS_PRIORITY_1, /* Priority of the Background request source in the VADC module*/
  .trigger_signal    = (uint32_t) XMC_VADC_REQ_TR_A,            /*If Trigger needed then this denotes the Trigger signal*/
  .trigger_edge      = (uint32_t) XMC_VADC_TRIGGER_EDGE_NONE,   /*If Trigger needed then this denotes Trigger edge selected*/
  .gate_signal    	 = (uint32_t) XMC_VADC_REQ_GT_A,			 /*If Gating needed then this denotes the Gating signal*/
  .timer_mode        = (uint32_t) 0,							 /*Timer Mode Disabled */
  .external_trigger  = (uint32_t) 0,                               /*Trigger is Disabled*/
  .req_src_interrupt = (uint32_t) 1,                              /*Background Request source interrupt Enabled*/
  .enable_auto_scan  = (uint32_t) 0,
  .load_mode         = (uint32_t) XMC_VADC_SCAN_LOAD_OVERWRITE
};

ADC_MEASUREMENT_t ADC_TEMPERATURE =
{
  .backgnd_config_handle = (XMC_VADC_BACKGROUND_CONFIG_t*) &backgnd_config,
  .req_src_intr_handle	 = (ADC_MEASUREMENT_ISR_t *) &backgnd_rs_intr_handle,
  .iclass_config_handle  = ( XMC_VADC_GLOBAL_CLASS_t *) &global_iclass_config,
  .srv_req_node          = XMC_VADC_SR_SHARED_SR2,
  .start_conversion		 = (bool) true,
};

SensorAnalogPort_t Temperature_Sensors[ADC_CHANNEL_COUNT] =
{
	/* Temperature Sensor 1 */
	{
		.ch_num        	= (uint8_t) 1,
		.group_handle  	= (VADC_G_TypeDef*)(void*) VADC_G0,
		.group_index	= (uint8_t) 0,
		.ch_handle		= (XMC_VADC_CHANNEL_CONFIG_t*) &Channel_A_ch_config,
		.res_handle	 	= (XMC_VADC_RESULT_CONFIG_t*) &Channel_A_res_config,
		.analog_port 	= XMC_GPIO_PORT14,
		.analog_pin 	= 1U
	}
};

NTCLookupEntry_t TemperatureLookupTable[NTC_LOOKUP_ENTRIES] =
{
	{-40, 190556}, {-39, 183413}, {-38, 175674}, {-37, 167646}, {-36, 159564}, {-35, 151597}, {-34, 143862}, {-33, 136436}, {-32, 129364}, {-31, 122667}, {-30, 116351},
	{-29, 110409}, {-28, 104827}, {-27, 99584}, {-26, 94660}, {-25, 90032}, {-24, 85677}, {-23, 81574}, {-22, 77703}, {-21, 74044}, {-20, 70581}, {-19, 67298},
	{-18, 64183}, {-17, 61223}, {-16, 58408}, {-15, 55728}, {-14, 53176}, {-13, 50745}, {-12, 48429}, {-11, 46222}, {-10, 44120}, {-9, 42118}, {-8, 40212},
	{-7, 38398}, {-6, 36674}, {-5, 35036}, {-4, 33480}, {-3, 32003}, {-2, 30602}, {-1, 29275}, {0, 28017},  {1, 26825},  {2, 25697},  {3, 24629},
	{4, 23617},  {5, 22659},  {6, 21752},  {7, 20891},  {8, 20074},  {9, 19298},  {10, 18560}, {11, 18481}, {12, 18148}, {13, 17631}, {14, 16991},
	{15, 16279}, {16, 15535}, {17, 14786}, {18, 14055}, {19, 13353}, {20, 12690}, {21, 12068}, {22, 11490}, {23, 10953}, {24, 10458}, {25, 10000},
	{26, 9576}, {27, 9183}, {28, 8818}, {29, 8478}, {30, 8160}, {31, 7860}, {32, 7578}, {33, 7310}, {34, 7056}, {35, 6813}, {36, 6580},
	{37, 6357}, {38, 6141}, {39, 5934}, {40, 5734}, {41, 5540}, {42, 5353}, {43, 5172}, {44, 4997}, {45, 4828}, {46, 4665}, {47, 4507},
	{48, 4354}, {49, 4207}, {50, 4065}, {51, 3927}, {52, 3793}, {53, 3663}, {54, 3537}, {55, 3414}, {56, 3293}, {57, 3175}, {58, 3057},
	{59, 2941}, {60, 2825}, {61, 2776}, {62, 2717}, {63, 2652}, {64, 2581}, {65, 2507}, {66, 2431}, {67, 2355}, {68, 2280}, {69, 2206},
	{70, 2135}, {71, 2066}, {72, 2000}, {73, 1937}, {74, 1878}, {75, 1822}, {76, 1769}, {77, 1719}, {78, 1672}, {79, 1628}, {80, 1586},
	{81, 1545}, {82, 1507}, {83, 1470}, {84, 1435}, {85, 1400}, {86, 1366}, {87, 1333}, {88, 1300}, {89, 1268}, {90, 1236}, {91, 1203},
	{92, 1171}, {93, 1139}, {94, 1106}, {95, 1074}, {96, 1042}, {97, 1010}, {98, 978}, {99, 948}, {100, 918}, {101, 888}, {102, 861},
	{103, 834}, {104, 809}, {105, 787}, {106, 766}, {107, 748}, {108, 733}, {109, 721}, {110, 713}

};

uint8_t MeasurementRunning;
uint16_t SensorVoltages[ADC_CHANNEL_COUNT];

/*********************************************************************************
 * Local function prototypes
 *********************************************************************************/

void Sensor_Temperature_ADC_InitGlobal(void);

void Sensor_Temperature_ADC_InitMeasurements(void);

void Sensor_Temperature_ADC_StartConversion(void);

uint16_t Sensor_Temperature_ADC_GetConversionResult(SensorAnalogPort_t* sensor_handle);

double Sensor_Temperature_ConvertToVoltage(uint16_t adcValue);

/*********************************************************************************
 * Local function definitions
 *********************************************************************************/

void Sensor_Temperature_ADC_InitGlobal(void)
{
	/* Initialize an instance of Global hardware */
	XMC_VADC_GLOBAL_Init(ADC_MODULE, &global_config);

	/*Initialize Group 0*/
	{
		XMC_VADC_GROUP_Init((XMC_VADC_GROUP_t *)VADC_G0, (XMC_VADC_GROUP_CONFIG_t *)&group_init_handle0);

		/* Switch on the converter of the Group[group_index]*/
		XMC_VADC_GROUP_SetPowerMode((XMC_VADC_GROUP_t *)VADC_G0, XMC_VADC_GROUP_POWERMODE_NORMAL);

		/* Disable the post calibration option for the respective group*/
		XMC_VADC_GLOBAL_DisablePostCalibration(ADC_MODULE, 0);
	}

	XMC_VADC_GLOBAL_StartupCalibration(ADC_MODULE);
}

void Sensor_Temperature_ADC_InitMeasurements(void)
{
	uint8_t j;
	SensorAnalogPort_t *indexed;

	/*Initialize the Global Conversion class 0*/
	XMC_VADC_GLOBAL_InputClassInit(ADC_MODULE, (XMC_VADC_GLOBAL_CLASS_t)global_iclass_config,
									  XMC_VADC_GROUP_CONV_STD, ADC_MEASUREMENT_ICLASS_NUM);

	/* Initialize the Background Scan hardware */
	XMC_VADC_GLOBAL_BackgroundInit(ADC_MODULE, (XMC_VADC_BACKGROUND_CONFIG_t*) &backgnd_config);

	/* Initialize Channels */
	for (j = 0; j < ADC_CHANNEL_COUNT; j++)
	{
		indexed = &Temperature_Sensors[j];

		/* Initialize for configured channels*/
		XMC_VADC_GROUP_ChannelInit(indexed->group_handle,(uint32_t)indexed->ch_num, indexed->ch_handle);

		/* Set reference voltage*/
		XMC_VADC_GROUP_ChannelSetInputReference(indexed->group_handle, (uint32_t)indexed->ch_num,
				(uint32_t)indexed->ch_handle->alternate_reference);

		/* Initialize for configured result registers */
		XMC_VADC_GROUP_ResultInit(indexed->group_handle, (uint32_t)indexed->ch_handle->result_reg_number,
								  indexed->res_handle);

		/* Add all channels into the Background Request Source Channel Select Register */
		XMC_VADC_GLOBAL_BackgroundAddChannelToSequence(ADC_MODULE,
													   (uint32_t)indexed->group_index, (uint32_t)indexed->ch_num);

	}

	/* Enable Interrupt */
	NVIC_SetPriority((IRQn_Type)backgnd_rs_intr_handle.node_id,
						NVIC_EncodePriority(NVIC_GetPriorityGrouping(),
						backgnd_rs_intr_handle.priority, backgnd_rs_intr_handle.sub_priority));

	/* Connect background Request Source Event to NVIC node */
	XMC_VADC_GLOBAL_BackgroundSetReqSrcEventInterruptNode(ADC_MODULE,
												 (XMC_VADC_SR_t) XMC_VADC_SR_SHARED_SR2);

	/* Enable Background Scan Request source IRQ */
	NVIC_EnableIRQ((IRQn_Type)backgnd_rs_intr_handle.node_id);
}

void Sensor_Temperature_ADC_StartConversion(void)
{
	XMC_VADC_GLOBAL_BackgroundTriggerConversion(ADC_MODULE);
}

uint16_t Sensor_Temperature_ADC_GetConversionResult(SensorAnalogPort_t* sensor_handle)
{
	return XMC_VADC_GROUP_GetResult(sensor_handle->group_handle, sensor_handle->ch_handle->result_reg_number);
}

double Sensor_Temperature_ConvertToVoltage(uint16_t adcValue)
{
	return adcValue * ADC_RESOLUTION;
}

void Adc_Measurement_Handler()
{
	SensorVoltages[TEMPERATURE_SENSOR_A] = Sensor_Temperature_ADC_GetConversionResult(SENSOR_CFG_A);

	MeasurementRunning = 0;
}

/*********************************************************************************
 * Global function definitions
 *********************************************************************************/

void Sensor_Temperature_Init(void)
{
	Sensor_Temperature_ADC_InitGlobal();
	Sensor_Temperature_ADC_InitMeasurements();
}

int Sensor_Temperature_Calculate(Sensor_TemperatureType sensor)
{
	int i = 0;
	double adcVoltage = 0;
	double res = 0;

	/* Start measurement */
	MeasurementRunning = 1;
	Sensor_Temperature_ADC_StartConversion();

	/* Wait for measurement to finish*/
	while(MeasurementRunning == 1);

	/* Calculate temperature */
	adcVoltage = Sensor_Temperature_ConvertToVoltage(SensorVoltages[sensor]);
	res = (adcVoltage * R1) / (SENSOR_REF_VOLTAGE - adcVoltage);

	while(TemperatureLookupTable[i++].resistence > res && i < NTC_LOOKUP_ENTRIES);

	return TemperatureLookupTable[i].temperature;
}
