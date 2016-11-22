/*
 * Paramsparser.h
 *
 *  Created on: Nov 19, 2016
 *      Author: Michael
 */

#ifndef PARAMSPARSER_H_
#define PARAMSPARSER_H_

#include <stdint.h>
#include <stdbool.h>
/*------------external variables ----------------*/
extern bool newParams;
extern double param_p;
extern double param_i;
extern double param_d;
extern double velo_aim;
extern short angle_aim;

/*-------------------global functions -------------*/
/*
 * bool ProtoToParams(uint8_t* protoMsg,int size)
 * Function:
 * parses Protobuf message to regulation parameters and sets them
 * Params:
 * protoMsg (in): pointer to beginning of proto message in input Buffer
 * size (in): size of input Buffer
 * Return:
 * boolean value if operation was successful
 */
extern bool ProtoToParams(uint8_t* protoMsg,int size);

/*
 *bool ParamsToProto(uint8_t* protoMsg, int* size, SENSORDATA* inData)
 *Function:
 *gets regulation parameters to a protobuf message
 *Params:
 *protoMsg (out): pointer to 2nd element of protoMsg buffer (first byte of buffer must be left free for framelength)
 *size (in/out): pointer to buffer size(will be set to new buffer size)
 *Return:
 *boolean value if operation was successful
 */
extern bool ParamsToProto(uint8_t* protoMsg, int* size);


#endif /* PARAMSPARSER_H_ */
