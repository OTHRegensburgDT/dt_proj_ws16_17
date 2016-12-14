/*
 * Posif_Control.c
 *
 *  Created on: Nov 29, 2016
 *      Author: MotorXP
 */

#include <xmc_posif.h>

#include "Sensor_Hall.h"
#include "Posif_Control.h"

static volatile uint16_t pwmpattern = 0;

/* XMC System Clock Unit (SCU) Configuration: */
/* PWM period is calculated based on PCLK which is equivalent to 64 MHz. */
/* TODO: validierung clock speed setup */

XMC_SCU_CLOCK_CONFIG_t clock_config =
{
    .pclk_src = XMC_SCU_CLOCK_PCLKSRC_DOUBLE_MCLK,
    .rtc_src = XMC_SCU_CLOCK_RTCCLKSRC_DCO2,
    .fdiv = 0,
    .idiv = 1,
};
XMC_CCU4_SLICE_COMPARE_CONFIG_t CCU40_SLICE_config =
{
    .timer_mode             = (uint32_t) XMC_CCU4_SLICE_TIMER_COUNT_MODE_EA,
    .monoshot               = (uint32_t) false,
    .shadow_xfer_clear      = (uint32_t) 0,
    .dither_timer_period    = (uint32_t) 0,
    .dither_duty_cycle      = (uint32_t) 0,
    .prescaler_mode         = (uint32_t) XMC_CCU4_SLICE_PRESCALER_MODE_NORMAL,
    .mcm_enable             = (uint32_t) 0,
    .prescaler_initval      = (uint32_t) 0,
    .float_limit            = (uint32_t) 0,
    .dither_limit           = (uint32_t) 0,
    .passive_level          = (uint32_t) XMC_CCU4_SLICE_OUTPUT_PASSIVE_LEVEL_LOW,
    .timer_concatenation    = (uint32_t) 0
};

XMC_CCU4_SLICE_EVENT_CONFIG_t CCU40_SLICE_event0_config =
{
    .mapped_input   = XMC_CCU4_SLICE_INPUT_I,
    /* mapped to SCU.GSC40 */
    .edge           = XMC_CCU4_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
    .level          = XMC_CCU4_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_HIGH,
    .duration       = XMC_CCU4_SLICE_EVENT_FILTER_3_CYCLES
};

XMC_POSIF_CONFIG_t POSIF_HALL_config =
{
	.mode = XMC_POSIF_MODE_HALL_SENSOR, /**< POSIF Operational mode */
	.input0 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-1 */
	.input1 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-2 */
	.input2 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-3 */
	.filter = XMC_POSIF_FILTER_DISABLED /**< Input filter configuration */
};

XMC_CCU8_SLICE_COMPARE_CONFIG_t CCU80_SLICE_config =
{
    .timer_mode             = (uint32_t)XMC_CCU8_SLICE_TIMER_COUNT_MODE_EA,
    .monoshot               = (uint32_t)XMC_CCU8_SLICE_TIMER_REPEAT_MODE_REPEAT,
    .shadow_xfer_clear      = 0U
    .dither_timer_period    = 0U,
    .dither_duty_cycle      = 0U,
    .prescaler_mode         = 1U,
    .mcm_ch1_enable         = 1U,
    .mcm_ch2_enable         = (uint32_t) XMC_CCU8_SLICE_STATUS_CHANNEL_1, 
    .slice_status           = (uint32_t) XMC_CCU8_SLICE_OUTPUT_PASIVE_LEVEL_LOW, 
    .passive_level_out0     = (uint32_t) XMC_CCU8_SLICE_OUTPUT_PASIVE_LEVEL_LOW, 
    .passive_level_out1     = (uint32_t) XMC_CCU8_SLICE_OUTPUT_PASIVE_LEVEL_LOW, 
    .passive_level_out2     = (uint32_t) XMC_CCU8_SLICE_OUTPUT_PASIVE_LEVEL_LOW, 
    .passive_level_out3     = 0U,
    .asymmetric_pwm         = 0U,
    .invert_out0            = 0U,
    .invert_out1            = 1U,
    .invert_out2            = 0U,
    .invert_out3            = 1U,
    .prescaler_initval      = 0U,
    .float_limit            = 0U,
    .dither_limit           = 0U,
    .timer_concatenation    = 0U,
};

XMC_CCU8_SLICE_EVENT_CONFIG_t CCU80_SLICE_event0_config =
{
    .mapped_input   = XMC_CCU8_SLICE_INPUT_H, //Connected to SCU.GSC80
    .edge           = XMC_CCU8_SLICE_EVENT_EDGE_SENSITIVITY_RISING_EDGE,
    .level          = XMC_CCU8_SLICE_EVENT_LEVEL_SENSITIVITY_ACTIVE_LOW,
    .duration       = XMC_CCU8_SLICE_EVENT_FILTER_DISABLED,
};
XMC_POSIF_CONFIG_t POSIF_config =
{
    .mode   = XMC_POSIF_MODE_MCM,
    /**< POSIF Operational mode */
    .input0 = XMC_POSIF_INPUT_PORT_A,
    /**< Choice of input for Input-1 */
    .input1 = XMC_POSIF_INPUT_PORT_A,
    /**< Choice of input for Input-2 */
    .input2 = XMC_POSIF_INPUT_PORT_A,
    /**< Choice of input for Input-3 */
    .filter = XMC_POSIF_FILTER_DISABLED
    /**< Input filter configuration */
};

/* Configuration for POSIF - Multi-Channel Mode update settings */
XMC_POSIF_MCM_CONFIG_t POSIF_MCM_config =
{
    .pattern_sw_update      = (uint8_t)false,
    .pattern_update_trigger = XMC_POSIF_INPUT_PORT_A,
    .pattern_trigger_edge   = XMC_POSIF_HSC_TRIGGER_EDGE_RISING,
    .pwm_sync               = (uint8_t)XMC_POSIF_INPUT_PORT_A
};

/* XMC GPIO Configuration: P1_0 */
XMC_GPIO_CONFIG_t CCU40_SLICE_OUTPUT_config =
{
    .mode = XMC_GPIO_MODE_OUTPUT_PUSH_PULL_ALT2,
    .input_hysteresis = XMC_GPIO_INPUT_HYSTERESIS_STANDARD,
    .output_level = XMC_GPIO_OUTPUT_LEVEL_LOW,
};

/* Configuration for standard pads: Port0;[0:7] */
XMC_GPIO_CONFIG_t CCU80_SLICE_OUTPUT_config =
{
    .mode = XMC_GPIO_MODE_OUTPUT_PUSH_PULL_ALT5,
    .input_hysteresis = XMC_GPIO_INPUT_HYSTERESIS_STANDARD,
    .output_level = XMC_GPIO_OUTPUT_LEVEL_LOW,
};

/* CCU40 Compare Match ISR for the new multi channel pattern. */
void CCU40_0_IRQHandler(void)
{
    /* Acknowledge CCU4 compare match event*/
    XMC_CCU4_SLICE_ClearEvent(CCU40_SLICE0_PTR, \
    XMC_CCU4_SLICE_IRQ_ID_COMPARE_MATCH_UP);
    /* Increment and prepare for the next PWM pattern */
    PWMPATTERN++;
    /* Write new multichannel pattern */
    XMC_POSIF_MCM_SetMultiChannelPattern(POSIF_PTR, PWMPATTERN);
}

void initPosifMCM(void)
{
    /* Ensure clock frequency is set at 64MHz (2*MCLK) */
    XMC_SCU_CLOCK_Init(&clock_config);
    /* Enable clock, enable prescaler block and configure global control */
    XMC_CCU4_Init(CCU40_MODULE_PTR, XMC_CCU4_SLICE_MCMS_ACTION_TRANSFER_PR_CR);
    XMC_CCU8_Init(CCU80_MODULE_PTR, XMC_CCU8_SLICE_MCMS_ACTION_TRANSFER_PR_CR);
    /* Start the prescaler and restore clocks to slices */
    XMC_CCU4_StartPrescaler(CCU40_MODULE_PTR);
    XMC_CCU8_StartPrescaler(CCU80_MODULE_PTR);
    /* Ensure fCCU reaches CCU40, CCU80 */
    XMC_CCU4_SetModuleClock(CCU40_MODULE_PTR, XMC_CCU4_CLOCK_SCU);
    XMC_CCU8_SetModuleClock(CCU80_MODULE_PTR, XMC_CCU8_CLOCK_SCU);
    /* Configure CCU8x_CC8y slice as timer */
    XMC_CCU4_SLICE_CompareInit(CCU40_SLICE0_PTR, &CCU40_SLICE_config);
    XMC_CCU8_SLICE_CompareInit(CCU80_SLICE0_PTR, &CCU80_SLICE_config);
    XMC_CCU8_SLICE_CompareInit(CCU80_SLICE1_PTR, &CCU80_SLICE_config);
    /* Set period match value of the timer */
    XMC_CCU4_SLICE_SetTimerPeriodMatch(CCU40_SLICE0_PTR, 3199U);
    XMC_CCU8_SLICE_SetTimerPeriodMatch(CCU80_SLICE0_PTR, 3199U);
    XMC_CCU8_SLICE_SetTimerPeriodMatch(CCU80_SLICE1_PTR, 3199U);
    /* Set timer compare match value for channel (50%) of period */
    XMC_CCU4_SLICE_SetTimerCompareMatch(CCU40_SLICE0_PTR, 1600U);
    XMC_CCU8_SLICE_SetTimerCompareMatch(CCU80_SLICE0_PTR, \
        XMC_CCU8_SLICE_COMPARE_CHANNEL_1, 1600U);
    XMC_CCU8_SLICE_SetTimerCompareMatch(CCU80_SLICE0_PTR, \
        XMC_CCU8_SLICE_COMPARE_CHANNEL_2, 1600U);
    XMC_CCU8_SLICE_SetTimerCompareMatch(CCU80_SLICE1_PTR, \
        XMC_CCU8_SLICE_COMPARE_CHANNEL_1, 1600U);
    XMC_CCU8_SLICE_SetTimerCompareMatch(CCU80_SLICE1_PTR, \
        XMC_CCU8_SLICE_COMPARE_CHANNEL_2, 1600U);
    /* Transfer value from shadow timer registers to actual timer registers */
    XMC_CCU4_EnableShadowTransfer(CCU40_MODULE_PTR, \
        (uint32_t) XMC_CCU4_SHADOW_TRANSFER_SLICE_0);
    XMC_CCU8_EnableShadowTransfer(CCU80_MODULE_PTR, (uint32_t) \
        (XMC_CCU8_SHADOW_TRANSFER_SLICE_0|XMC_CCU8_SHADOW_TRANSFER_SLICE_1));
    /* Configure events */
    XMC_CCU4_SLICE_ConfigureEvent(CCU40_SLICE0_PTR, \
        XMC_CCU4_SLICE_EVENT_0, &CCU40_SLICE_event0_config);
    XMC_CCU8_SLICE_ConfigureEvent(CCU80_SLICE0_PTR, \
        XMC_CCU8_SLICE_EVENT_0, &CCU80_SLICE_event0_config);
    XMC_CCU8_SLICE_ConfigureEvent(CCU80_SLICE1_PTR, \
        XMC_CCU8_SLICE_EVENT_0, &CCU80_SLICE_event0_config);
    XMC_CCU4_SLICE_StartConfig(CCU40_SLICE0_PTR, \
        XMC_CCU4_SLICE_EVENT_0, XMC_CCU4_SLICE_START_MODE_TIMER_START_CLEAR);
    XMC_CCU8_SLICE_StartConfig(CCU80_SLICE0_PTR, \
        XMC_CCU8_SLICE_EVENT_0, XMC_CCU8_SLICE_START_MODE_TIMER_START_CLEAR);
    XMC_CCU8_SLICE_StartConfig(CCU80_SLICE1_PTR, \
        XMC_CCU8_SLICE_EVENT_0, XMC_CCU8_SLICE_START_MODE_TIMER_START_CLEAR);
    /* Enable events */
    /*Enable Compare match event for pattern update*/
    XMC_CCU4_SLICE_EnableEvent(CCU40_SLICE0_PTR, \
        XMC_CCU4_SLICE_IRQ_ID_COMPARE_MATCH_UP);
    /*Enable Period match event for synchronizing the pattern update with the PWM */
    XMC_CCU8_SLICE_EnableEvent(CCU80_SLICE1_PTR, \
            XMC_CCU8_SLICE_IRQ_ID_PERIOD_MATCH);
    /* Connect compare match event to SR0 - MSETA*/
    XMC_CCU4_SLICE_SetInterruptNode(CCU40_SLICE0_PTR, \
        XMC_CCU4_SLICE_IRQ_ID_COMPARE_MATCH_UP, XMC_CCU4_SLICE_SR_ID_0);
    /* Connect period match event to SR1 - connect as input to MSYNC*/
    XMC_CCU8_SLICE_SetInterruptNode(CCU80_SLICE1_PTR, \
        XMC_CCU8_SLICE_IRQ_ID_PERIOD_MATCH, XMC_CCU8_SLICE_SR_ID_1);
    /* Set NVIC priority */
    NVIC_SetPriority(CCU40_0_IRQn, 3U);
    /* Enable IRQ */
    NVIC_EnableIRQ(CCU40_0_IRQn);
    /*Initializes the GPIO*/
    XMC_GPIO_Init(CCU40_SLICE0_OUTPUT, &CCU40_SLICE_OUTPUT_config);
    XMC_GPIO_Init(CCU80_SLICE0_OUTPUT00, &CCU80_SLICE_OUTPUT_config);
    XMC_GPIO_Init(CCU80_SLICE0_OUTPUT01, &CCU80_SLICE_OUTPUT_config);
    XMC_GPIO_Init(CCU80_SLICE0_OUTPUT02, &CCU80_SLICE_OUTPUT_config);
    XMC_GPIO_Init(CCU80_SLICE0_OUTPUT03, &CCU80_SLICE_OUTPUT_config);
    /* POSIF Configuration - Standalone mode */
    XMC_POSIF_Init(POSIF_PTR, &POSIF_config);
    XMC_POSIF_MCM_Init(POSIF_PTR, &POSIF_MCM_config);
    /* Start the POSIF module*/
    XMC_POSIF_Start(POSIF_PTR);

    /* Get the slice out of idle mode */
    XMC_CCU8_EnableClock(CCU80_MODULE_PTR, CCU80_SLICE0_NUMBER);
    XMC_CCU8_EnableClock(CCU80_MODULE_PTR, CCU80_SLICE1_NUMBER);
    XMC_CCU4_EnableClock(CCU40_MODULE_PTR, CCU40_SLICE0_NUMBER);
    /* Start the PWM on a rising edge on SCU.GSC40 and GSC80 */
    XMC_SCU_SetCcuTriggerHigh((uint32_t) \
        (XMC_SCU_CCU_TRIGGER_CCU40|XMC_SCU_CCU_TRIGGER_CCU80));
}

XMC_POSIF_HSC_CONFIG_t POSIF_HSC_config =
{
	.disable_idle_signal = 1,
	.sampling_trigger = 0, //HSDA
	.sampling_trigger_edge = 0 //Rising edge
};

void POSIF0_0_IRQHandler(void)
{
	uint8_t hallposition;

	/* Set the new Hall pattern */
	hallposition = XMC_POSIF_HSC_GetExpectedPattern(POSIF_PTR);
	XMC_POSIF_HSC_SetHallPatterns(POSIF_PTR, Sensor_Hall_GetPattern(hallposition));

	SensorHallCallback();
}

void POSIF_Init(void)
{

	/* POSIF Configuration */
	XMC_POSIF_Init(POSIF_PTR, &POSIF_HALL_config);
}
