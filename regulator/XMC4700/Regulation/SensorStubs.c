#include "Sensor/Sensor.h"
#include <stdio.h>

double stubVelocity = 0.0;

void Sensor_Init(void)
{
	printf("sensor_init\n");
}

void Sensor_StartAll(void)
{
	printf("sensor_startAll\n");
}

void Sensor_StopAll()
{

}

void Sensor_SetDirection(MotorDirection_t direction)
{

}

Std_ReturnType Sensor_RegisterHallCallback(Sensor_HallCallbackType callback)
{

	return E_OK;
}

Std_ReturnType Sensor_GetCurrentHallPattern(Sensor_HallPattern_t* pattern)
{
	return E_OK;
}

Std_ReturnType Sensor_GetVelocity(double* velocity)
{
	*velocity = stubVelocity;
	return E_OK;
}

Std_ReturnType Sensor_GetAngle(double* angle)
{
	return E_OK;
}

Std_ReturnType Sensor_GetTemperature(int* temperature)
{
	*temperature = 5; // 5 LUL
	return E_OK;
}
