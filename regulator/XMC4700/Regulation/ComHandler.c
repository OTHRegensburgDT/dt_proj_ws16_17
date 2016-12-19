#include "ComHandler.h"

static Sensordata SensorDataToSend; // struct containing the information which shall be sent, used in SendSensorReadings

Std_ReturnType ComHandler_SendSensorReadings(double velocity, double angle, double temperature) {
	SensorDataToSend.velocity = velocity;
	SensorDataToSend.angle = angle;
	SensorDataToSend.temperature0 = temperature;
	return sendSensorData(&SensorDataToSend) ? E_OK : E_NOT_OK;
}

Std_ReturnType ComHandler_UpdatePidValues(struct Regulation_PidValues* velocityVariables, struct Regulation_PidValues* angleVariables, struct Regulation_PidValues* temperatureVariables) {
	// let com process the data
	processRegulationData();

	// choose the target struct by the regulationTarget
	struct Regulation_PidValues* TargetValues;
	switch (regulationTarget) {
	case ANGLE:
		TargetValues = angleVariables;
		break;
	case TEMPERATURE:
		TargetValues = temperatureVariables;
		break;
	case VELOCITY:
		TargetValues = velocityVariables;
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
