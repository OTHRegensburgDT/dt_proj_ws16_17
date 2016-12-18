#define HallCallback
#define SENSORDATA Regulation_SensorMsg

#include <DAVE.h>                 //Declarations from DAVE Code Generation (includes SFR declaration)
#include <malloc.h>
#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"
#include "RegulationHandler.h"


int main()
{

	DAVE_STATUS_t status;
	status = DAVE_Init();           /* Initialization of DAVE APPs  */
	float speed = 0;
	float power = 0;
	float passedS;
	int i, j;
	Regulation_VelocityVariables.targetValue = 100.f;
	Regulation_VelocityVariables.Kp = 0.6;
	Regulation_VelocityVariables.Ki = 0.2;
	Regulation_VelocityVariables.Kd = 0.03;

	  if(status != DAVE_STATUS_SUCCESS)
	  {
	    /* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
	    XMC_DEBUG("DAVE APPs initialization failed\n");

	    while(1U)
	    {

	    }
	  }

	// initialize
	COMHANDLER_INITIALIZE();
	MOTORHANDLER_INITIALIZE();
	SENSORHANDLER_INITIALIZE();

	// main loop
    while(1U)
	{
		speed += power;

		for (i = 0; i < 80; i++)for (j = 0; j < 100000; j++);


		passedS = 2;

		power = REGULATION_REGULATE_SINGLE((&Regulation_VelocityVariables), passedS, speed);
		// com

		// read sensor

		// regulate
	}
    return 0;
}
