/*
 * Std_Types.h
 *
 *  Created on: Nov 9, 2016
 *      Author: Andreas Lackner
 */

#ifndef STD_TYPES_H
#define STD_TYPES_H

#include "stdint.h"

typedef uint8_t Std_ReturnType;

typedef enum MotorDirection
{
	ClockWise = 0,
	CounterClockWise
} MotorDirection_t;

#define E_NOT_OK 0
#define E_OK 1

#endif /* STD_TYPES_H */
