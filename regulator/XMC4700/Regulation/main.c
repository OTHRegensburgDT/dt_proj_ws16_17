/*
 * main.c
 *
 *  Created on: 2016 Oct 26 16:20:02
 *  Author: Michael
 */

#define SENSORDATA Regulation_SensorMsg


#include <DAVE.h>                 //Declarations from DAVE Code Generation (includes SFR declaration)
#include <malloc.h>
#include "Paramsparser.h"
#include "communication.h"
#include "unitTest.h"
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
	bool testval;
  DAVE_STATUS_t status;
  status = DAVE_Init();           /* Initialization of DAVE APPs  */
  Sensordata testData;
  uint8_t buffer[256];
  int size = 256;

  if(status != DAVE_STATUS_SUCCESS)
  {
    /* Placeholder for error handler code. The while loop below can be replaced with an user error handler. */
    XMC_DEBUG("DAVE APPs initialization failed\n");

    while(1U)
    {

    }
  }
  testval = comTest_frameCaps();
  initCom();
  param_p = 1.1;
  param_i = 2.2;
  param_d = 3.3;
  target_val = 45;

  /* Placeholder for user application code. The while loop below can be replaced with user application code. */

  testval = comTest_sendData();
  return 0;
}
