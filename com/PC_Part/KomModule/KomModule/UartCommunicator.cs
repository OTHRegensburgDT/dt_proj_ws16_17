using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace KomModule
{
    public class UartCommunicator : ICommunicator
    {
        private Sensordata recData;
        private RegulationParams regPara;
        private Action _dataArrived;
        private SerialPort uart;

        public event Action newSensordata
        {
            add { _dataArrived += value; }
            remove {_dataArrived -= value;}
        }
        public UartCommunicator(String portName)
        {
            uart = new SerialPort(portName);
            port_Init();
        }
        public UartCommunicator(SerialPort port)
	    {
            uart = port;
            port_Init();
        }
        ~UartCommunicator()
        {
            port_Deinit();
        }
        public bool SendParams()
        {
            bool retVal;
            retVal = false;

            return retVal;
        }

        public Sensordata getData()
        {
            return recData;
        }

        private void port_DataRcv(object sender, SerialDataReceivedEventArgs e)
        {
            String InputData;
            InputData = uart.ReadExisting();
            if (InputData != String.Empty)
            {
                recData = Parser.StrtoSData(InputData);
                new Task(_dataArrived).Start();
            }
        }

        private void port_Init()
        {
            if (uart.IsOpen == false)
            {
                uart.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataRcv);
                uart.BaudRate = 9600;
                uart.DataBits = 8;
                uart.StopBits = StopBits.One;
                uart.Handshake = Handshake.None;
                uart.Parity = Parity.Even;
                uart.Open();
            }
        }

        private void port_Deinit()
        {
            if (uart.IsOpen)
            {
                uart.Close();
            }
            else
            {
                throw new SystemException("UART is closed???");
            }
        }
    }
}
