#ifndef _COM_HANDLER_H_
#define _COM_HANDLER_H_

#include "Com/communication.h"
#include "Com/Frameparser.h"
#include "Com/Paramsparser.h"
#include "Com/Sensorparser.h"

#include "Std_Types.h"
#include "RegulationHandler.h"


/*
* <summary>
* Initializes the communication interface.
* </summary>
*/
#define COMHANDLER_INITIALIZE() while(!initCom())

/*
* <summary>
* Checks if new PID data has been received and if so, updates them
* </summary>
*/
#define COMHANDLER_UPDATE_PID_VALUES(velocityVariables, angleVariables, temperatureVariables) (getParamFlag() ?  ComHandler_UpdatePidValues(velocityVariables, angleVariables, temperatureVariables) : E_OK)

/*
* <summary>
* Sends sensor data
* </summary>
*/
Std_ReturnType ComHandler_SendSensorReadings(double velocity, double angle, double temperature);

/*
* <summary>
* Sets PID data based on processRegulationData method
* </summary>
*/
Std_ReturnType ComHandler_UpdatePidValues(struct Regulation_PidValues* velocityVariables, struct Regulation_PidValues* angleVariables, struct Regulation_PidValues* temperatureVariables);

#endif
