#include "RegulationHandler.h"

/*
 * <summary>Variables for PID regulation on the velocity.</summary>
 */
static struct Regulation_PidValues Regulation_VelocityVariables = {
	.targetValue = 0,.Kp = 0.0,.Ki = 0.0,.Kd = 0.0
};

/*
* <summary>Variables for PID regulation on the motor angle.</summary>
*/
static struct Regulation_PidValues Regulation_AngleVariables = {
	.targetValue = 0,.Kp = 0.0,.Ki = 0.0,.Kd = 0.0
};

/*
* <summary>Variables for PID regulation on the temperature.</summary>
*/
static struct Regulation_PidValues Regulation_TemperatureVariables = {
	.targetValue = 0,.Kp = 0.0,.Ki = 0.0,.Kd = 0.0
};

