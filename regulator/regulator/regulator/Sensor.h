/*
* Sensor.h
*
*  Created on: Nov 8, 2016
*      Author: Andreas Lackner
*/

#ifndef SENSOR_H
#define SENSOR_H

#include "Std_Types.h"
#include "Sensor_Types.h"

/*
* <summary>
* Initializes the sensor interface.
* </summary>
*/
void Sensor_Init(void);

/*
* <summary>
* Starts the measurement of all sensor parameters.
* </summary>
*/
void Sensor_StartAll(void);

/*
* <summary>
*	Stops the measurement of all sensor parameters.
* </summary>
*/
void Sensor_StopAll(void);

/*
* <summary>
* Registers a callback function that is invoked if a correct hall event is detected.
* </summary>
* <param name="callback">Callback function pointer</param>
* <returns>E_OK if the registration was successful.</returns>
*/
Std_ReturnType Sensor_RegisterHallCallback(Sensor_HallCallbackType callback);

/*
* <summary>
* Gets the current velocity of the motor shaft.
* </summary>
* <param name="velocity">Pointer to memory location where the velocity is stored.</param>
* <returns>E_OK if the velocity was read successful.</returns>
*/
Std_ReturnType Sensor_GetVelocity(double* velocity);

/*
* <summary>
* Gets the current angle of the motor shaft.
* The value is measured in degrees.
* </summary>
* <param name="angle">Pointer to memory location where the angle is stored.</param>
* <returns>E_OK if the angle was read successful.</returns>
*/
Std_ReturnType Sensor_GetAngle(double* angle);

/*
* <summary>
* Gets the current temperature value for the given sensor.
* The value is measured in degree Celsius.
* </summary>
* <param name="sensor">Identifier of the sensor</param>
* <param name="temperature">Pointer to the memory location where the temperature is stored.</param>
* <returns>E_OK if the temperature was read successful.</returns>
*/
Std_ReturnType Sensor_GetTemperature(Sensor_TemperatureType sensor, uint16_t* temperature);

#endif /* SENSOR_H */
