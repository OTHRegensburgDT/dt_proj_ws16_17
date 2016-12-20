/*
 * Sensor_QuadratureDecoder.c
 *
 *  Created on: Dec 11, 2016
 *      Author: Andreas Lackner
 */

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include <xmc_posif.h>
#include <xmc_gpio.h>
#include <xmc_ccu4.h>

/*********************************************************************************
 * Local macros
 *********************************************************************************/

#define POSIF_PTR	POSIF1

#define CCU_PTR		CCU40
#define CCU_NUMBER 	(0U)

#define SLICE0_PTR 		CCU41_CC40
#define SLICE0_NUMBER 	(0U)

#define SLICE1_PTR 		CCU41_CC41
#define SLICE1_NUMBER 	(1U)

#define SLICE2_PTR 		CCU41_CC42
#define SLICE2_NUMBER 	(2U)

#define SLICE3_PTR 		CCU41_CC43
#define SLICE3_NUMBER 	(3U)

#define QD_INDEX	P3_8
#define QD_A		P3_10
#define QD_B		P3_9

/*********************************************************************************
 * Local data
 *********************************************************************************/

/************************* CCU Configuration **********************************/

/* CCU4 Configuration for Position Count */
XMC_CCU4_SLICE_COMPARE_CONFIG_t position_config =
{
		  .timer_mode          = XMC_CCU4_SLICE_TIMER_COUNT_MODE_EA,
		  .monoshot            = 0U,
		  .shadow_xfer_clear   = 1U,
		  .dither_timer_period = 0U,
		  .dither_duty_cycle   = 0U,
		  .prescaler_mode      = (uint32_t)XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
		  .mcm_enable          = 0U,
		  .prescaler_initval   = 0U,
		  .float_limit         = 0U,
		  .dither_limit        = 0U,
		  .passive_level       = (uint32_t)XMC_CCU4_SLICE_OUTPUT_PASSIVE_LEVEL_LOW,
		  .timer_concatenation = 0U
};

/* Event 0: Counts up on rising edge of POSIF0.OUT0 (Quadrature Clock) */
XMC_CCU4_SLICE_EVENT_CONFIG_t position_event0_config =
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_E, /* mapped to POSIF0.OUT0 - POSITION tick */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_COUNT_UP_ON_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Event 1: Count direction of POSIF0.OUT1 (Quadrature Direction)
 * OUT1 signal is asserted high when motor is rotating clockwise */
XMC_CCU4_SLICE_EVENT_CONFIG_t position_event1_config=
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_F, /* mapped to POSIF0.OUT1 - Direction */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Configuration for Revolution Count */
XMC_CCU4_SLICE_COMPARE_CONFIG_t revolution_config =
{
		  .timer_mode          = XMC_CCU4_SLICE_TIMER_COUNT_MODE_EA,
		  .monoshot            = 0U,
		  .shadow_xfer_clear   = 1U,
		  .dither_timer_period = 0U,
		  .dither_duty_cycle   = 0U,
		  .prescaler_mode      = (uint32_t)XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
		  .mcm_enable          = 0U,
		  .prescaler_initval   = 0U,
		  .float_limit         = 0U,
		  .dither_limit        = 0U,
		  .passive_level       = (uint32_t)XMC_CCU4_SLICE_OUTPUT_PASSIVE_LEVEL_LOW,
		  .timer_concatenation = 0U
};


/* Event 0: Counts up on rising edge of Index signal direct from Encoder */
XMC_CCU4_SLICE_EVENT_CONFIG_t revolution_event0_config=
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_B, /* mapped to index input P14.5 to P2.8 */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Event 1: Count direction of POSIF0.OUT1 (Quadrature Direction)
 * OUT1 signal is asserted high when motor is rotating clockwise */
XMC_CCU4_SLICE_EVENT_CONFIG_t revolution_event1_config=
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_F, /* mapped to POSIF0.OUT1 - Direction  */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_COUNT_UP_ON_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Configuration for tick Count */
XMC_CCU4_SLICE_COMPARE_CONFIG_t tick_config =
{
		  .timer_mode          = XMC_CCU4_SLICE_TIMER_COUNT_MODE_EA,
		  .monoshot            = 0U,
		  .shadow_xfer_clear   = 1U,
		  .dither_timer_period = 0U,
		  .dither_duty_cycle   = 0U,
		  .prescaler_mode      = (uint32_t)XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
		  .mcm_enable          = 0U,
		  .prescaler_initval   = 0U,
		  .float_limit         = 0U,
		  .dither_limit        = 0U,
		  .passive_level       = (uint32_t)XMC_CCU4_SLICE_OUTPUT_PASSIVE_LEVEL_LOW,
		  .timer_concatenation = 0U
};

/* Event 0: Counts up on rising edge of POSIF0.OUT2 (Period Clock) */
XMC_CCU4_SLICE_EVENT_CONFIG_t tick_event0_config=
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_F, /* mapped to POSIF0.OUT2 - Period Clock  */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* As the motor encoder produces an index signal that is less than 1/2 tick period,
 * the index is connected directly to the CCU4 module for this example.
 *
 * Since a board connection is not directly available on the General Purpose Motor
 * Drive Card, we need to connect P14.5 (index signal) to P2.8 (CCU40.41 event0 input)
 *
 * Event 1: Counts up on rising edge of Index signal direct from Encoder */
XMC_CCU4_SLICE_EVENT_CONFIG_t tick_event1_config=
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_B, /* mapped to index input P14.5 to P2.8 */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Configuration for capture count */
XMC_CCU4_SLICE_CAPTURE_CONFIG_t capture_config =
{
	.fifo_enable = false,
	.timer_clear_mode = XMC_CCU4_SLICE_TIMER_CLEAR_MODE_ALWAYS,
	.same_event = false,
	.ignore_full_flag = false,
	.prescaler_mode = XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
	.prescaler_initval = (uint32_t) 6,		/* set the prescaler to 6; ccu4 resolution = 530ns */
	.float_limit = (uint32_t) 0,
	.timer_concatenation = (uint32_t) 0
};

/* Event 0: Start the slice on rising edge of POSIF0.OUT5 (Sync Start)  */
XMC_CCU4_SLICE_EVENT_CONFIG_t capture_event0_config =
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_F, 	/* POSIF0.OUT5 - SYNC Start  */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Event 1: Capture on rising edge of CCU40.ST2 */
XMC_CCU4_SLICE_EVENT_CONFIG_t capture_event1_config =
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_O, 		/* CAPTURE on CCU40.ST2 rising edge */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/* Event 2: Capture on falling edge of CCU40.ST2 */
XMC_CCU4_SLICE_EVENT_CONFIG_t capture_event2_config =
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_O, 		/* CAPTURE on CCU40.ST2 falling edge */
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_FALLING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

/************************* POSIF Configuration **********************************/

XMC_POSIF_CONFIG_t posif_config =
{
   .mode   = XMC_POSIF_MODE_QD,    		/**< POSIF Operational mode */
   .input0 = XMC_POSIF_INPUT_PORT_B,    /**< Choice of input for Input-1 */
   .input1 = XMC_POSIF_INPUT_PORT_B,    /**< Choice of input for Input-2 */
   .input2 = XMC_POSIF_INPUT_PORT_B,   	/**< Choice of input for Input-3 */
   .filter = XMC_POSIF_FILTER_DISABLED, /**< Input filter configuration */
};

/* Defines POSIF quadrature decoder initialization data structure. */
XMC_POSIF_QD_CONFIG_t posif_qd_config =
{
  .mode                = (XMC_POSIF_QD_MODE_t)0,
  .phase_a             = 0U,	/* Phase A is active High */
  .phase_b             = 0U,	/* Phase B is active High */
  .phase_leader        = 0U,	/* Phase A is the leading signal for clockwise rotation */
  .index               = 0U		/* No index marker generation on POSIF0.OUT3 */
};

/* GPIO Init handle for inverter enable Pin */
XMC_GPIO_CONFIG_t posif_encoder_port_config =
{
  .mode             = XMC_GPIO_MODE_OUTPUT_PUSH_PULL,
  .output_level     = XMC_GPIO_OUTPUT_LEVEL_HIGH,
  .output_strength  = XMC_GPIO_OUTPUT_STRENGTH_STRONG_SHARP_EDGE,
};

/* GPIO Init handle for input Pin */
/* Port 14 digital pad input is disabled by default, this is to enable the digital pad input */
XMC_GPIO_CONFIG_t posif_encoder_inputport_config =
{
  .mode             = XMC_GPIO_MODE_INPUT_PULL_UP,
  .output_level     = XMC_GPIO_OUTPUT_LEVEL_HIGH,
  .output_strength  = XMC_GPIO_OUTPUT_STRENGTH_STRONG_SHARP_EDGE,
};

/*********************************************************************************
 * Local function prototypes
 *********************************************************************************/

void Sensor_QD_InitCCU();

void Sensor_QD_InitPosif();

/*********************************************************************************
 * Local function definitions
 *********************************************************************************/

void Sensor_QD_InitCCU()
{
	/* Enable clock, enable prescaler block and configure global control */
	XMC_CCU4_Init(CCU_PTR, XMC_CCU4_SLICE_MCMS_ACTION_TRANSFER_PR_CR);

	/* Start the prescaler and restore clocks to slices */
	XMC_CCU4_StartPrescaler(CCU_PTR);

	/* Start of CCU4 configurations */
	XMC_CCU4_SetModuleClock(CCU_PTR, XMC_CCU4_CLOCK_SCU);

	/* Initialize the Slice */
	XMC_CCU4_SLICE_CompareInit(SLICE0_PTR, &position_config);
	XMC_CCU4_SLICE_CompareInit(SLICE1_PTR, &revolution_config);
	XMC_CCU4_SLICE_CompareInit(SLICE2_PTR, &tick_config);
	XMC_CCU4_SLICE_CaptureInit(SLICE3_PTR, &capture_config);

	/* Program the compare register */
	XMC_CCU4_SLICE_SetTimerCompareMatch(SLICE0_PTR,1000U);
	XMC_CCU4_SLICE_SetTimerPeriodMatch(SLICE0_PTR, 3000U);

	XMC_CCU4_SLICE_SetTimerCompareMatch(SLICE1_PTR,1000U);
	XMC_CCU4_SLICE_SetTimerPeriodMatch(SLICE1_PTR, 2000U);

	/* This defined the trigger for the capture. In this example,
	 * a captured event is taken for every 5 ticks of period clock.  */
	XMC_CCU4_SLICE_SetTimerCompareMatch(SLICE2_PTR,5U);
	XMC_CCU4_SLICE_SetTimerPeriodMatch(SLICE2_PTR, 9U);

	/* Enable shadow transfer */
	XMC_CCU4_EnableShadowTransfer(CCU_PTR, \
			(uint32_t)(XMC_CCU4_SHADOW_TRANSFER_SLICE_0| \
					   XMC_CCU4_SHADOW_TRANSFER_PRESCALER_SLICE_0));

	XMC_CCU4_EnableShadowTransfer(CCU_PTR, \
			(uint32_t)(XMC_CCU4_SHADOW_TRANSFER_SLICE_1| \
					   XMC_CCU4_SHADOW_TRANSFER_PRESCALER_SLICE_1));

	XMC_CCU4_EnableShadowTransfer(CCU_PTR, \
			(uint32_t)(XMC_CCU4_SHADOW_TRANSFER_SLICE_2| \
					   XMC_CCU4_SHADOW_TRANSFER_PRESCALER_SLICE_2));

	XMC_CCU4_EnableShadowTransfer(CCU_PTR, \
				(uint32_t)(XMC_CCU4_SHADOW_TRANSFER_SLICE_3| \
						 XMC_CCU4_SHADOW_TRANSFER_PRESCALER_SLICE_3));

	/* Configure CC40 event0 - Count on rising edge of Quadrature Clock (POSIF0.OUT0) */
	XMC_CCU4_SLICE_CountConfig(SLICE0_PTR, XMC_CCU4_SLICE_EVENT_0);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE0_PTR, XMC_CCU4_SLICE_EVENT_0, &position_event0_config);

	/* Configure CC40 event1 - Set up count direction on (POSIF0.OUT1) */
	XMC_CCU4_SLICE_DirectionConfig(SLICE0_PTR, XMC_CCU4_SLICE_EVENT_1);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE0_PTR, XMC_CCU4_SLICE_EVENT_1, &position_event1_config);

	/* Configure CC41 event0 - Count on rising edge of POSIF index signal */
	XMC_CCU4_SLICE_CountConfig(SLICE1_PTR, XMC_CCU4_SLICE_EVENT_0);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE1_PTR, XMC_CCU4_SLICE_EVENT_0, &revolution_event0_config);

	/* Configure CC41 event1 - Set up count direction on (POSIF0.OUT1) */
	XMC_CCU4_SLICE_DirectionConfig(SLICE1_PTR, XMC_CCU4_SLICE_EVENT_1);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE1_PTR, XMC_CCU4_SLICE_EVENT_1, &revolution_event1_config);

	/* Configure CC42 event0 - Count on rising edge of Period Clock (POSIF0.OUT2) */
	XMC_CCU4_SLICE_CountConfig(SLICE2_PTR, XMC_CCU4_SLICE_EVENT_0);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE2_PTR, XMC_CCU4_SLICE_EVENT_0, &tick_event0_config);

	/* Configure CC42 event1 - Set up external flush and start on POSIF index signal */
	XMC_CCU4_SLICE_StartConfig(SLICE2_PTR, XMC_CCU4_SLICE_EVENT_1, XMC_CCU4_SLICE_START_MODE_TIMER_START_CLEAR);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE2_PTR, XMC_CCU4_SLICE_EVENT_1, &tick_event1_config);

	/* Configure CC43 event0 - Set up external start on POSIF0.OUT5 (Sync Start) */
	XMC_CCU4_SLICE_StartConfig(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_0, XMC_CCU4_SLICE_START_MODE_TIMER_START_CLEAR);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_0, &capture_event0_config);

	/* Configure CC43 event1 - Set up capture event on rising edge of CCU40.ST2/RISING */
	XMC_CCU4_SLICE_Capture0Config(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_1);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_1, &capture_event1_config);

	/* Configure CC43 event1 - Set up capture event on rising edge of CCU40.ST2/FALLING */
	XMC_CCU4_SLICE_Capture1Config(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_2);
	XMC_CCU4_SLICE_ConfigureEvent(SLICE3_PTR, XMC_CCU4_SLICE_EVENT_2, &capture_event2_config);

	/* Enable CC43 event  */
	XMC_CCU4_SLICE_EnableEvent(SLICE3_PTR, XMC_CCU4_SLICE_IRQ_ID_EVENT2);

	/* Connect capture on CC43 event 2 to SR0 */
	XMC_CCU4_SLICE_SetInterruptNode(SLICE3_PTR, XMC_CCU4_SLICE_IRQ_ID_EVENT2, XMC_CCU4_SLICE_SR_ID_0);

	/* Set NVIC priority */
	NVIC_SetPriority(CCU41_0_IRQn, 3U);

	/* Enable IRQ */
	NVIC_EnableIRQ(CCU41_0_IRQn);
}

void Sensor_QD_InitPosif()
{
	/* POSIF Initialization */
	XMC_POSIF_Init(POSIF_PTR, &posif_config);
	XMC_POSIF_QD_Init(POSIF_PTR, &posif_qd_config);

	XMC_GPIO_Init(QD_A, &posif_encoder_inputport_config); /* A */
	XMC_GPIO_Init(QD_B, &posif_encoder_inputport_config); /* B */
	XMC_GPIO_Init(QD_INDEX, &posif_encoder_inputport_config); /* index */

	/* Get the CCU4 slice out of idle mode */
	XMC_CCU4_EnableClock(CCU_PTR, SLICE0_NUMBER);
	XMC_CCU4_EnableClock(CCU_PTR, SLICE1_NUMBER);
	XMC_CCU4_EnableClock(CCU_PTR, SLICE2_NUMBER);
	XMC_CCU4_EnableClock(CCU_PTR, SLICE3_NUMBER);
}

void CCU41_0_IRQHandler(void)
{
	uint16_t capturedvalue0;
	uint16_t capturedvalue1;

	/* Clear pending interrupt */
	XMC_CCU4_SLICE_ClearEvent(SLICE3_PTR, XMC_CCU4_SLICE_IRQ_ID_EVENT2);

	capturedvalue0 = XMC_CCU4_SLICE_GetCaptureRegisterValue(SLICE3_PTR,1U);
	capturedvalue1 = XMC_CCU4_SLICE_GetCaptureRegisterValue(SLICE3_PTR,3U);
}

/*********************************************************************************
 * Global function definitions
 *********************************************************************************/

void Sensor_QD_Init()
{
	Sensor_QD_InitCCU();
	Sensor_QD_InitPosif();
}

void Sensor_QD_Start()
{
	/* Start the CCU4 Timer */
	XMC_CCU4_SLICE_StartTimer(SLICE0_PTR);
	XMC_CCU4_SLICE_StartTimer(SLICE1_PTR);
	XMC_CCU4_SLICE_StartTimer(SLICE2_PTR);

	/* Start the Encoder */
	XMC_POSIF_Start(POSIF0);
}

void Sensor_QD_Stop()
{

}
