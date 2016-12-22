/*
 * Sensor_Types
 *
 *  Created on: Nov 9, 2016
 *      Author: Andreas Lackner
 */

#ifndef SENSOR_TYPES_H
#define SENSOR_TYPES_H

#define TEMPERATURE_SENSOR_A (0U)

typedef struct Sensor_HallPattern
{
	uint8_t h1;
	uint8_t h2;
	uint8_t h3;
} Sensor_HallPattern_t;

typedef uint8_t Sensor_TemperatureType;
typedef void(*Sensor_HallCallbackType)();

#endif /* SENSOR_TYPES_H */
