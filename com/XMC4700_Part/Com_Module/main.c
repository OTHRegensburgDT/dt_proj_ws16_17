/*
 * main.c
 *
 *  Created on: 2016 Oct 26 16:20:02
 *  Author: Michael
 */

#define SENSORDATA Com_Module_SensorMsg


#include <DAVE.h>                 //Declarations from DAVE Code Generation (includes SFR declaration)
#include <malloc.h>
#include "communication.h"
#include "protobuf/SensorMsg.pb.h"
#include "parser.h"
/**

 * @brief main() - Application entry point
 *
 * <b>Details of function</b><br>
 * This routine is the application entry point. It is invoked by the device startup code. It is responsible for
 * invoking the APP initialization dispatcher routine - DAVE_Init() and hosting the place-holder for user application
 * code.
 */

int main(void)
{
  DAVE_STATUS_t status;
  status = DAVE_Init();           /* Initialization of DAVE APPs  */

  Sensordata testData;

  testData.hallpattern = 4;
  testData.velocity =  6000.7;
  testData.angle = 360;
  testData.temperature0 = 77.3;
  testData.temperature1 = 77.9;

  if(status != DAVE_STATUS_SUCCESS)
  {
    /* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
    XMC_DEBUG("DAVE APPs initialization failed\n");

    while(1U)
    {

    }
  }
  sendSensorData(&testData);

  /* Placeholder for user application code. The while loop below can be replaced with user application code. */
  while(1U)
  {

  }
}
