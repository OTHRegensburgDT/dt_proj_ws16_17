#ifndef _COM_HANDLER_H_
#define _COM_HANDLER_H_

#include "Com/communication.h"
#include "Com/Frameparser.h"
#include "Com/Paramsparser.h"
#include "Com/Sensorparser.h"

#include "Std_Types.h"


/*
* <summary>
* Initializes the communication interface.
* </summary>
*/
#define COMHANDLER_INITIALIZE() while(!initCom())

/*
* <summary>
* Receives new data and sets newDate flag.
* </summary>
*/
#define COMHANDLER_RECEIVE() DataRcvICR()

/*
* <summary>
* Checks if new PID data has been received and if so, updates them
* </summary>
*/
#define COMHANDLER_UPDATE_PID_VALUES() (getParamFlag() ?  ComHandler_UpdatePidValues() : E_OK)

/*
* <summary>
* Sends sensor data
* </summary>
*/
Std_ReturnType ComHandler_SendSensorReadings(double velocity, double angle);

/*
* <summary>
* Sets PID data based on processRegulationData method
* </summary>
*/
Std_ReturnType ComHandler_UpdatePidValues();

#endif
