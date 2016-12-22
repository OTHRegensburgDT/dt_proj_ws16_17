#ifndef _REGULATION_HANDLER_H_
#define _REGULATION_HANDLER_H_

// PID regulators
/*
 * <summary>Single line P regulation macro.</summary>
 */
#define REGULATION_P_REGULATE(crntValue, targetValue, Kp) Kp * (targetValue - crntValue)

 /*
  * <summary>Single line I regulation macro.</summary>
  */
#define REGULATION_I_REGULATE(crntValue, targetValue, regSumPtr, passedTime, Ki) Ki * (*regSumPtr = passedTime * (targetValue - crntValue))

  /*
   * <summary>Single line D regulation macro.</summary>
   * <note>For easier understanding written out:
   * float increase = ((TARGET - at) - lastDifference) / passedTime ;
   * lastDifference = TARGET-at;
   *
   * return Kd * increase;
   * </note>
   */
#define REGULATION_D_REGULATE(crntValue, targetValue, lastDifferencePtr, lastDifferenceValue, passedTime, Kd) (Kd * (((*lastDifferencePtr = targetValue-crntValue) - lastDifferenceValue) / passedTime))


struct Regulation_PidValues
{
	float targetValue; // the desired target value
	float Kp; // degree in how much the p regulator affects the output.
	float Ki; // degree in how much the i regulator affects the output.
	float Kd; // degree in how much the d regulator affects the output.

	float regSum; // for I regulator.
	float lastDifferenceValue; // for d regulator.
};


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

#define REGULATION_REGULATE_SINGLE(regulateVariablesPtr, passedTime, crntValue) REGULATION_P_REGULATE(crntValue, regulateVariablesPtr->targetValue, regulateVariablesPtr->Kp) + \
	REGULATION_I_REGULATE(crntValue, regulateVariablesPtr->targetValue, &(regulateVariablesPtr->regSum), passedTime, regulateVariablesPtr->Ki) + \
	REGULATION_D_REGULATE(crntValue, regulateVariablesPtr->targetValue, &(regulateVariablesPtr->lastDifferenceValue), regulateVariablesPtr->lastDifferenceValue, passedTime, regulateVariablesPtr->Kd);

/*
 * <summary>Regulates everything: velocity, angle and temperature</summary>
 */
#define REGULATION_REGULATE_ALL(passedTime, crntVelocity, crntAngle, crntTemperature) REGULATION_REGULATE_SINGLE((&Regulation_VelocityVariables), passedTime, crntVelocity);  \
	REGULATION_REGULATE_SINGLE((&Regulation_AngleVariables), passedTime, crntAngle); \
	REGULATION_REGULATE_SINGLE((&Regulation_TemperatureVariables), passedTime, crntTemperature) 

#endif
