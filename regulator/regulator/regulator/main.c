#define HallCallback 

#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"
#include "RegulationHandler.h"
#include <stdio.h>


int main()
{
	// initialize
	COMHANDLER_INITIALIZE();
	MOTORHANDLER_INITIALIZE();
	SENSORHANDLER_INITIALIZE();

	// main loop
	while(1)
	{
		// wait a bit

		// com

		// read sensor

		// regulate
	}
}
