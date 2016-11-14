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
	int targetValue; // the desired target value
	float Kp; // degree in how much the p regulator affects the output.
	float Ki; // degree in how much the i regulator affects the output.
	float Kd; // degree in how much the d regulator affects the output.

	float regSum; // for I regulator.
	float lastDifferenceValue; // for d regulator.
};


#endif
