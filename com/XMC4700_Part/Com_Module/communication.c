/*
 * communication.c
 *
 *  Created on: Oct 31, 2016
 *      Author: Michael
 */

#define BUFFERSIZE 256

#include "communication.h"
#include "Sensorparser.h"
#include "Frameparser.h"
#include "Paramsparser.h"
#include <malloc.h>
#include <Dave/Generated/UART/uart.h>

/*--------global Variables -----------*/
bool newParams;
double param_p;
double param_i;
double param_d;
double velo_aim;
short angle_aim;

/*--------public functions -----------*/

bool initCom(void){
	newParams = false;
	param_p = 0.0;
	param_i = 0.0;
	param_d = 0.0;
	velo_aim = 0.0;
	angle_aim = 0;
	return true;
}

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
	free(outBuffer);
	return retVal;
}

bool processRegulationData(){
	UART_STATUS_t retStat;
	bool retVal = false;
	int inSize;
	uint8_t* inBuffer = malloc(BUFFERSIZE);
	//read framelength
	retStat = UART_Receive(&UART_0, inBuffer, 1);
	if(UART_STATUS_SUCCESS == retStat){
		inSize = inBuffer[0];
		//decrease framelength as first field was already read
		inSize--;
		//read rest of frame
		retStat = UART_Receive(&UART_0, &(inBuffer[1]), inSize);
		if(UART_STATUS_SUCCESS == retStat){
			retVal = FrameToProto(&(inBuffer[1]), &inSize);
			if(retVal){
				retVal = ProtoToParam(&(inBuffer[1]), inSize);
			}
		}
	}

	free(inBuffer);
	return retVal;
}

void DataRcvICR(){
	newParams = true;
}
