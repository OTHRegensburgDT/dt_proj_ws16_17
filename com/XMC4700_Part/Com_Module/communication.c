/*
 * communication.c
 *
 *  Created on: Oct 31, 2016
 *      Author: Michael
 */

#define BUFFERSIZE 256

#include "communication.h"
#include "parser.h"
#include "malloc.h"
#include <Dave/Generated/UART/uart.h>

bool sendSensorData(Sensordata* data){
	bool retVal = false;
	uint8_t* outBuffer = malloc(BUFFERSIZE);
	int bufsize = BUFFERSIZE -1;
	UART_STATUS_t retStat;
	//parse to protobuf
	//first buffer-element is reserved for framesize
	retVal = SensorToProto(&outBuffer[1], &bufsize, data);

	if(retVal){
		//parse to frame if successful
		retVal = ProtoToFrame(outBuffer, &bufsize);
		if(retVal && (bufsize < 256)){
			retStat = UART_Transmit(&UART_0, outBuffer, bufsize);
		}
	}

	if(retStat == UART_STATUS_SUCCESS){
		retVal = true;
	} else {
		retVal = false;
	}

	return retVal;
}

bool processRegulationData(){
	return true;
}

void DataRcvICR(){

}
