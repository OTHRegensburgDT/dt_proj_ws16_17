/*
 * Motor_PWMSchemes.c
 *
 *  Created on: Jan 4, 2017
 *      Author: Andreas Kölbl
 */

#include "Motor_PWMSchemes.h"
#include "Motor.h"

void Motor_Scheme_Default(uint8_t* currentPattern, uint8_t position)
{
	if (position == currentPattern[0])
	{
		 Motor_ClearOutputs();
		 XMC_GPIO_SetOutputHigh(MOTOR_AH);
		 XMC_GPIO_SetOutputHigh(MOTOR_BL);
	}
	else if (position == currentPattern[1])
	{
		Motor_ClearOutputs();
		XMC_GPIO_SetOutputLow(MOTOR_AH);
		XMC_GPIO_SetOutputHigh(MOTOR_CH);
	}
	else if (position == currentPattern[2])
	{
		Motor_ClearOutputs();
		XMC_GPIO_SetOutputLow(MOTOR_BL);
		XMC_GPIO_SetOutputHigh(MOTOR_AL);
	}
	else if (position == currentPattern[3])
	{
		Motor_ClearOutputs();
		XMC_GPIO_SetOutputLow(MOTOR_CH);
		XMC_GPIO_SetOutputHigh(MOTOR_BH);
	}
	else if (position == currentPattern[4])
	{
		Motor_ClearOutputs();
		XMC_GPIO_SetOutputLow(MOTOR_AL);
		XMC_GPIO_SetOutputHigh(MOTOR_CL);
	}
	else if (position == currentPattern[5])
	{
		Motor_ClearOutputs();
		XMC_GPIO_SetOutputLow(MOTOR_BH);
		XMC_GPIO_SetOutputHigh(MOTOR_AH);
	}
	else
	{
		//TODO error handling
	}
}
