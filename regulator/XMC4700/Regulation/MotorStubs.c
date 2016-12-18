#include "Motor/Motor.h"

extern double stubVelocity;

Std_ReturnType Motor_Initialize() {
	return E_OK;
}

Std_ReturnType Motor_SetVelocityPower(double to) {
	stubVelocity += to;
	return E_OK;
}
