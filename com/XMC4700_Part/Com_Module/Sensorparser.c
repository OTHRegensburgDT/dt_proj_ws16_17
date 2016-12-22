/*
 * parser.c
 *
 *  Created on: Oct 29, 2016
 *      Author: Michael
 */
#include <math.h>
#include <malloc.h>
#include <protobuf/pb_encode.h>
#include <protobuf/pb_decode.h>
#include "Sensorparser.h"
#include "protobuf/SensorMsg.pb.h"
#include "SensorIDs.h"

#define PROTO_SENSORDATA Com_Module_SensorMsg
#define PROTO_SENSORDATA_FIELDS Com_Module_SensorMsg_fields
#define PROTO_DATAENTRY Com_Module_DataEntry
#define PROTO_DATAENTRY_FIELDS Com_Module_DataEntry_fields

/*----------- local functions -----------*/

/*
 * bool encodeData_cb
 * This function encodes the DataEntries of the protobuf Message
 */
bool encodeData_cb(pb_ostream_t *stream, const pb_field_t *field, void * const *arg)
{
	int i;
	bool retval = true;
	Sensordata* data = (Sensordata*)*arg;
	Com_Module_DataEntry entries[SENSORDATA_COUNT];

	/*velocity*/
	entries[0].SensorId = VELOCITYID;
	entries[0].Data = data->velocity;
	/*angle*/
	entries[1].SensorId = ANGLEID;
	entries[1].Data = data->angle;
	/*temp sensor*/
	entries[2].SensorId = TEMP_ID;
	entries[2].Data = data->temperature0;
	/*hall sensor pattern*/
	entries[3].SensorId = HALLID;
	entries[3].Data = data->hallpattern;

	for(i = 0; i < SENSORDATA_COUNT && retval; i++){
		if(pb_encode_tag_for_field(stream, field)){
			retval = pb_encode_submessage(stream, PROTO_DATAENTRY_FIELDS, &(entries[i]));
		}
	}
	return retval;
}

/*
 * bool decodeData_cb
 * This function encodes the DataEntries of the protobuf Message
 */
bool decodeData_cb(pb_istream_t *stream, const pb_field_t *field, void **arg)
{

	bool retval = false;
	Sensordata** data = (Sensordata**)arg;
	Com_Module_DataEntry entry = {};

	retval = pb_decode(stream, PROTO_DATAENTRY_FIELDS, &entry);

	if(retval == true){
		switch(entry.SensorId){
		case TEMP_ID:
			(*data)->temperature0 = entry.Data;
			break;
		case HALLID:
			(*data)->hallpattern = entry.Data;
			break;
		case VELOCITYID:
			(*data)->velocity = entry.Data;
			break;
		case ANGLEID:
			(*data)->angle = entry.Data;
			break;
		default:
			break;
		}
	}
	return retval;
}
/*----------- global functions ----------*/
bool ProtoToSensor(Sensordata* outData, uint8_t* protoMsg,int size){
	bool status = false;
	PROTO_SENSORDATA protoData;

	pb_istream_t stream = pb_istream_from_buffer(protoMsg, size);

	protoData.DataTable.funcs.decode = &decodeData_cb;
	protoData.DataTable.arg = outData;

	status = pb_decode(&stream, PROTO_SENSORDATA_FIELDS, &protoData);

	return status;
}


bool SensorToProto(uint8_t* protoMsg, int* size, Sensordata* inData){
	bool status = false;
	static uint64_t seqnr = 0;
	PROTO_SENSORDATA protoData;

	//assemble protobuf message
	protoData.SequenceNr = seqnr;
	protoData.DataTable.funcs.encode = &encodeData_cb;
	protoData.DataTable.arg = inData;

	//serialize protobuf message
	if((*size) >= 128){
		pb_ostream_t stream = pb_ostream_from_buffer(protoMsg+1, *size);
		status = pb_encode(&stream, PROTO_SENSORDATA_FIELDS, &protoData);
		*size = stream.bytes_written;
		protoMsg = realloc(protoMsg, (*size)+1);
	}

	seqnr++;
	return status;
}


