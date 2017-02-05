/*
 * Motor_PWMSchemes.h
 *
 *  Created on: Jan 4, 2017
 *      Author: Andreas Kölbl
 */

#ifndef MOTOR_PWMSCHEMES_H
#define MOTOR_PWMSCHEMES_H

#include <stdint.h>

void Motor_Scheme_Default(uint8_t* currentPattern, uint8_t position);
void Motor_Scheme_PWM_ON(uint8_t* currentPattern, uint8_t position);


#endif /* MOTOR_PWMSCHEMES_H_ */
