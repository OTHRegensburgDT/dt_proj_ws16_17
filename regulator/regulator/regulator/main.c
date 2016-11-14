#define HallCallback 

#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"

int main()
{
	// initialize
	ComHandler_Initialize();
	MotorHandler_Initialize();
	SensorHandler_Initialize();

	// main loop
	while(1)
	{
		// com

		// read sensor

		// regulate

	}

	return 0;
}