#ifndef MOTOR_H
#define MOTOR_H

/* 
 * Needed input:
 *  Function Selector -> PCONF.FSEL = 11, Multi-Channel & Quadrature Decoder
 *      POSIF0.IN0[D...A] ... POSIF0.IN2[D...A] 
 *      POSIF0.OUT[6...0]
 *
 *  Hall Sensor Control:
 *      POSIF
 *      
 *  
 */ 
#include <xmc_gpio.h>
#define	  MOTOR_AH P0_5 /* U */
#define	  MOTOR_AL P0_2
#define	  MOTOR_BH P0_4 /* V */
#define	  MOTOR_BL P0_1
#define	  MOTOR_CH P0_3  /* W */
#define	  MOTOR_CL P0_0

void Motor_Init();
void Motor_Main();
#endif /* MOTOR_INIT */



