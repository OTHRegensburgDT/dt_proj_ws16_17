/*
 * com_structs.h
 *
 *  Created on: Nov 4, 2016
 *      Author: Michael
 */

#ifndef COM_STRUCTS_H_
#define COM_STRUCTS_H_

#define SENSORDATA_COUNT 5
typedef struct Sensordata{
	double velocity;
	double angle;
	double temperature0;
	double temperature1;
	double hallpattern;
} Sensordata;

#endif /* COM_STRUCTS_H_ */
