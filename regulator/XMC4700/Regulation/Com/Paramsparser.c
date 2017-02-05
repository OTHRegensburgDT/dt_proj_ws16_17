/*
 * Paramsparser.c
 *
 *  Created on: Nov 19, 2016
 *      Author: Michael
 */

#include <protobuf/pb_encode.h>
#include <protobuf/pb_decode.h>
#include "Paramsparser.h"
#include "protobuf/ParamMsg.pb.h"
#include <malloc.h>

#define PROTO_REGPARAMS Com_Module_RegParams
#define PROTO_REGPARAMS_FIELDS Com_Module_RegParams_fields

/*----------- global functions ----------*/
bool ProtoToParams(uint8_t* protoMsg,int size){
	bool status = false;
	PROTO_REGPARAMS message;

	pb_istream_t stream = pb_istream_from_buffer(protoMsg, size);

	status = pb_decode(&stream, PROTO_REGPARAMS_FIELDS, &message);
	if(true == status){
		//a fixed angle is not possible with velocity != 0
		switch(message.target){
		case ANGLE:
			regulationTarget = ANGLE;
			break;
		case VELOCITY:
			regulationTarget = VELOCITY;
			break;
		case TEMPERATURE:
			regulationTarget = TEMPERATURE;
			break;
		default:
			status = false;
			break;
		}

		target_val = message.tgtVal;

		param_p = message.paraP;

		param_i = message.paraI;

		param_d = message.paraD;
	}
	return status;
}


bool ParamToProto(uint8_t* protoMsg, int* size){
	bool status = false;
	PROTO_REGPARAMS message;

	//add paramter p
	message.paraP = param_p;

	//add parameter i
	message.paraI = param_i;

	//add parameter d
	message.paraD = param_d;

	//add target value
	switch(regulationTarget){
	case ANGLE:
		message.target = ANGLE;
		message.tgtVal = target_val;
		status = true;
		break;
	case TEMPERATURE:
		message.target = TEMPERATURE;
		message.tgtVal = target_val;
		status = true;
		break;
	case VELOCITY:
		message.target = VELOCITY;
		message.tgtVal = target_val;
		status = true;
		break;
	default:
		status = false;
	}

	//serialize protobuf message
	if(status && ((*size) >= 128)){
		pb_ostream_t stream = pb_ostream_from_buffer(protoMsg, *size);
		status = pb_encode(&stream, PROTO_REGPARAMS_FIELDS, &message);
		*size = stream.bytes_written;
		protoMsg = realloc(protoMsg, (*size)+1);
	}

	return status;
}
