/*
 * Sensor_Hall.c
 *
 *  Created on: Nov 9, 2016
 *      Author: Andreas Lackner
 */

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include "Sensor_Hall.h"

#include <xmc_ccu4.h>
#include <xmc_posif.h>
#include <xmc_gpio.h>

/*********************************************************************************
 * Local macros
 *********************************************************************************/

#define POSIF_PTR		POSIF0
#define HALL_CCU		CCU40
#define HALL_CCU_NUM	(0U)

#define DELAY_SLICE_PTR 		CCU40_CC40
#define DELAY_SLICE_NUMBER 		(0U)
#define CAPTURE_SLICE_PTR 		CCU40_CC41
#define CAPTURE_SLICE_NUMBER	(1U)

#define HALL_PORT_A	P14_7
#define HALL_PORT_B	P14_6
#define HALL_PORT_C	P14_5

#define HALL_EVENT_IO P5_9

#define GEN_HALL_PATTERN(EHP, CHP) (((uint32_t)EHP << 3) | (uint32_t)CHP)
#define HALL_EMPTY 0

/*********************************************************************************
 * Local data
 *********************************************************************************/

uint32_t hall[3] = { 0,0,0 };
static MotorDirection_t motorDirection = ClockWise;

/* Hall pattern of the motor. This depends on the type and make of the motor selected */
uint8_t hall_pattern_ccw[] =
{
	(uint8_t)GEN_HALL_PATTERN(HALL_EMPTY,HALL_EMPTY),
	(uint8_t)GEN_HALL_PATTERN(3,1),
	(uint8_t)GEN_HALL_PATTERN(6,2),
	(uint8_t)GEN_HALL_PATTERN(2,3),
	(uint8_t)GEN_HALL_PATTERN(5,4),
	(uint8_t)GEN_HALL_PATTERN(1,5),
	(uint8_t)GEN_HALL_PATTERN(4,6)
};

uint8_t hall_pattern_cw[] =
{
	(uint8_t)GEN_HALL_PATTERN(HALL_EMPTY,HALL_EMPTY),
	(uint8_t)GEN_HALL_PATTERN(5,1),
	(uint8_t)GEN_HALL_PATTERN(3,2),
	(uint8_t)GEN_HALL_PATTERN(1,3),
	(uint8_t)GEN_HALL_PATTERN(6,4),
	(uint8_t)GEN_HALL_PATTERN(4,5),
	(uint8_t)GEN_HALL_PATTERN(2,6)
};

//XMC Capture/Compare Unit 4 (CCU4) Configuration for Capture:
XMC_CCU4_SLICE_COMPARE_CONFIG_t hall_delay_config =
{
	.timer_mode = (uint32_t)XMC_CCU4_SLICE_TIMER_COUNT_MODE_EA,
	.monoshot = (uint32_t)true,
	.shadow_xfer_clear = (uint32_t)0,
	.dither_timer_period = (uint32_t)0,
	.dither_duty_cycle = (uint32_t)0,
	.prescaler_mode = (uint32_t)XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
	.mcm_enable = (uint32_t)0,
	.prescaler_initval = (uint32_t)0, /* in this case, prescaler = 2^10 */
	.float_limit = (uint32_t)0,
	.dither_limit = (uint32_t)0,
	.passive_level = (uint32_t)XMC_CCU4_SLICE_OUTPUT_PASSIVE_LEVEL_LOW,
	.timer_concatenation = (uint32_t)0
};

/* Capture Slice configuration */
XMC_CCU4_SLICE_CAPTURE_CONFIG_t hall_capture_config =
{
	.fifo_enable = false,
	.timer_clear_mode = XMC_CCU4_SLICE_TIMER_CLEAR_MODE_ALWAYS,
	.same_event = false,
	.ignore_full_flag = true,
	.prescaler_mode = XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
	.prescaler_initval = (uint32_t)5, /* in this case, prescaler = 2^5 */
	.float_limit = (uint32_t)0,
	.timer_concatenation = (uint32_t)0
};

XMC_CCU4_SLICE_EVENT_CONFIG_t hall_start_event0_config = //off time capture
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_E, //CAPTURE on POSIF0.OUT0
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

XMC_CCU4_SLICE_EVENT_CONFIG_t hall_capture_event0_config = //off time capture
{
	.mapped_input = XMC_CCU4_SLICE_INPUT_F, //CAPTURE on POSIF0.OUT1
	.edge = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
	.level = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
	.duration = XMC_CCU4_SLICE_EVENT_FILTER_DISABLED
};

XMC_POSIF_CONFIG_t POSIF_HALL_config =
{
	.mode = XMC_POSIF_MODE_HALL_SENSOR, /**< POSIF Operational mode */
	.input0 = XMC_POSIF_INPUT_PORT_B, /**< Choice of input for Input-1 */
	.input1 = XMC_POSIF_INPUT_PORT_B, /**< Choice of input for Input-2 */
	.input2 = XMC_POSIF_INPUT_PORT_B, /**< Choice of input for Input-3 */
	.filter = XMC_POSIF_FILTER_DISABLED /**< Input filter configuration */
};

XMC_POSIF_HSC_CONFIG_t POSIF_HSC_config =
{
	.disable_idle_signal = 1,
	.sampling_trigger = 0, //HSDA
	.sampling_trigger_edge = 0 //Rising edge
};

XMC_GPIO_CONFIG_t HALL_POSIF_0_Hall_PadConfig =
{
	.mode = (XMC_GPIO_MODE_t)XMC_GPIO_MODE_INPUT_PULL_DOWN,
	.output_level = (XMC_GPIO_OUTPUT_LEVEL_t)XMC_GPIO_OUTPUT_LEVEL_LOW,
};

/*********************************************************************************
 * Local function prototypes
 *********************************************************************************/
MotorDirection_t Sensor_Get_Direction(void);

void Sensor_Hall_InitPattern(void);

void Sensor_Hall_SetActivePattern(uint8_t hallposition);

uint8_t Sensor_Hall_GetPattern(uint8_t currentPattern);

/*********************************************************************************
 * Local function definitions
 *********************************************************************************/

void Sensor_Hall_InitPattern()
{
	uint8_t hallposition;

	hall[0] = XMC_GPIO_GetInput(HALL_PORT_A);
	hall[1] = XMC_GPIO_GetInput(HALL_PORT_B);
	hall[2] = XMC_GPIO_GetInput(HALL_PORT_C);

	hallposition = (uint32_t)((hall[0] | (hall[1] << 1) | (hall[2] << 2)));

	//Get the current hall pattern and push it to the registers
	XMC_POSIF_HSC_SetHallPatterns(POSIF_PTR, Sensor_Hall_GetPattern(hallposition));
	XMC_POSIF_HSC_UpdateHallPattern(POSIF_PTR);

	//Save the active hall position for queries
	Sensor_Hall_SetActivePattern(hallposition);
}

void Sensor_Hall_SetActivePattern(uint8_t hallposition)
{
	ActiveHallPattern.h1 = hallposition & 0x1;
	ActiveHallPattern.h2 = (hallposition & 0x2) >> 1;
	ActiveHallPattern.h3 = (hallposition & 0x4) >> 2;
}

uint8_t Sensor_Hall_GetPattern(uint8_t currentPattern)
{
	switch(motorDirection)
	{
	case ClockWise:
		return hall_pattern_cw[currentPattern];
	case CounterClockWise:
		return hall_pattern_ccw[currentPattern];
	}

	return 0;
}

/*********************************************************************************
 * Global function definitions
 *********************************************************************************/

MotorDirection_t Sensor_GetDirection(void)
{
	return motorDirection;
}

void POSIF0_0_IRQHandler(void)
{
	uint8_t hallposition;

    XMC_GPIO_ToggleOutput(P1_9);

	/* Set the new Hall pattern */
	hallposition = XMC_POSIF_HSC_GetExpectedPattern(POSIF_PTR);
	XMC_POSIF_HSC_SetHallPatterns(POSIF_PTR, Sensor_Hall_GetPattern(hallposition));
	XMC_POSIF_HSC_UpdateHallPattern(POSIF_PTR);

	//Save the active hall position for queries
	Sensor_Hall_SetActivePattern(hallposition);

	/* Peak */
	XMC_GPIO_ToggleOutput(HALL_EVENT_IO);

	SensorHallCallback();
}

void Sensor_Hall_Init()
{
	XMC_GPIO_Init(HALL_PORT_A, &HALL_POSIF_0_Hall_PadConfig);
	XMC_GPIO_Init(HALL_PORT_B, &HALL_POSIF_0_Hall_PadConfig);
	XMC_GPIO_Init(HALL_PORT_C, &HALL_POSIF_0_Hall_PadConfig);

	XMC_GPIO_SetMode(HALL_EVENT_IO, XMC_GPIO_MODE_OUTPUT_PUSH_PULL);

	/* Enable clock, enable prescaler block and configure global control */
	XMC_CCU4_Init(HALL_CCU, XMC_CCU4_SLICE_MCMS_ACTION_TRANSFER_PR_CR);

	/* Start the prescaler and restore clocks to slices */
	XMC_CCU4_StartPrescaler(HALL_CCU);

	/* Ensure fCCU reaches CCU40, CCU80 */
	XMC_CCU4_SetModuleClock(HALL_CCU, XMC_CCU4_CLOCK_SCU);

	/* Configure CCU4 slices as monoshot and capture slice */
	XMC_CCU4_SLICE_CompareInit(DELAY_SLICE_PTR, &hall_delay_config);
	XMC_CCU4_SLICE_CaptureInit(CAPTURE_SLICE_PTR, &hall_capture_config);

	/* Configure CCU4 delay as 1us - CCU40.ST0 is connected to POSIF.HSDA*/
	XMC_CCU4_SLICE_SetTimerPeriodMatch(DELAY_SLICE_PTR, 127U);
	XMC_CCU4_SLICE_SetTimerCompareMatch(DELAY_SLICE_PTR, 64U);
	XMC_CCU4_SLICE_SetTimerPeriodMatch(CAPTURE_SLICE_PTR, 65535U);

	/* Transfer value from shadow timer registers to actual timer registers */
	XMC_CCU4_EnableShadowTransfer(HALL_CCU, (uint32_t)(XMC_CCU4_SHADOW_TRANSFER_SLICE_0 | XMC_CCU4_SHADOW_TRANSFER_SLICE_1));

	/* Configure and enable events */
	XMC_CCU4_SLICE_StartConfig(DELAY_SLICE_PTR, XMC_CCU4_SLICE_EVENT_0, XMC_CCU4_SLICE_START_MODE_TIMER_START_CLEAR);
	XMC_CCU4_SLICE_Capture0Config(CAPTURE_SLICE_PTR, XMC_CCU4_SLICE_EVENT_0);
	XMC_CCU4_SLICE_ConfigureEvent(CAPTURE_SLICE_PTR, XMC_CCU4_SLICE_EVENT_0, &hall_capture_event0_config);
	XMC_CCU4_SLICE_ConfigureEvent(DELAY_SLICE_PTR, XMC_CCU4_SLICE_EVENT_0, &hall_start_event0_config);

	/* Get the slice out of idle mode */
	XMC_CCU4_EnableClock(HALL_CCU, DELAY_SLICE_NUMBER);
	XMC_CCU4_EnableClock(HALL_CCU, CAPTURE_SLICE_NUMBER);

	/* POSIF Configuration */
	XMC_POSIF_Init(POSIF_PTR, &POSIF_HALL_config);
	XMC_POSIF_HSC_Init(POSIF_PTR, &POSIF_HSC_config);
	XMC_POSIF_EnableEvent(POSIF_PTR, XMC_POSIF_IRQ_EVENT_CHE);

	/* Connect correct hall event to SR0 */
	XMC_POSIF_SetInterruptNode(POSIF_PTR, XMC_POSIF_IRQ_EVENT_CHE, XMC_POSIF_SR_ID_0);

	/* Configure NVIC */
	/* Set priority */
	NVIC_SetPriority(POSIF0_0_IRQn, 0U);
	/* Enable IRQ */
	NVIC_EnableIRQ(POSIF0_0_IRQn);

	//Initialize the hall pattern
	Sensor_Hall_InitPattern();
}

void Sensor_Hall_Start()
{
	/* Start Timer Running */
	XMC_CCU4_SLICE_StartTimer(CAPTURE_SLICE_PTR);

	/* Start the POSIF module*/
	XMC_POSIF_Start(POSIF_PTR);
}

void Sensor_Hall_Stop()
{
	/* Start Timer Running */
	XMC_CCU4_SLICE_StopTimer(CAPTURE_SLICE_PTR);

	/* Start the POSIF module*/
	XMC_POSIF_Stop(POSIF_PTR);
}

void Sensor_Hall_SetDirection(MotorDirection_t direction)
{
	motorDirection = direction;
}
