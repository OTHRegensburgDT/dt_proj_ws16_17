/*
 * Frameparser.h
 *
 *  Created on: Nov 19, 2016
 *      Author: Michael
 */

#ifndef FRAMEPARSER_H_
#define FRAMEPARSER_H_

#include <stdint.h>
#include <stdbool.h>

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
extern bool FrameToProto(uint8_t* buffer, int* size);

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
extern bool ProtoToFrame(uint8_t* buffer, int* size);


#endif /* FRAMEPARSER_H_ */
