/*
 * Sensor_Hall.h
 *
 *  Created on: Nov 9, 2016
 *      Author: Andreas Lackner
 */

#ifndef SENSOR_HALL_H
#define SENSOR_HALL_H

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include "Std_Types.h"
#include "Sensor_Types.h"

/*********************************************************************************
 * Global data
 *********************************************************************************/

Sensor_HallCallbackType SensorHallCallback;
Sensor_HallPattern_t ActiveHallPattern;

/*********************************************************************************
 * Global function declarations
 *********************************************************************************/

void Sensor_Hall_Init(void);

void Sensor_Hall_Start(void);

void Sensor_Hall_Stop(void);

void Sensor_Hall_SetDirection(MotorDirection_t direction);

#endif /* SENSOR_HALL_H */
