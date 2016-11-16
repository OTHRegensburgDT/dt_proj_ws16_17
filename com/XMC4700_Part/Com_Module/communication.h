/*
 * communication.h
 *
 *  Created on: Oct 31, 2016
 *      Author: Michael
 */

#ifndef COMMUNICATION_H_
#define COMMUNICATION_H_

#include "protobuf/SensorMsg.pb.h"
#include "com_structs.h"

/*
 * bool sendSensorData(Sensordata* data)
 * Function:
 * sends the Sensordata struct as protobuf message
 * Params:
 * data (in): pointer to SENSORDATA struct to be sent
 * Return:
 * boolean value indicating if the operation was successful
 */
bool sendSensorData(Sensordata* data);

/*
 * bool processRegulationData()
 * Function:
 * stores the received regulation parameters into variables
 * Params:
 * (in): pointer to Params struct received
 * Return:
 * boolean value indicating if the operation was successful
 */
bool processRegulationData();

/*
 * void DataRcvICR()
 * Function:
 * ISR when DMA has received Data
 * Params:
 * no
 * Retrun:
 * void
 */
void DataRcvICR();

#endif /* COMMUNICATION_H_ */
