#define HallCallback 

#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"
#include "RegulationHandler.h"
#include <stdio.h>
#include <sys\timeb.h> 


int main()
{

	float speed = 0;
	float power = 0;
	struct timeb start, end;
	float passedS;
	int i, j;
	Regulation_VelocityVariables.targetValue = 100.f;
	Regulation_VelocityVariables.Kp = 0.6;
	Regulation_VelocityVariables.Ki = 0.2;
	Regulation_VelocityVariables.Kd = 0.03;

	// initialize
	COMHANDLER_INITIALIZE();
	MOTORHANDLER_INITIALIZE();
	SENSORHANDLER_INITIALIZE();
	// main loop
	while (1)
	{
		ftime(&start);
		speed += power;

		for (i = 0; i < 80; i++)for (j = 0; j < 100000; j++);

		ftime(&end);
		passedS = (1000.0 * (end.time - start.time)
			+ (end.millitm - start.millitm)) / 1000.0;

		power = REGULATION_REGULATE_SINGLE((&Regulation_VelocityVariables), passedS, speed);

		printf("%f\n", speed);
		// com

		// read sensor

		// regulate
	}
}
