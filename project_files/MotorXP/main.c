/*
 * main.c
 *
 *  Created on: 2016 Dec 20 09:50:50
 *  Author: MotorXP
 */
/**

 * @brief main() - Application entry point
 *
 * <b>Details of function</b><br>
 * This routine is the application entry point. It is invoked by the device startup code. 
 */
#include "Std_Types.h"
#include "Motor.h"
#include "Sensor.h"


int main(void)
{
  MotorDirection_t direction = ClockWise;
  Sensor_SetDirection(direction);
  int temp;

  Sensor_Init();
  Motor_Init();

  Sensor_StartAll();
  /* Start the motor */
  Motor_Main();

  while(1U)
  {
	  Sensor_GetTemperature(&temp);
  }
}
