/*
 * communication.h
 *
 *  Created on: Oct 31, 2016
 *      Author: Michael
 */

#ifndef COMMUNICATION_H_
#define COMMUNICATION_H_

#include <stdbool.h>
#include "com_types.h"
/*
 * bool getParamFlag()
 * Function:
 * returns the flag if new parameters are available
 * Params:
 * none
 * Return:
 * boolean value indicating if new parameters have arrived
 */
bool getParamFlag(void);

/*
 * bool initCom()
 * Function:
 * initializes Com module by wiping all regulation params
 * Params:
 * none
 * Return:
 * boolean value indicating if the operation was successful (always true)
 */
bool initCom(void);

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
