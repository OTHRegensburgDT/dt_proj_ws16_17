/*
 * parser.h
 *
 *  Created on: Oct 29, 2016
 *      Author: Michael
 */

#ifndef SENSORPARSER_H_
#define SENSORPARSER_H_

#include <stdint.h>
#include <stdbool.h>
#include "com_structs.h"

/*
 * bool ProtoToSensor(Sensordata* outData, uint8_t* protoMsg,int size)
 * Function:
 * parses Protobuf message to Sensordata struct
 * Params:
 * outData (out): pointer to deserialized SENSORDATA struct
 * protoMsg (in): pointer to input Buffer
 * size (in): size of input Buffer
 * Return:
 * boolean value if operation was successful
 */
extern bool ProtoToSensor(Sensordata* outData, uint8_t* protoMsg,int size);

/*
 *bool SensorToProto(uint8_t* protoMsg, int* size, SENSORDATA* inData)
 *Function:
 *parses Sensordata struct to a protobuf message
 *Params:
 *protoMsg (out): pointer to protoMsg buffer
 *size (in/out): pointer to buffer size(will be set to new buffer size)
 *inData (in): pointer to SENSORDATA struct to be serialized
 *Return:
 *boolean value if operation was successful
 */
extern bool SensorToProto(uint8_t* protoMsg, int* size, Sensordata* inData);



#endif /* SENSORPARSER_H_ */
