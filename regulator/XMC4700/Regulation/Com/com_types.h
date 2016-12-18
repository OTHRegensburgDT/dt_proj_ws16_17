/*
 * com_types.h
 *
 *  Created on: Nov 4, 2016
 *      Author: Michael
 */

#ifndef COM_TYPES_H_
#define COM_TYPES_H_

#define SENSORDATA_COUNT 4
typedef struct Sensordata{
	double velocity;
	double angle;
	double temperature0;
	double hallpattern;
} Sensordata;

typedef enum reguTarget{
	ANGLE,
	TEMPERATURE,
	VELOCITY
}reguTarget;

#endif /* COM_TYPES_H_ */
