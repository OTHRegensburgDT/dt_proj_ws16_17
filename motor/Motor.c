/*
 * Motor_Init.c
 *
 *  Created on: Dec 19, 2016
 *      Author: Andreas Kölbl
 */

#include "../sensor/Sensor.h"
#include "Motor_Init.h"
#include <xmc_gpio.h>
#include <xmc_posif.h>

#define HALL_CCU		CCU80
#define HALL_CCU_NUM	(0U)

/* PWM only on High */
#define	  MOTOR_AH P1_10 /* U */
#define	  MOTOR_AL P1_11
#define	  MOTOR_BH P3_10 /* V */
#define	  MOTOR_BL P3_8
#define	  MOTOR_CH P3_7  /* W */
#define	  MOTOR_CL P3_9

#define MOTOR_AH_PWM            CCU80_CC80
#define MOTOR_AH_PWM            CCU80_CC80
#define MOTOR_AH_PWM            CCU80_CC80
#define DELAY_SLICE_NUMBER 		(0U)
#define CAPTURE_SLICE_PTR 		CCU80_CC81
#define CAPTURE_SLICE_NUMBER	(1U)

#define HALL_EVENT_IO P1_10

#define GEN_HALL_PATTERN(EHP, CHP) (((uint32_t)EHP << 3) | (uint32_t)CHP)
#define HALL_EMPTY 0



static uint32_t state; 

void Motor_Init()
{
    state = 0;
    XMC_GPIO_SetOutputLow(MOTOR_AH);
    XMC_GPIO_SetOutputLow(MOTOR_AL);
    XMC_GPIO_SetOutputLow(MOTOR_BH);
    XMC_GPIO_SetOutputLow(MOTOR_BL);
    XMC_GPIO_SetOutputLow(MOTOR_CH);
    XMC_GPIO_SetOutputLow(MOTOR_CL);

    Sensor_RegisterHallCallback(Motor_Main);
}

void Motor_Main()
{
    Sensor_HallPattern_t pattern;
    if (Sensor_GetCurrentHallPattern(&pattern) != E_OK)
    {
        switch (pattern.h1 | pattern.h2 << 1 | pattern.h3 << 2)
        {
            case 5:
                 XMC_GPIO_SetOutputLow(MOTOR_CL);
                 XMC_GPIO_SetOutputHigh(MOTOR_AH);
                 XMC_GPIO_SetOutputHigh(MOTOR_BL);
                 break;
            case 1:
                XMC_GPIO_SetOutputLow(MOTOR_AH);
                XMC_GPIO_SetOutputHigh(MOTOR_CH);
                break;
            case 3:
                XMC_GPIO_SetOutputLow(MOTOR_BL);
                XMC_GPIO_SetOutputHigh(MOTOR_AL);
                break;
            case 2:
                XMC_GPIO_SetOutputLow(MOTOR_CH);
                XMC_GPIO_SetOutputHigh(MOTOR_BH);
                break;
            case 6:
                XMC_GPIO_SetOutputLow(MOTOR_AL);
                XMC_GPIO_SetOutputHigh(MOTOR_CL);
                break;
            case 4:
                XMC_GPIO_SetOutputLow(MOTOR_BH);
                XMC_GPIO_SetOutputHigh(MOTOR_AH);
                break;
            default:
                break;
        }
    }
}


