#include "ComHandler.h"
#include "RegulationHandler.h"

static Sensordata SensorDataToSend; // struct containing the information which shall be sent, used in SendSensorReadings

Std_ReturnType ComHandler_SendSensorReadings(double velocity, double angle) {
	SensorDataToSend.velocity = velocity;
	SensorDataToSend.angle = angle;
	SensorDataToSend.temperature0 = 9.4;
	return sendSensorData(&SensorDataToSend) ? E_OK : E_NOT_OK;
}

Std_ReturnType ComHandler_UpdatePidValues() {
	// let com process the data
	processRegulationData();

	// choose the target struct by the regulationTarget
	struct Regulation_PidValues* TargetValues;
	switch (regulationTarget) {
	case ANGLE:
		TargetValues = &Regulation_AngleVariables;
		break;
	case TEMPERATURE:
		TargetValues = &Regulation_TemperatureVariables;
		break;
	case VELOCITY:
		TargetValues = &Regulation_VelocityVariables;
		break;
	default:
		return E_NOT_OK;
	}

	// set the values
	TargetValues->Kp = param_p;
	TargetValues->Ki = param_i;
	TargetValues->Kd = param_d;
	TargetValues->targetValue = target_val;
	return E_OK;
}
