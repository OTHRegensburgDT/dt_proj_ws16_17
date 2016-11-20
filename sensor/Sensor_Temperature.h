/*
 * Sensor_Temperature.h
 *
 *  Created on: Nov 19, 2016
 *      Author: Andreas Lackner
 */

#ifndef SENSOR_TEMPERATURE_H
#define SENSOR_TEMPERATURE_H

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include "Std_Types.h"
#include "Sensor_Types.h"

/*********************************************************************************
 * Global data
 *********************************************************************************/


/*********************************************************************************
 * Global function declarations
 *********************************************************************************/

void Sensor_Temperature_Init(void);

int Sensor_Temperature_Calculate(Sensor_TemperatureType sensor);

#endif /* SENSOR_TEMPERATURE_H */
