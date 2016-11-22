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
bool ProtoToParam(uint8_t* protoMsg,int size){
	bool status = false;
	PROTO_REGPARAMS message;

	pb_istream_t stream = pb_istream_from_buffer(protoMsg, size);

	status = pb_decode(&stream, PROTO_REGPARAMS_FIELDS, &message);
	if(true == status){
		//a fixed angle is not possible with velocity != 0
		if(message.has_paraAng && !(message.has_paraVelo)){
			angle_aim = message.paraAng;
		}
		//velocity != 0 is not possible with a fixed angle
		if(message.has_paraVelo && !(message.has_paraAng)){
			velo_aim = message.paraVelo;
		}
		if(message.has_paraP){
			param_p = message.paraP;
		}
		if(message.has_paraI){
			param_i = message.paraI;
		}
		if(message.has_paraD){
			param_d = message.paraD;
		}
	}
	return status;
}


bool ParamToProto(uint8_t* protoMsg, int* size){
	bool status = false;
	PROTO_REGPARAMS message;

	//add paramter p
	if(param_p > 0.0001 || param_p < -0.0001){
		message.has_paraP = true;
		message.paraP = param_p;
	}
	//add parameter i
	if(param_i > 0.0001 || param_i < -0.0001){
		message.has_paraI = true;
		message.paraI = param_i;
	}
	//add parameter d
	if(param_d > 0.0001 || param_d < -0.0001){
		message.has_paraD = true;
		message.paraD = param_d;
	}
	//add target velocity
	if(velo_aim > 0.0001 || velo_aim < -0.0001){
		message.has_paraVelo = true;
		message.paraVelo = velo_aim;
	}
	//add target angle
	if(angle_aim != 0){
		message.has_paraAng = true;
		message.paraAng = angle_aim;
	}

	//serialize protobuf message
	if((*size) >= 128){
		pb_ostream_t stream = pb_ostream_from_buffer(protoMsg, *size);
		status = pb_encode(&stream, PROTO_REGPARAMS_FIELDS, &message);
		*size = stream.bytes_written;
		protoMsg = realloc(protoMsg, *size);
	}

	return status;
}
