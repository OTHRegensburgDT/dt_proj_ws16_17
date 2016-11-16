/*
 * Sensor.c
 *
 *  Created on: Nov 8, 2016
 *      Author: Andreas Lackner
 */

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include "Sensor.h"
#include "Sensor_Hall.h"

/*********************************************************************************
 * Global function definitions
 *********************************************************************************/

void Sensor_Init()
{
	Sensor_Hall_Init();
}

void Sensor_StartAll()
{
	Sensor_Hall_Start();
}

void Sensor_StopAll()
{
	Sensor_Hall_Stop();
}

void Sensor_SetDirection(MotorDirection_t direction)
{
	Sensor_Hall_SetDirection(direction);
}

Std_ReturnType Sensor_RegisterHallCallback(Sensor_HallCallbackType callback)
{
	SensorHallCallback = callback;
	return E_OK;
}

Std_ReturnType Sensor_GetVelocity(double* velocity)
{
	return E_OK;
}

Std_ReturnType Sensor_GetAngle(double* angle)
{
	return E_OK;
}

Std_ReturnType Sensor_GetTemperature(Sensor_TemperatureType sensor, uint16_t* temperature)
{
	return E_OK;
}
