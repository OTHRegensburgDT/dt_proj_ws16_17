
/***********************************************************************************************************************
 * HEADER FILES
 **********************************************************************************************************************/
#include "bldc_scalar_user_interface.h"
#include "uCProbe.h"
#include "xmc4_flash.h"
/***********************************************************************************************************************
 * MACROS
 **********************************************************************************************************************/
#define BLDC_SCALAR_MCM_PATTERN_SIZE (15U)                                            /*Multi-channel pattern array size*/

/***********************************************************************************************************************
 * LOCAL DATA
 **********************************************************************************************************************/
#if (MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1U)
#if (MOTOR0_BLDC_SCALAR_HALL_LEARNING == 0U)
BLDC_SCALAR_HALL_LEARNING_t Motor0_BLDC_SCALAR_HallLearning;
#endif
#if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_VOLTAGE_CTRL))
BLDC_SCALAR_VOLTAGE_CONTROL_t Motor0_BLDC_SCALAR_VoltageControl;
#endif
#if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_SPEED_CTRL) && (MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_SPEEDCURRENT_CTRL))
BLDC_SCALAR_SPEED_CONTROL_t Motor0_BLDC_SCALAR_SpeedControl;
BLDC_SCALAR_PI_CONTROLLER_t Motor0_BLDC_SCALAR_SpeedControl_PI;
#endif
#if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_CURRENT_CTRL))
BLDC_SCALAR_CURRENT_CONTROL_t Motor0_BLDC_SCALAR_CurrentControl;
#endif
#if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_CURRENT_CTRL) && (MOTOR0_BLDC_SCALAR_CTRL_SCHEME != BLDC_SCALAR_SPEEDCURRENT_CTRL))
BLDC_SCALAR_PI_CONTROLLER_t Motor0_BLDC_SCALAR_CurrentControl_PI;
#endif
#endif /*End of #if (MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1U)*/
/**********************************************************************************************************************
* DATA STRUCTURES
**********************************************************************************************************************/
BLDC_SCALAR_UCPROBE_t Motor0_BLDC_SCALAR_ucprobe;
/***********************************************************************************************************************
 * LOCAL ROUTINES
 **********************************************************************************************************************/
/*Update all the UCPROBE UI variables*/
void Motor0_BLDC_SCALAR_uCPorbe_Update_UI_Var(void);

/**********************************************************************************************************************
 * API IMPLEMENTATION
 **********************************************************************************************************************/
#if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))
/*UCproBE scheduler function to handle ucprobe comments from UI */
void Motor0_BLDC_SCALAR_uCProbe_Scheduler(void)
{
  switch(Motor0_BLDC_SCALAR_ucprobe.control_word)
  {
    case 1:  /* Start the motor */
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      Motor0_BLDC_SCALAR_MotorStart();
      break;

    case 2: /*Stop the motor*/
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      Motor0_BLDC_SCALAR_MotorStop();
      break;

    case 3: /*Clear Error state*/
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      Motor0_BLDC_SCALAR_ClearErrorState();
      break;

    case 6:  /*Learn Hall pattern*/
      #if (MOTOR0_BLDC_SCALAR_HALL_LEARNING == 1U)
      if((Motor0_BLDC_SCALAR.msm_state == BLDC_SCALAR_MSM_STOP)&&((uint8_t)BLDC_SCALAR_HALL_LEARNING_FLAG_DISABLED == Motor0_BLDC_SCALAR_Hall.hall_learning_flag))
      {
        Motor0_BLDC_SCALAR_Hall.hall_learning_flag =  BLDC_SCALAR_HALL_LEARNING_FLAG_ENABLED;
        Motor0_BLDC_SCALAR_HallLearning.adapt_algoindex=0;
      }
      Motor0_BLDC_SCALAR_MotorStart();
      if (((uint8_t)BLDC_SCALAR_HALL_LEARNING_FLAG_DISABLED == Motor0_BLDC_SCALAR_Hall.hall_learning_flag))
      {
    	Motor0_BLDC_SCALAR_ucprobe.control_word=0;
        Motor0_BLDC_SCALAR_MotorStop();
      }
      #else
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      #endif
      break;

    case 8:  /*Disable Hall learning*/
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      #if (MOTOR0_BLDC_SCALAR_HALL_LEARNING == 1U)
      Motor0_BLDC_SCALAR_Hall.hall_learning_flag=BLDC_SCALAR_HALL_LEARNING_FLAG_DISABLED;
      #endif
      break;

    case 9:  /*Direction change*/
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      #if(MOTOR0_BLDC_SCALAR_ENABLE_BIDIRECTIONAL_CTRL == 0U)
      Motor0_BLDC_SCALAR_SetDirection(Motor0_BLDC_SCALAR.motor_set_direction*-1);
      #endif
      break;

    default:
      Motor0_BLDC_SCALAR_ucprobe.control_word=0;
      break;
  } /*End of switch(Motor0_BLDC_SCALAR_ucprobe.control_word)*/
  Motor0_BLDC_SCALAR_uCPorbe_Update_UI_Var();
}
#endif /* End of #if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))*/

/***********************************************************************************************************************/
#if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))
/* UCPROBE variable initialization */
void Motor0_BLDC_SCALAR_uCProbe_Init(void)
{
  Motor0_BLDC_SCALAR_ucprobe.max_speed_RPM = MOTOR0_BLDC_SCALAR_MOTOR_NO_LOAD_SPEED;
  Motor0_BLDC_SCALAR_ucprobe.max_current_mA = (uint32_t)(MOTOR0_BLDC_SCALAR_MAX_CURRENT*1000);
  Motor0_BLDC_SCALAR_ucprobe.max_voltage_Volt = (uint32_t)MOTOR0_BLDC_SCALAR_NOMINAL_DC_LINK_VOLT;

  #if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_OSC_ENABLE ==1) && (MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))
  ProbeScope_Init(20000);
  #endif

}
#endif /*#if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))*/

/***********************************************************************************************************************
 * LOCAL ROUTINES
 **********************************************************************************************************************/
#if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))

void Motor0_BLDC_SCALAR_uCPorbe_Update_UI_Var(void)
{
  Motor0_BLDC_SCALAR_ucprobe.dclinkgvoltage_Volt = ((int32_t)(Motor0_BLDC_SCALAR.dclink_voltage *MOTOR0_BLDC_SCALAR_BASE_VOLTAGE*10) >>14);
  Motor0_BLDC_SCALAR_ucprobe.motor_current_mA =	((int32_t)(Motor0_BLDC_SCALAR.motor_current*MOTOR0_BLDC_SCALAR_BASE_CURRENT*1000)>>14);
  Motor0_BLDC_SCALAR_ucprobe.motor_speed_RPM =  ((int32_t)(Motor0_BLDC_SCALAR.motor_speed*MOTOR0_BLDC_SCALAR_BASE_SPEED_MECH_RPM)>>14);


  #if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEED_CTRL) || (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEEDCURRENT_CTRL))
  if (Motor0_BLDC_SCALAR.msm_state != BLDC_SCALAR_MSM_STOP)
  {
    Motor0_BLDC_SCALAR_ucprobe.speed_pi_error = (int32_t)(Motor0_BLDC_SCALAR_SpeedControl.ref_speed) - (int32_t)(Motor0_BLDC_SCALAR_SpeedControl.fdbk_speed);
    Motor0_BLDC_SCALAR_ucprobe.speed_set_RPM = ((int32_t)(Motor0_BLDC_SCALAR_SpeedControl.ref_speed*MOTOR0_BLDC_SCALAR_BASE_SPEED_MECH_RPM)>>14);
  }
  else
  {
    Motor0_BLDC_SCALAR_ucprobe.speed_pi_error=0;
    Motor0_BLDC_SCALAR_ucprobe.speed_set_RPM=0;
  }
  #endif
  #if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_CURRENT_CTRL) || (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEEDCURRENT_CTRL))
  if (Motor0_BLDC_SCALAR.msm_state != BLDC_SCALAR_MSM_STOP)
  {
    Motor0_BLDC_SCALAR_ucprobe.current_pi_error=(int32_t)(Motor0_BLDC_SCALAR_CurrentControl.ref_current) - (int32_t)(Motor0_BLDC_SCALAR_CurrentControl.fdbk_current);
  }
  else
  {
    Motor0_BLDC_SCALAR_ucprobe.current_pi_error=0;
  }
  #endif

  #if (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_CURRENT_CTRL)
  if (Motor0_BLDC_SCALAR.msm_state != BLDC_SCALAR_MSM_STOP)
  {
    Motor0_BLDC_SCALAR_ucprobe.current_set_mA = ((int32_t)(Motor0_BLDC_SCALAR_CurrentControl.ref_current*MOTOR0_BLDC_SCALAR_BASE_CURRENT*1000)>>14);
  }
  else
  {
    Motor0_BLDC_SCALAR_ucprobe.current_set_mA =0;
 }
  #endif

  #if (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_VOLTAGE_CTRL)
  if (Motor0_BLDC_SCALAR.msm_state != BLDC_SCALAR_MSM_STOP)
  {
    Motor0_BLDC_SCALAR_ucprobe.voltage_set_Volt = ((int32_t)(Motor0_BLDC_SCALAR_VoltageControl.ref_voltage *MOTOR0_BLDC_SCALAR_BASE_VOLTAGE*10) >>14);
  }
  else
  {
    Motor0_BLDC_SCALAR_ucprobe.voltage_set_Volt =0;
  }
	#endif

  if(Motor0_BLDC_SCALAR.msm_state == BLDC_SCALAR_MSM_STOP)
  {
    #if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEED_CTRL) || (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEEDCURRENT_CTRL))
    Motor0_BLDC_SCALAR_ucprobe.speed_pi_error = 0;
    Motor0_BLDC_SCALAR_SpeedControl_PI.uk=0;
    Motor0_BLDC_SCALAR_ucprobe.speed_set_RPM=0;
    #endif
    #if ((MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_CURRENT_CTRL) || (MOTOR0_BLDC_SCALAR_CTRL_SCHEME == BLDC_SCALAR_SPEEDCURRENT_CTRL))
    Motor0_BLDC_SCALAR_ucprobe.current_pi_error=0;
    Motor0_BLDC_SCALAR_CurrentControl_PI.uk=0;
    #endif

  }
}

#endif /*#if ((MOTOR0_BLDC_SCALAR_CTRL_UCPROBE_ENABLE==1))*/
