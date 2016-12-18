#define HallCallback
#define SENSORDATA Regulation_SensorMsg

#include <DAVE.h>                 //Declarations from DAVE Code Generation (includes SFR declaration)
#include <malloc.h>
#include "Std_Types.h"
#include "comHandler.h"
#include "sensorHandler.h"
#include "motorHandler.h"
#include "RegulationHandler.h"

int main() {

	DAVE_STATUS_t status;
	status = DAVE_Init(); /* Initialization of DAVE APPs  */
	double velocity = 0; // crnt motor velocity
	double power = 0; // crnt motor power
	float passedMs; // milliseconds passed since last call
	int i, j; // iterator for busy waiting

	// initial PID values / target value
	Regulation_VelocityVariables.targetValue = 100.f;
	Regulation_VelocityVariables.Kp = 0.6;
	Regulation_VelocityVariables.Ki = 0.2;
	Regulation_VelocityVariables.Kd = 0.03;

	if (status != DAVE_STATUS_SUCCESS) {
		/* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
		XMC_DEBUG("DAVE APPs initialization failed\n");

		while (1U) {

		}
	}

	// initialize
	COMHANDLER_INITIALIZE();
	MOTORHANDLER_INITIALIZE();
	SENSORHANDLER_INITIALIZE();

	// main loop
	while (1U) {

		// busy wait and assume it has been like 10 milliseconds
		for (i = 0UL; i < 720000; ++i) {
			//asm("nop");
		}
		passedMs = 10;

		// com
		COMHANDLER_RECEIVE();
		COMHANDLER_UPDATE_PID_VALUES();

		// read sensor
		Sensor_GetVelocity(&velocity);

		// regulate velocity
		power = REGULATION_REGULATE_SINGLE((&Regulation_VelocityVariables),
				passedMs, velocity)
		;
		Motor_SetVelocityPower(power);

		// send new values
		ComHandler_SendSensorReadings(velocity);
	}
	return 0;
}
