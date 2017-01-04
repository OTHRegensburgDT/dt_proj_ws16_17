/*
 * Motor_Init.c
 *
 *  Created on: Dec 19, 2016
 *      Author: Andreas Kölbl
 */

#include "Sensor.h"
#include "Motor.h"
#include "Sensor_Hall.h" //direction
#include "Motor_PWMSchemes.h"
#include <xmc_gpio.h>
#include <xmc_posif.h>

#define HALL_CCU		CCU80
#define HALL_CCU_NUM	(0U)

#define MOTOR_AH_PWM            CCU80_CC80
#define MOTOR_AH_PWM            CCU80_CC80
#define MOTOR_AH_PWM            CCU80_CC80
#define DELAY_SLICE_NUMBER 		(0U)
#define CAPTURE_SLICE_PTR 		CCU80_CC81
#define CAPTURE_SLICE_NUMBER	(1U)

#define HALL_EVENT_IO P1_10

#define GEN_HALL_PATTERN(EHP, CHP) (((uint32_t)EHP << 3) | (uint32_t)CHP)
#define HALL_EMPTY 0

extern MotorDirection_t motorDirection;

XMC_GPIO_CONFIG_t MOTOR_POSIF_0_PadConfig =
{
	.mode = (XMC_GPIO_MODE_t)XMC_GPIO_MODE_OUTPUT_PUSH_PULL,
	.output_level = (XMC_GPIO_OUTPUT_LEVEL_t)XMC_GPIO_OUTPUT_LEVEL_LOW,
};

void Motor_ClearOutputs()
{
    XMC_GPIO_SetOutputLow(MOTOR_AH);
    XMC_GPIO_SetOutputLow(MOTOR_AL);
    XMC_GPIO_SetOutputLow(MOTOR_BH);
    XMC_GPIO_SetOutputLow(MOTOR_BL);
    XMC_GPIO_SetOutputLow(MOTOR_CH);
    XMC_GPIO_SetOutputLow(MOTOR_CL);
}

void Motor_Main()
{//451326
    Sensor_HallPattern_t pattern;
    uint8_t position;
    uint8_t counterClockWisePattern[] = {1, 3, 2, 6, 4, 5};
    uint8_t clockWisePattern[] = {5, 4, 6, 2, 3, 1};
    uint8_t* currentPattern = clockWisePattern;
    if (Sensor_GetDirection() == CounterClockWise)
    {
    	currentPattern = counterClockWisePattern;
    }
    if (Sensor_GetCurrentHallPattern(&pattern) == E_OK)
    {
        /* http://multicopter.org/wiki/PWM_Schemes */
        uint8_t position = pattern.h1 | pattern.h2 << 1 | pattern.h3 << 2;
        Motor_Scheme_Default(currentPattern, position);
    }
}

void Motor_Init()
{
	XMC_GPIO_Init(MOTOR_AH, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(MOTOR_AL, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(MOTOR_BH, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(MOTOR_BL, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(MOTOR_CH, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(MOTOR_CL, &MOTOR_POSIF_0_PadConfig);
	XMC_GPIO_Init(P1_9, &MOTOR_POSIF_0_PadConfig);
	Motor_ClearOutputs();
    Sensor_RegisterHallCallback(&Motor_Main);
}
