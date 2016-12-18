/*
 * unitTest.h
 *
 *  Created on: Nov 29, 2016
 *      Author: Michael
 */

#ifndef UNITTEST_H_
#define UNITTEST_H_

#include <stdbool.h>

/*
 * bool comTest_sensorParsing()
 * params: none
 * return: Was the parsing successful?
 * Description: Test the parsing of SensorData to Protobuf and back;
 */
extern bool comTest_sensorParsing();

/*
 * bool comTest_paramParsing()
 * params: none
 * return: Was the parsing successful?
 * Description: Test the parsing of Parameters to Protobuf and back;
 */
extern bool comTest_paramParsing();

/*
 * bool comTest_frameCaps()
 * params: none
 * return Was the encapsulation and decapsulation successful?
 * Description: Test the encapsulation of a bytearray to a frame and back;
 */
extern bool comTest_frameCaps();

extern bool comTest_rcvParams();

extern bool comTest_sendData();

#endif /* UNITTEST_H_ */
