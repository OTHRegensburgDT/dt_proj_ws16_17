/*
 * uintTest.c
 *
 *  Created on: Nov 29, 2016
 *      Author: Michael
 */

#include <malloc.h>
#include "unitTest.h"
#include "Paramsparser.h"
#include "Frameparser.h"
#include "Sensorparser.h"
#include "communication.h"

bool comTest_sensorParsing(){
	Sensordata test_in, test_out;
	bool retVal;
	uint8_t* buf = malloc(45);
	int size = 45;
	test_in.angle = 45.0;
	test_in.hallpattern = 3;
	test_in.temperature0 = 21.0;
	test_in.velocity = 0.0;

	retVal = SensorToProto(buf, &size, &test_in);
	if(retVal){
		retVal = ProtoToSensor(&test_out, buf, size);
	}
	return retVal;
}

bool comTest_paramParsing(){
	return false;
}

bool comTest_frameCaps(){
	int size = 7, i;
	uint8_t* buf = malloc(8);
	bool retVal;

	for(i = 0; i < 8; i++){
		buf[i] = i;
	}
	retVal = ProtoToFrame(buf, &size);
	if(retVal){
		retVal = FrameToProto(buf, &size);
	}

	for(i = 1; i < 8; i++){
		if(buf[i] == i){
			retVal = true;
		}else{
			retVal = false;
			break;
		}
	}
	return retVal;
}

bool comTest_rcvParams(){
	bool retVal = false;
	for(int i = 0; i < 2; i++){
		while(false == getParamFlag()){};
		processRegulationData();
	}
	if((param_d > 9.89f && param_d < 9.91f) && (param_i > 8.79f && param_i < 8.81f) && (param_p > 7.69f && param_p < 7.71f)){
		retVal = true;
	}
	return retVal;
}

bool comTest_sendData(){
	bool retVal;
	Sensordata data;
	uint32_t j;
	data.angle = 45.6;
	data.hallpattern = 3;
	data.temperature0 = 21;
	data.velocity = 4000;
	for(int i = 0; i < 3; i++){
		retVal = sendSensorData(&data);

		/*----delay ~10ms---*/
		  for(j = 0UL; j < 720000 ;++j)
		  {
			  //asm("nop");
		  }
		/*---delay end---*/
		data.angle+=2;
		data.hallpattern += 2;
		data.temperature0 += 2;
		data.velocity -= 2;
	}
	return retVal;
}
