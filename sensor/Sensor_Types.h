/*
 * Sensor_Types
 *
 *  Created on: Nov 9, 2016
 *      Author: Andreas Lackner
 */

#ifndef SENSOR_TYPES_H
#define SENSOR_TYPES_H

#define TEMPERATURE_SENSOR_A (0U)
#define TEMPERATURE_SENSOR_B (1U)

typedef uint8_t Sensor_TemperatureType;
typedef void(*Sensor_HallCallbackType)();

#endif /* SENSOR_TYPES_H */
