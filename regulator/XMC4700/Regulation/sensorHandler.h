#ifndef _SENSOR_HANDLER_H_
#define _SENSOR_HANDLER_H_

#include "Sensor/Sensor.h"

/*
* <summary>
* Initializes the sensor interface.
* </summary>
*/
#define SENSORHANDLER_INITIALIZE() Sensor_Init(); Sensor_StartAll()

#endif
