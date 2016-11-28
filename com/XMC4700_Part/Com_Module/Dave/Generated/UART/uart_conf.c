/*********************************************************************************************************************
* DAVE APP Name : UART       APP Version: 4.1.8
*
* NOTE:
* This file is generated by DAVE. Any manual modification done to this file will be lost when the code is regenerated.
*********************************************************************************************************************/

/**
 * @cond
 ***********************************************************************************************************************
 *
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
 * 2015-02-16:
 *     - Initial version for DAVEv4.
 *
 * 2015-06-20:
 *     - Service request value moved from runtime structure to config structure.
 *
 * 2015-07-06:
 *     - Structure name changed from UART_DYNAMIC_t to UART_RUNTIME_t
 *
 * 2015-07-30:
 *     - Added DMA and Direct mode configuration
 * @endcond
 *
 */
/***********************************************************************************************************************
 * HEADER FILES
 **********************************************************************************************************************/
#include "uart.h"

/***********************************************************************************************************************
 * EXTERN DECLARATIONS
 ***********************************************************************************************************************/

/*
 * Function monitors the configured protocol interrupt flags. It is called from the protocol interrupt
 * service handler.
 * Function reads the status of the USIC channel and checks for configured flags in the APP UI.
 * If any callback function is provided in the APP UI, it will be called when the selected flag is set.
 */
extern void UART_lProtocolHandler(const UART_t * const handle);


/**********************************************************************************************************************
 * DATA STRUCTURES
 **********************************************************************************************************************/
UART_STATUS_t UART_0_init(void);
void UART_0_dma_tx_handler(XMC_DMA_CH_EVENT_t event);
void UART_0_dma_rx_handler(XMC_DMA_CH_EVENT_t event);

/*USIC channel configuration*/
const XMC_UART_CH_CONFIG_t UART_0_channel_config =
{
  .baudrate      = 19200U,
  .data_bits     = 8U,
  .frame_length  = 8U,
  .stop_bits     = 1U,
  .oversampling  = 16U,
  .parity_mode   = XMC_USIC_CH_PARITY_MODE_EVEN
};
/*Transmit pin configuration*/
const XMC_GPIO_CONFIG_t UART_0_tx_pin_config   = 
{ 
  .mode             = XMC_GPIO_MODE_OUTPUT_PUSH_PULL_ALT2, 
  .output_level     = XMC_GPIO_OUTPUT_LEVEL_HIGH,
  .output_strength  = XMC_GPIO_OUTPUT_STRENGTH_STRONG_SOFT_EDGE
};

/*Transmit pin configuration used for initializing*/
const UART_TX_CONFIG_t UART_0_tx_pin = 
{
  .port = (XMC_GPIO_PORT_t *)PORT1_BASE,
  .config = &UART_0_tx_pin_config,
  .pin = 5U
};

const XMC_DMA_CH_CONFIG_t UART_0_tx_dma_ch_config =
{
  .enable_interrupt = true,
  .dst_transfer_width = (uint32_t)XMC_DMA_CH_TRANSFER_WIDTH_8,
  .src_transfer_width = (uint32_t)XMC_DMA_CH_TRANSFER_WIDTH_8,
  .dst_address_count_mode = (uint32_t)XMC_DMA_CH_ADDRESS_COUNT_MODE_NO_CHANGE,
  .src_address_count_mode = (uint32_t)XMC_DMA_CH_ADDRESS_COUNT_MODE_INCREMENT,
  .dst_burst_length = (uint32_t)XMC_DMA_CH_BURST_LENGTH_1,
  .src_burst_length = (uint32_t)XMC_DMA_CH_BURST_LENGTH_8,
  .transfer_flow = (uint32_t)XMC_DMA_CH_TRANSFER_FLOW_M2P_DMA,
  .transfer_type = (uint32_t)XMC_DMA_CH_TRANSFER_TYPE_SINGLE_BLOCK,
  .dst_handshaking = (uint32_t)XMC_DMA_CH_DST_HANDSHAKING_HARDWARE,
  .dst_peripheral_request = DMA_PERIPHERAL_REQUEST(5U, 10U), /*DMA0_PERIPHERAL_REQUEST_USIC0_SR0_5*/
};

const UART_DMA_CONFIG_t UART_0_tx_dma_config =
{
  .dma_ch_config = &UART_0_tx_dma_ch_config,
  .dma_channel   = 4U
};

const XMC_DMA_CH_CONFIG_t UART_0_rx_dma_ch_config =
{
  .enable_interrupt = true,
  .dst_transfer_width = (uint32_t)XMC_DMA_CH_TRANSFER_WIDTH_8,
  .src_transfer_width = (uint32_t)XMC_DMA_CH_TRANSFER_WIDTH_8,
  .dst_address_count_mode = (uint32_t)XMC_DMA_CH_ADDRESS_COUNT_MODE_INCREMENT,
  .src_address_count_mode = (uint32_t)XMC_DMA_CH_ADDRESS_COUNT_MODE_NO_CHANGE,
  .dst_burst_length = (uint32_t)XMC_DMA_CH_BURST_LENGTH_8,
  .src_burst_length = (uint32_t)XMC_DMA_CH_BURST_LENGTH_1,
  .transfer_flow = (uint32_t)XMC_DMA_CH_TRANSFER_FLOW_P2M_DMA,
  .transfer_type = (uint32_t)XMC_DMA_CH_TRANSFER_TYPE_SINGLE_BLOCK,
  .src_handshaking = (uint32_t)XMC_DMA_CH_SRC_HANDSHAKING_HARDWARE,
  .src_peripheral_request = DMA_PERIPHERAL_REQUEST(2U, 11U), /*DMA0_PERIPHERAL_REQUEST_USIC0_SR1_2*/
};

const UART_DMA_CONFIG_t UART_0_rx_dma_config =
{
  .dma_ch_config = &UART_0_rx_dma_ch_config,
  .dma_channel   = 5U
};

/*UART APP configuration structure*/
const UART_CONFIG_t UART_0_config = 
{
  .channel_config   = &UART_0_channel_config,

  .global_dma    = &GLOBAL_DMA_0,
  .transmit_dma_config = &UART_0_tx_dma_config,
  .receive_dma_config = &UART_0_rx_dma_config,
  .fptr_uart_config = UART_0_init,  
  .sync_error_cbhandler = NULL,  
  .rx_noise_error_cbhandler = NULL,  
  .format_error_bit0_cbhandler = NULL,  
  .format_error_bit1_cbhandler = NULL,  
  .collision_error_cbhandler = NULL,
  .tx_pin_config    = &UART_0_tx_pin,
  .mode             = UART_MODE_FULLDUPLEX,
  .transmit_mode = UART_TRANSFER_MODE_DMA,
  .receive_mode = UART_TRANSFER_MODE_DMA,
  .tx_fifo_size     = XMC_USIC_CH_FIFO_DISABLED,
  .rx_fifo_size     = XMC_USIC_CH_FIFO_DISABLED,
};

/*Runtime handler*/
UART_RUNTIME_t UART_0_runtime = 
{
  .tx_busy = false,  
  .rx_busy = false,
};

/*APP handle structure*/
UART_t UART_0 = 
{
  .channel = XMC_UART0_CH0,
  .config  = &UART_0_config,
  .runtime = &UART_0_runtime
};

/*Receive pin configuration*/
const XMC_GPIO_CONFIG_t UART_0_rx_pin_config   = {
  .mode             = XMC_GPIO_MODE_INPUT_TRISTATE,
  .output_level     = XMC_GPIO_OUTPUT_LEVEL_HIGH,
  .output_strength  = XMC_GPIO_OUTPUT_STRENGTH_STRONG_SOFT_EDGE
};
/**********************************************************************************************************************
 * API IMPLEMENTATION
 **********************************************************************************************************************/
/*Channel initialization function*/
UART_STATUS_t UART_0_init()
{
  UART_STATUS_t status = UART_STATUS_SUCCESS;
  status = (UART_STATUS_t)GLOBAL_DMA_Init(&GLOBAL_DMA_0);
  XMC_DMA_CH_Init(XMC_DMA0, 4U, &UART_0_tx_dma_ch_config);
  XMC_DMA_CH_EnableEvent(XMC_DMA0,  4U, XMC_DMA_CH_EVENT_TRANSFER_COMPLETE);

  XMC_DMA_CH_Init(XMC_DMA0, 5U, &UART_0_rx_dma_ch_config);
  XMC_DMA_CH_EnableEvent(XMC_DMA0,  5U, XMC_DMA_CH_EVENT_TRANSFER_COMPLETE);

  /*Configure Receive pin*/
  XMC_GPIO_Init((XMC_GPIO_PORT_t *)PORT1_BASE, 4U, &UART_0_rx_pin_config);
  /* Initialize USIC channel in UART mode*/
  XMC_UART_CH_Init(XMC_UART0_CH0, &UART_0_channel_config);
  /*Set input source path*/
  XMC_USIC_CH_SetInputSource(XMC_UART0_CH0, XMC_USIC_CH_INPUT_DX0, 1U);
  /* Start UART */
  XMC_UART_CH_Start(XMC_UART0_CH0);

  /* Initialize UART TX pin */
  XMC_GPIO_Init((XMC_GPIO_PORT_t *)PORT1_BASE, 5U, &UART_0_tx_pin_config);

  /*Set service request for transmit interrupt*/
  XMC_USIC_CH_SetInterruptNodePointer(XMC_UART0_CH0, XMC_USIC_CH_INTERRUPT_NODE_POINTER_TRANSMIT_BUFFER,
     0U);
  /*Set service request for receive interrupt*/
  XMC_USIC_CH_SetInterruptNodePointer(XMC_UART0_CH0, XMC_USIC_CH_INTERRUPT_NODE_POINTER_RECEIVE,
     1U);
  XMC_USIC_CH_SetInterruptNodePointer(XMC_UART0_CH0, XMC_USIC_CH_INTERRUPT_NODE_POINTER_ALTERNATE_RECEIVE,
     1U);
  /*Set service request for UART protocol events*/
  XMC_USIC_CH_SetInterruptNodePointer(XMC_UART0_CH0, XMC_USIC_CH_INTERRUPT_NODE_POINTER_PROTOCOL,
     2U);
  /*Register transfer complete event handler*/
  XMC_DMA_CH_SetEventHandler(XMC_DMA0, 5U, UART_0_dma_rx_handler);
  /*Register transfer complete event handler*/
  XMC_DMA_CH_SetEventHandler(XMC_DMA0, 4U, UART_0_dma_tx_handler);
  /* make DMA ready for transmission*/
  XMC_USIC_CH_TriggerServiceRequest(XMC_UART0_CH0, 0U); 
  return status;
}

void UART_0_dma_tx_handler(XMC_DMA_CH_EVENT_t event)
{
  if(event == XMC_DMA_CH_EVENT_TRANSFER_COMPLETE)
  {
    UART_0.runtime->tx_busy = false;
  }
}


void UART_0_dma_rx_handler(XMC_DMA_CH_EVENT_t event)
{
  if(event == XMC_DMA_CH_EVENT_TRANSFER_COMPLETE)
  {
    UART_0.runtime->rx_busy = false;
    DataRcvICR();

  }
}

/*CODE_BLOCK_END*/

