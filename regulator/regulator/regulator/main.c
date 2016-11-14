#define HallCallback 

#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"
#include <stdio.h>



int main()
{
	// initialize
	COMHANDLER_INITIALIZE();
	MOTORHANDLER_INITIALIZE();
	SENSORHANDLER_INITIALIZE();
	int a = 5;
	int* b = &a;

	printf("%d", *b = 3);

	// main loop
	while(1)
	{
		// com

		// read sensor

		// regulate

	}

	return 0;
}
