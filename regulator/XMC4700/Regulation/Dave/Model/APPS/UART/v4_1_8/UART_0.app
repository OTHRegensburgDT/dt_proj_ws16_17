<?xml version="1.0" encoding="ASCII"?>
<ResourceModel:App xmi:version="2.0" xmlns:xmi="http://www.omg.org/XMI" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ResourceModel="http://www.infineon.com/Davex/Resource.ecore" name="UART" URI="http://resources/4.1.8/app/UART/0" description="UART APP configures a USIC channel to perform &#xA;transmit &amp; receive operations using UART protocol." mode="NOTSHARABLE" version="4.1.8" minDaveVersion="4.0.0" instanceLabel="UART_0" appLabel="" containingProxySignal="true">
  <properties provideInit="true"/>
  <virtualSignals name="dx0_input_stage" URI="http://resources/4.1.8/app/UART/0/vs_uart_dx0ins" hwSignal="dx0ins" hwResource="//@hwResources.0"/>
  <virtualSignals name="dx3_input_stage" URI="http://resources/4.1.8/app/UART/0/vs_uart_dx3ins" hwSignal="dx3ins" hwResource="//@hwResources.0"/>
  <virtualSignals name="dout0_output" URI="http://resources/4.1.8/app/UART/0/vs_uart_dout0" hwSignal="dout0" hwResource="//@hwResources.0"/>
  <virtualSignals name="rxd_pin" URI="http://resources/4.1.8/app/UART/0/vs_uart_rxpad" hwSignal="pad" hwResource="//@hwResources.1" visible="true"/>
  <virtualSignals name="txd_pin" URI="http://resources/4.1.8/app/UART/0/vs_uart_txpad" hwSignal="pad" hwResource="//@hwResources.2" visible="true"/>
  <virtualSignals name="event_transmit_buffer" URI="http://resources/4.1.8/app/UART/0/vs_uart_std_tx_int" hwSignal="transmit_buffer_int" hwResource="//@hwResources.0"/>
  <virtualSignals name="event_std_receive" URI="http://resources/4.1.8/app/UART/0/vs_uart_std_rx_int" hwSignal="standard_receive_int" hwResource="//@hwResources.0"/>
  <virtualSignals name="event_alt_receive" URI="http://resources/4.1.8/app/UART/0/vs_uart_alt_rx_int" hwSignal="alternate_receive_int" hwResource="//@hwResources.0"/>
  <virtualSignals name="event_protocol" URI="http://resources/4.1.8/app/UART/0/vs_uart_proto_specfic_int" hwSignal="protocol_specific_int" hwResource="//@hwResources.0" visible="true"/>
  <virtualSignals name="event_fifo_transmit" URI="http://resources/4.1.8/app/UART/0/vs_uart_fifo_tx_int" hwSignal="standard_transmit_buffer_int" hwResource="//@hwResources.0" required="false"/>
  <virtualSignals name="event_fifo_std_receive" URI="http://resources/4.1.8/app/UART/0/vs_uart_fifo_rx_int" hwSignal="standard_receive_buffer_int" hwResource="//@hwResources.0" required="false"/>
  <virtualSignals name="event_fifo_alt_receive" URI="http://resources/4.1.8/app/UART/0/vs_uart_fifo_alt_rx_int" hwSignal="alternate_receive_buffer_int" hwResource="//@hwResources.0" required="false"/>
  <virtualSignals name="sr_transmit" URI="http://resources/4.1.8/app/UART/0/vs_interrupt_uart_tx" hwSignal="signal_in" hwResource="//@hwResources.3" required="false"/>
  <virtualSignals name="sr_receive" URI="http://resources/4.1.8/app/UART/0/vs_interrupt_uart_rx" hwSignal="signal_in" hwResource="//@hwResources.4" required="false"/>
  <virtualSignals name="sr_protocol_events" URI="http://resources/4.1.8/app/UART/0/vs_interrupt_uart_error" hwSignal="signal_in" hwResource="//@hwResources.5" required="false"/>
  <virtualSignals name="source_trigger" URI="http://resources/4.1.8/app/UART/0/vs_src_trigger_dlr_rx_in" hwSignal="in" hwResource="//@hwResources.9"/>
  <virtualSignals name="destination_trigger" URI="http://resources/4.1.8/app/UART/0/vs_dst_trigger_dlr_tx_in" hwSignal="in" hwResource="//@hwResources.8"/>
  <virtualSignals name="dlr_out" URI="http://resources/4.1.8/app/UART/0/vs_src_trigger_dlr_rx_out" hwSignal="out" hwResource="//@hwResources.9"/>
  <virtualSignals name="dlr_out" URI="http://resources/4.1.8/app/UART/0/vs_dst_trigger_dlr_tx_out" hwSignal="out" hwResource="//@hwResources.8"/>
  <virtualSignals name="DMA Transfer complete event" URI="http://resources/4.1.8/app/UART/0/vs_dma_interrupt_tx" hwSignal="dma_int" hwResource="//@hwResources.6"/>
  <virtualSignals name="DMA Transfer complete event" URI="http://resources/4.1.8/app/UART/0/vs_dma_interrupt_rx" hwSignal="dma_int" hwResource="//@hwResources.7"/>
  <virtualSignals name="Destination request" URI="http://resources/4.1.8/app/UART/0/vs_dma_ch_dst_req_tx" hwSignal="dest_req" hwResource="//@hwResources.6"/>
  <virtualSignals name="Source request" URI="http://resources/4.1.8/app/UART/0/vs_dma_ch_src_req_rx" hwSignal="src_req" hwResource="//@hwResources.7"/>
  <virtualSignals name="Receive Pin_signal" URI="http://resources/4.1.8/app/UART/0/__pin_vs_uart_rxpad" hwSignal="pin" hwResource="//@hwResources.10"/>
  <virtualSignals name="Transmit Pin_signal" URI="http://resources/4.1.8/app/UART/0/__pin_vs_uart_txpad" hwSignal="pin" hwResource="//@hwResources.11"/>
  <requiredApps URI="http://resources/4.1.8/app/UART/0/appres_clock" requiredAppName="CLOCK_XMC4" requiringMode="SHARABLE">
    <downwardMapList xsi:type="ResourceModel:App" href="../../CLOCK_XMC4/v4_0_22/CLOCK_XMC4_0.app#/"/>
  </requiredApps>
  <requiredApps URI="http://resources/4.1.8/app/UART/0/appres_cpu" requiredAppName="CPU_CTRL_XMC4" required="false" requiringMode="SHARABLE"/>
  <requiredApps URI="http://resources/4.1.8/app/UART/0/appres_global_dma" requiredAppName="GLOBAL_DMA" requiringMode="SHARABLE">
    <downwardMapList xsi:type="ResourceModel:App" href="../../GLOBAL_DMA/v4_0_8/GLOBAL_DMA_0.app#/"/>
  </requiredApps>
  <hwResources name="Channel" URI="http://resources/4.1.8/app/UART/0/hwres_usic_ch" resourceGroupUri="peripheral/usic/*/channel/*" mResGrpUri="peripheral/usic/*/channel/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/usic0/usic0_0.dd#//@provided.0"/>
  </hwResources>
  <hwResources name="Receive Pin" URI="http://resources/4.1.8/app/UART/0/hwres_port_pad_uart_rx" resourceGroupUri="port/p/*/pad/*" mResGrpUri="port/p/*/pad/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/port1/port1_1.dd#//@provided.10"/>
  </hwResources>
  <hwResources name="Transmit Pin" URI="http://resources/4.1.8/app/UART/0/hwres_port_pad_uart_tx" resourceGroupUri="port/p/*/pad/*" mResGrpUri="port/p/*/pad/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/port1/port1_1.dd#//@provided.15"/>
  </hwResources>
  <hwResources name="NVIC Node Tx" URI="http://resources/4.1.8/app/UART/0/hwres_nvic_node_tx" resourceGroupUri="" required="false" mResGrpUri="peripheral/cpu/0/nvic/interrupt/*"/>
  <hwResources name="NVIC Node Rx" URI="http://resources/4.1.8/app/UART/0/hwres_nvic_node_rx" resourceGroupUri="" required="false" mResGrpUri="peripheral/cpu/0/nvic/interrupt/*"/>
  <hwResources name="NVIC Node Error" URI="http://resources/4.1.8/app/UART/0/hwres_nvic_node_error" resourceGroupUri="" required="false" mResGrpUri="peripheral/cpu/0/nvic/interrupt/*"/>
  <hwResources name="DMA channel Tx" URI="http://resources/4.1.8/app/UART/0/hwres_dma_ch_tx" resourceGroupUri="peripheral/sv0/0/ch/*" solverVariable="true" mResGrpUri="peripheral/sv0/0/ch/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/gpdma0/gpdma0_0.dd#//@provided.4"/>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
  </hwResources>
  <hwResources name="DMA channel Rx" URI="http://resources/4.1.8/app/UART/0/hwres_dma_ch_rx" resourceGroupUri="resources/48.0.0/peripheral/dma8/0/ch/5" solverVariable="true" mResGrpUri="peripheral/sv0/0/ch/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/gpdma0/gpdma0_0.dd#//@provided.0"/>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
    <solverVarMap index="1">
      <value variableName="sv0" solverValue="dma8"/>
    </solverVarMap>
  </hwResources>
  <hwResources name="DLR channel Tx" URI="http://resources/4.1.8/app/UART/0/hwres_dlr_ch_tx" resourceGroupUri="peripheral/dlr/0/ch/*" mResGrpUri="peripheral/dlr/0/ch/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/dlr/dlr_0.dd#//@provided.6"/>
  </hwResources>
  <hwResources name="DLR channel Rx" URI="http://resources/4.1.8/app/UART/0/hwres_dlr_ch_rx" resourceGroupUri="resources/48.0.0/peripheral/dlr/0/ch/2" mResGrpUri="peripheral/dlr/0/ch/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/dlr/dlr_0.dd#//@provided.3"/>
  </hwResources>
  <hwResources name="Receive Pin" URI="http://resources/4.1.8/app/UART/0/__pin_hwres_port_pad_uart_rx" resourceGroupUri="devicepackage/0/108" constraintType="GLOBAL_RESOURCE" mResGrpUri="devicepackage/0/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/DEVICEPACKAGE/DEVICEPACKAGE_0.dd#//@provided.6"/>
  </hwResources>
  <hwResources name="Transmit Pin" URI="http://resources/4.1.8/app/UART/0/__pin_hwres_port_pad_uart_tx" resourceGroupUri="devicepackage/0/107" constraintType="GLOBAL_RESOURCE" mResGrpUri="devicepackage/0/*">
    <downwardMapList xsi:type="ResourceModel:ResourceGroup" href="../../../HW_RESOURCES/DEVICEPACKAGE/DEVICEPACKAGE_0.dd#//@provided.7"/>
  </hwResources>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_rxpad/http://resources/4.1.8/app/UART/0/vs_uart_dx0ins" systemDefined="true" sourceSignal="rxd_pin" targetSignal="dx0_input_stage" srcVirtualSignal="//@virtualSignals.3" targetVirtualSignal="//@virtualSignals.0"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_dout0/http://resources/4.1.8/app/UART/0/vs_uart_txpad" systemDefined="true" sourceSignal="dout0_output" targetSignal="txd_pin" srcVirtualSignal="//@virtualSignals.2" targetVirtualSignal="//@virtualSignals.4"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_txpad/http://resources/4.1.8/app/UART/0/vs_uart_dx0ins" systemDefined="true" sourceSignal="txd_pin" targetSignal="dx0_input_stage" required="false" srcVirtualSignal="//@virtualSignals.4" targetVirtualSignal="//@virtualSignals.0"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_dout0/http://resources/4.1.8/app/UART/0/vs_uart_dx0ins" systemDefined="true" sourceSignal="dout0_output" targetSignal="dx0_input_stage" required="false" srcVirtualSignal="//@virtualSignals.2" targetVirtualSignal="//@virtualSignals.0"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_std_tx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_tx" systemDefined="true" sourceSignal="event_transmit_buffer" targetSignal="sr_transmit" required="false" srcVirtualSignal="//@virtualSignals.5" targetVirtualSignal="//@virtualSignals.12"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_std_rx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_rx" systemDefined="true" sourceSignal="event_std_receive" targetSignal="sr_receive" required="false" srcVirtualSignal="//@virtualSignals.6" targetVirtualSignal="//@virtualSignals.13"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_alt_rx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_rx" systemDefined="true" sourceSignal="event_alt_receive" targetSignal="sr_receive" required="false" srcVirtualSignal="//@virtualSignals.7" targetVirtualSignal="//@virtualSignals.13"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_fifo_tx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_tx" systemDefined="true" sourceSignal="event_fifo_transmit" targetSignal="sr_transmit" required="false" srcVirtualSignal="//@virtualSignals.9" targetVirtualSignal="//@virtualSignals.12"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_fifo_rx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_rx" systemDefined="true" sourceSignal="event_fifo_std_receive" targetSignal="sr_receive" required="false" srcVirtualSignal="//@virtualSignals.10" targetVirtualSignal="//@virtualSignals.13"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_fifo_alt_rx_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_rx" systemDefined="true" sourceSignal="event_fifo_alt_receive" targetSignal="sr_receive" required="false" srcVirtualSignal="//@virtualSignals.11" targetVirtualSignal="//@virtualSignals.13"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_proto_specfic_int/http://resources/4.1.8/app/UART/0/vs_interrupt_uart_error" systemDefined="true" sourceSignal="event_protocol" targetSignal="sr_protocol_events" required="false" srcVirtualSignal="//@virtualSignals.8" targetVirtualSignal="//@virtualSignals.14"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_std_tx_int/http://resources/4.1.8/app/UART/0/vs_dst_trigger_dlr_tx_in" systemDefined="true" sourceSignal="event_transmit_buffer" targetSignal="destination_trigger" srcVirtualSignal="//@virtualSignals.5" targetVirtualSignal="//@virtualSignals.16"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_std_rx_int/http://resources/4.1.8/app/UART/0/vs_src_trigger_dlr_rx_in" systemDefined="true" sourceSignal="event_std_receive" targetSignal="source_trigger" srcVirtualSignal="//@virtualSignals.6" targetVirtualSignal="//@virtualSignals.15"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_alt_rx_int/http://resources/4.1.8/app/UART/0/vs_src_trigger_dlr_rx_in" systemDefined="true" sourceSignal="event_alt_receive" targetSignal="source_trigger" srcVirtualSignal="//@virtualSignals.7" targetVirtualSignal="//@virtualSignals.15"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_dma_interrupt_tx/http://resources/4.1.8/app/UART/0/vsra_gpdma_interrupt" systemDefined="true" sourceSignal="DMA Transfer complete event" targetSignal="Interrupt signal" srcVirtualSignal="//@virtualSignals.19" containingProxySignal="true">
    <downwardMapList xsi:type="ResourceModel:VirtualSignal" href="../../GLOBAL_DMA/v4_0_8/GLOBAL_DMA_0.app#//@virtualSignals.0"/>
    <targetVirtualSignal href="../../GLOBAL_DMA/v4_0_8/GLOBAL_DMA_0.app#//@virtualSignals.0"/>
  </connections>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_dma_interrupt_rx/http://resources/4.1.8/app/UART/0/vsra_gpdma_interrupt" systemDefined="true" sourceSignal="DMA Transfer complete event" targetSignal="Interrupt signal" srcVirtualSignal="//@virtualSignals.20" proxyTargetVirtualSignalUri="http://resources/4.0.8/app/GLOBAL_DMA/0/interrupt_dma" containingProxySignal="true">
    <downwardMapList xsi:type="ResourceModel:VirtualSignal" href="../../GLOBAL_DMA/v4_0_8/GLOBAL_DMA_0.app#//@virtualSignals.0"/>
    <targetVirtualSignal href="../../GLOBAL_DMA/v4_0_8/GLOBAL_DMA_0.app#//@virtualSignals.0"/>
  </connections>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_dst_trigger_dlr_tx_out/http://resources/4.1.8/app/UART/0/vs_dma_ch_dst_req_tx" systemDefined="true" sourceSignal="dlr_out" targetSignal="Destination request" srcVirtualSignal="//@virtualSignals.18" targetVirtualSignal="//@virtualSignals.21"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_src_trigger_dlr_rx_out/http://resources/4.1.8/app/UART/0/vs_dma_ch_src_req_rx" systemDefined="true" sourceSignal="dlr_out" targetSignal="Source request" srcVirtualSignal="//@virtualSignals.17" targetVirtualSignal="//@virtualSignals.22"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_rxpad/http://resources/4.1.8/app/UART/0/__pin_vs_uart_rxpad" systemDefined="true" sourceSignal="rxd_pin" targetSignal="Receive Pin_signal" srcVirtualSignal="//@virtualSignals.3" targetVirtualSignal="//@virtualSignals.23"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/__pin_vs_uart_rxpad/http://resources/4.1.8/app/UART/0/vs_uart_rxpad" systemDefined="true" sourceSignal="Receive Pin_signal" targetSignal="rxd_pin" srcVirtualSignal="//@virtualSignals.23" targetVirtualSignal="//@virtualSignals.3"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/vs_uart_txpad/http://resources/4.1.8/app/UART/0/__pin_vs_uart_txpad" systemDefined="true" sourceSignal="txd_pin" targetSignal="Transmit Pin_signal" srcVirtualSignal="//@virtualSignals.4" targetVirtualSignal="//@virtualSignals.24"/>
  <connections URI="http://resources/4.1.8/app/UART/0/http://resources/4.1.8/app/UART/0/__pin_vs_uart_txpad/http://resources/4.1.8/app/UART/0/vs_uart_txpad" systemDefined="true" sourceSignal="Transmit Pin_signal" targetSignal="txd_pin" srcVirtualSignal="//@virtualSignals.24" targetVirtualSignal="//@virtualSignals.4"/>
</ResourceModel:App>
