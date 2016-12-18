#include "Motor/Motor.h"

extern double stubVelocity;

void Motor_Initialize() {

}

void Motor_SetVelocityPower(double to) {
	stubVelocity += to;
}
