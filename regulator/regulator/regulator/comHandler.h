#ifndef _COM_HANDLER_H_
#define _COM_HANDLER_H_

#include "../../../com/XMC4700_Part/Com_Module/communication.h"
#include "../../../com/XMC4700_Part/Com_Module/Frameparser.h"
#include "../../../com/XMC4700_Part/Com_Module/Paramsparser.h"
#include "../../../com/XMC4700_Part/Com_Module/Sensorparser.h"


#include "../../../com/XMC4700_Part/Com_Module/com_structs.h"
#include "../../../com/XMC4700_Part/Com_Module/Sensorparser.h"

/*
* <summary>
* Initializes the communication interface.
* </summary>
*/
#define COMHANDLER_INITIALIZE() ;

// buffer and size for paramsparser.h
uint8_t buffer[256];
int size = 256;


#endif
