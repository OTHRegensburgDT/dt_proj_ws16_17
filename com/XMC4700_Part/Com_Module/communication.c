/*
 * communication.c
 *
 *  Created on: Oct 31, 2016
 *      Author: Michael
 */

#define OUT_BUFFSIZE 256

#include "communication.h"
#include "Sensorparser.h"
#include "Frameparser.h"
#include "Paramsparser.h"
#include <malloc.h>
#include <Dave/Generated/UART/uart.h>

/*--------global Variables -----------*/
float param_p;
float param_i;
float param_d;
float target_val;
reguTarget regulationTarget;
/*-------local Variables ------------*/
static uint8_t buffersize;
static uint8_t* inBuffer;
static bool newParams;

/*--------local functions ------------*/

bool restartRcv(void){
	bool retVal = false;
	UART_STATUS_t uStat;
	free(inBuffer);
	inBuffer = NULL;
	buffersize = 0;
	newParams = false;
	uStat = UART_AbortReceive(&UART_0);
	if(UART_STATUS_SUCCESS == uStat){
		uStat = UART_Receive(&UART_0, &buffersize, 1);
		if(UART_STATUS_SUCCESS == uStat){
			retVal = true;
		}
	}
	return retVal;
}

/*--------public functions -----------*/

bool getParamFlag(void){
	return newParams;
}

bool initCom(void){
	UART_STATUS_t uStat;
	newParams = false;
	param_p = 0.0;
	param_i = 0.0;
	param_d = 0.0;
	target_val = 0.0;
	buffersize = 0;
	UART_AbortReceive(&UART_0); //just in case
	//wait in background until framelength is known
	uStat = UART_Receive(&UART_0, &buffersize, 1);
	if(UART_STATUS_SUCCESS == uStat){
		return true;
	}else{
		return false;
	}
}

bool sendSensorData(Sensordata* data){
	bool retVal = false;
	uint8_t* outBuffer = malloc(OUT_BUFFSIZE);
	int bufsize = OUT_BUFFSIZE-1;
	UART_STATUS_t retStat;
	//parse to protobuf
	//first buffer-element is reserved for framesize
	retVal = SensorToProto(outBuffer, &bufsize, data);

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
	bool retVal = false;
	int inSize = buffersize;

	retVal = FrameToProto(inBuffer, &inSize);
	if(retVal){
		retVal = ProtoToParams(inBuffer+1, inSize);
	}

	restartRcv();

	return retVal;
}

void DataRcvICR(){
	UART_STATUS_t retStat;
	static uint8_t restctr = 0;
	if(NULL == inBuffer){
		//Frame size was received
		inBuffer = malloc(buffersize);
		//receive rest of frame
		inBuffer[0] = buffersize;
		restctr = buffersize -1;
		retStat = UART_Receive(&UART_0, inBuffer+1, 1);
		//check if receive will succeed

	}else{
		//receive rest
		restctr--;
		retStat = UART_Receive(&UART_0, inBuffer+(buffersize-restctr),1);
	}
	if(restctr == 0){
		newParams = true;
	}else{
		if(UART_STATUS_SUCCESS != retStat){
			free(inBuffer);
			inBuffer = NULL;
			buffersize = 0;
			initCom();
		}
	}
}
