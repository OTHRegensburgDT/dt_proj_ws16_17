/*
 * Frameparser.c
 *
 *  Created on: Nov 19, 2016
 *      Author: Michael
 */

#include "Frameparser.h"
#include <malloc.h>
#include <Dave/Generated/CRC_SW/crc_sw.h>

bool FrameToProto(uint8_t* buffer, int* size){
	bool status = false;
	uint32_t CRCResult;
	CRC_SW_CalculateCRC(&CRC_SW_0, buffer, *size);
	CRCResult = CRC_SW_GetCRCResult(&CRC_SW_0);

	//check if Message was transferred correctly overall-crc == 0
	if(CRCResult == 0){
		//cut buffer
		buffer = realloc(buffer, (*size)-2);
		//attention: protobuf is size -3 as first element is framesize and has to be spared
		*size = (*size) -3;
		status = true;
	}

	return status;
}

bool ProtoToFrame(uint8_t* buffer, int* size){
	bool status = false;
	uint16_t CRCResult;
	//calculate crc to protobuf message
	CRC_SW_CalculateCRC(&CRC_SW_0, buffer+1, *size);
	CRCResult = CRC_SW_GetCRCResult(&CRC_SW_0);
	//increase buffer
	buffer = realloc(buffer, (*size)+3);
	*size = (*size)+3;

	//insert framelength at beginning
	buffer[0] = *size;
	//add calculcated crc
	buffer[*size-2] = (uint16_t)(CRCResult >> 8);
	buffer[*size-1] = (uint16_t)CRCResult;

	status = true;
	return status;
}
