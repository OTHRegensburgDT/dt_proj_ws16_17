#include "ComHandler.h"
#include "RegulationHandler.h"

Std_ReturnType ComHandler_SendSensorReadings()  {

	return E_OK;
}

Std_ReturnType ComHandler_UpdatePidValues() {
	processRegulationData();
	struct Regulation_PidValues* TargetValues;
	switch(regulationTarget) {
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

TargetValues->Kp = param_p;
TargetValues->Ki = 	param_i;
TargetValues->Kd = param_d;
TargetValues->targetValue = target_val;
return E_OK;
}
