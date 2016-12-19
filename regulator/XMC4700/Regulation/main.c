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
	double angle = 0; // crnt motor angle
	double temperature = 0; // crnt motor temperature
	double power = 0; // crnt motor power
	double passedMs; // milliseconds passed since last call
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

		// check if new pid values were received and if so, apply them
		COMHANDLER_UPDATE_PID_VALUES((&Regulation_VelocityVariables), (&Regulation_AngleVariables), (&Regulation_TemperatureVariables));

		// busy wait and assume it has been like 10 milliseconds
		for (i = 0UL; i < 720000; ++i) {}
		passedMs = 10;

		// read sensors
		Sensor_GetVelocity(&velocity);
		Sensor_GetAngle(&angle);
		Sensor_GetTemperature(&temperature);

		// regulate velocity
		power = REGULATION_REGULATE_SINGLE((&Regulation_VelocityVariables), passedMs/1000, velocity);

		// apply the motor power
		Motor_SetVelocityPower(power);

		// send new sensor values currently send the power instead of the angle
		ComHandler_SendSensorReadings(velocity, power, temperature);
	}
	return 0;
}
