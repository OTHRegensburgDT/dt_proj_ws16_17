/*
 * Motor.h
 *
 *  Created on: Dec 18, 2016
 *      Author: BerndNK
 */

#ifndef MOTOR_H_
#define MOTOR_H_

#include "Std_Types.h"

Std_ReturnType Motor_Initialize();

Std_ReturnType Motor_SetVelocityPower(double to);

#endif /* MOTOR_H_ */
