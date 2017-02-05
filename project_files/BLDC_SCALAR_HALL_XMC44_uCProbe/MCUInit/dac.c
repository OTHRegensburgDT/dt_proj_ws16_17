/**
 * @file dac.c
 * @brief DAC configuration
 * -# DAC is optionally used to
 * provide 1.65V offset for current measurement.
 * @date 2016-08-31
 *
  **********************************************************************************************************************
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
 * - Initial version
 *
 * @endcond
 *
 */

/***********************************************************************************************************************
 * HEADER FILES
 **********************************************************************************************************************/
#include "dac.h"

/**********************************************************************************************************************
 * DATA STRUCTURES
 **********************************************************************************************************************/
#if (MOTOR0_BLDC_SCALAR_OFFSET_DAC == 1U)
/* DAC Channel Configuration */
XMC_DAC_CH_CONFIG_t DAC_0_Channel_Config =
{
  .data_type       = (uint8_t)XMC_DAC_CH_DATA_TYPE_UNSIGNED,
  .output_negation = (uint8_t)XMC_DAC_CH_OUTPUT_NEGATION_DISABLED,
  .output_scale    = (uint8_t)XMC_DAC_CH_OUTPUT_SCALE_NONE,
  .output_offset   = 0U
};

/***********************************************************************************************************************
 * LOCAL ROUTINES
 **********************************************************************************************************************/

/**********************************************************************************************************************
* API IMPLEMENTATION
**********************************************************************************************************************/
void Motor0_BLDC_SCALAR_DAC_Init(void)
{
/* Enable DAC module and configure the channel */
  XMC_DAC_CH_Init((XMC_DAC_t*)(void *)DAC,DAC_CHANNEL_0,&DAC_0_Channel_Config);
  /* start single value mode for configured channel */
  XMC_DAC_CH_StartSingleValueMode((XMC_DAC_t  *)(void *)DAC,DAC_CHANNEL_0);
}
#endif
