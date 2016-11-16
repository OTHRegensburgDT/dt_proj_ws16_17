/*
 * Sensor.h
 *
 *  Created on: Nov 8, 2016
 *      Author: Andreas Lackner
 */

#ifndef SENSOR_H
#define SENSOR_H

/*********************************************************************************
 * Includes
 *********************************************************************************/

#include "Std_Types.h"
#include "Sensor_Types.h"

/*********************************************************************************
 * Global function declarations
 *********************************************************************************/

/*
 * <summary>
 * Initializes the sensor interface.
 * </summary>
 */
void Sensor_Init();

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
 *	Sets the direction that the motor is turning.
 * </summary>
 */
void Sensor_SetDirection(MotorDirection_t direction);

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
 * Gets the current velocity of the motor shaft measured in rotations per second.
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
 * <returns>E_OK if the angle was read sucessfull.</returns>
 */
Std_ReturnType Sensor_GetAngle(double* angle);

/*
 * <summary>
 * Gets the current temperature value for the given sensor.
 * The value is measured in degree celcius.
 * </summary>
 * <param name="sensor">Identifier of the sensor</param>
 * <param name="temperature">Pointer to the memory location where the temperature is stored.</param>
 * <returns>E_OK if the temperature was read successful.</returns>
 */
Std_ReturnType Sensor_GetTemperature(Sensor_TemperatureType sensor, uint16_t* temperature);

#endif /* SENSOR_H */
