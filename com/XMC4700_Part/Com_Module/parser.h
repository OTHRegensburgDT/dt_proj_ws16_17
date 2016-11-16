/*
 * parser.h
 *
 *  Created on: Oct 29, 2016
 *      Author: Michael
 */

#ifndef PARSER_H_
#define PARSER_H_

#include <Dave/Generated/CRC_SW/crc_sw.h>
#include <protobuf/pb_encode.h>
#include <protobuf/pb_decode.h>
#include "protobuf/SensorMsg.pb.h"
#include "com_structs.h"

#define PROTO_SENSORDATA Com_Module_SensorMsg
#define PROTO_SENSORDATA_FIELDS Com_Module_SensorMsg_fields
#define PROTO_DATAENTRY Com_Module_DataEntry
#define PROTO_DATAENTRY_FIELDS Com_Module_DataEntry_fields

#define VELOCITYID 1u
#define ANGLEID 2u
#define TEMP_1ID 3u
#define TEMP_2ID 4u
#define HALLID 5u

/*
 * bool ProtoToSensor(Sensordata* outData, uint8_t* protoMsg,int size)
 * Function:
 * parses Protobuf message to Sensordata struct
 * Params:
 * outData (out): pointer to deserialized SENSORDATA struct
 * protoMsg (in): pointer to input Buffer(will be free'd)
 * size (in): size of input Buffer
 * Return:
 * boolean value if operation was successful
 */
bool ProtoToSensor(Sensordata* outData, uint8_t* protoMsg,int size);

/*
 *bool SensorToProto(uint8_t* protoMsg, int* size, SENSORDATA* inData)
 *Function:
 *parses Sensordata struct to a protobuf message
 *Params:
 *protoMsg (out): pointer to protoMsg buffer
 *size (in/out): pointer to buffer size(will be set to new buffer size)
 *inData (in): pointer to SENSORDATA struct to be serialized(will be free'd)
 *Return:
 *boolean value if operation was successful
 */
bool SensorToProto(uint8_t* protoMsg, int* size, Sensordata* inData);

/*
 *bool FrameToProto(uint8_t* protoMsg, int* size, uint8_t* frame)
 *Function:
 *parses bytestream of protobuf message to transferable frame (protoBytestream + crc16)
 *Params:
 *buffer (in/out): pointer to frame buffer becomes pointer to protocol buffer
 *size (in/out): pointer to buffer size(will be set to protobuf size)
 *Return:
 *boolean value if operation was successful
 */
bool FrameToProto(uint8_t* buffer, int* size);

/*
 *ProtoToFrame(uint8_t* outFrame, int* size, uint8_t* protoMsg)
 *Function:
 *parses transferred frame (protoBytestream + crc16) to protobuf bytestream
 *Params:
 *buffer (in/out): pointer to protocol buffer becomes pointer to frame buffer
 *size (in/out): pointer to buffer size(will be set to frame size)
 *Return:
 *boolean value if operation was successful
 */
bool ProtoToFrame(uint8_t* buffer, int* size);

#endif /* PARSER_H_ */
