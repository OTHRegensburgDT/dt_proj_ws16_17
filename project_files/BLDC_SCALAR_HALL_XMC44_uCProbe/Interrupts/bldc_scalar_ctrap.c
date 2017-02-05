/**
 * @file bldc_scalar_ctrap.c
 * @brief Ctrap event of CCU8
 * @date 2016-08-31
 ***********************************************************************************************************************
 * BLDC_SCALAR_CONTROL v1.0.2 - BLDC motor control using block commutation
 * Copyright (c) 2015-2016, Infineon Technologies AG
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,are permitted provided that the
 * following conditions are met:
 *
 *   Redistributions of source code must retain the above copyright notice, this list of conditions and the  following
 *   disclaimer.
 *
 *   Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
 *   following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 *   Neither the name of the copyright holders nor the names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE  FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY,OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT  OF THE
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * To improve the quality of the software, users are encouraged to share modifications, enhancements or bug fixes
 * with Infineon Technologies AG (dave@infineon.com).
 ***********************************************************************************************************************
 *
 * Change History
 * --------------
 *
 * 2016-08-31:
 *     - Initial version
 *
 */
/**
 * @addtogroup BLDC_SCALAR BLDC Motor Control
 * @{
 */

/**
 * @addtogroup Interrupt Interrupts
 * @brief  Interrupt Service Routines  <br>
 * @{
 */
/***********************************************************************************************************************
 * HEADER FILES
 **********************************************************************************************************************/

#include "../ControlModule/bldc_scalar_control_scheme.h"
#include "../bldc_scalar_user_interface.h"

/**********************************************************************************************************************
* API IMPLEMENTATION
**********************************************************************************************************************/

/**
 * @param none
 * @return None <br>
 *
 * \par<b>Description:</b><br>
 * Trap event mapped to the Phase U (CCU8) event 2 \n
 * PWM is stopped by the hardware when this event is detected.
 * This ISR, stops the motor and clears the state of the control.
 */
#if (MOTOR0_BLDC_SCALAR_ENABLE_CTRAP == 1U)
void Motor0_BLDC_SCALAR_Trap_ISR(void)
{
  /* Update the error status and stop the motor */
  Motor0_BLDC_SCALAR.error_status |= (uint32_t)1 << (uint32_t)BLDC_SCALAR_EID_CTRAP_ERROR;
  Motor0_BLDC_SCALAR.msm_state = BLDC_SCALAR_MSM_ERROR;
  Motor0_BLDC_SCALAR_MotorStop();
}
#endif

/**
 * @}
 */
/**
 * @}
 */
