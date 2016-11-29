/*
 * Posif_Control.c
 *
 *  Created on: Nov 29, 2016
 *      Author: MotorXP
 */

XMC_POSIF_CONFIG_t POSIF_HALL_config =
{
	.mode = XMC_POSIF_MODE_HALL_SENSOR, /**< POSIF Operational mode */
	.input0 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-1 */
	.input1 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-2 */
	.input2 = XMC_POSIF_INPUT_PORT_A, /**< Choice of input for Input-3 */
	.filter = XMC_POSIF_FILTER_DISABLED /**< Input filter configuration */
};

XMC_POSIF_HSC_CONFIG_t POSIF_HSC_config =
{
	.disable_idle_signal = 1,
	.sampling_trigger = 0, //HSDA
	.sampling_trigger_edge = 0 //Rising edge
};

void POSIF0_0_IRQHandler(void)
{
	uint8_t hallposition;

	/* Set the new Hall pattern */
	hallposition = XMC_POSIF_HSC_GetExpectedPattern(POSIF_PTR);
	XMC_POSIF_HSC_SetHallPatterns(POSIF_PTR, Sensor_Hall_GetPattern(hallposition));

	SensorHallCallback();
}
