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
/* PWM only on High */
#define	  MOTOR_AH P1_10 /* U */
#define	  MOTOR_AL P1_11
#define	  MOTOR_BH P3_10 /* V */
#define	  MOTOR_BL P3_8
#define	  MOTOR_CH P3_7  /* W */
#define	  MOTOR_CL P3_9

void Motor_Init();
void Motor_Main();

#endif /* MOTOR_INIT */



