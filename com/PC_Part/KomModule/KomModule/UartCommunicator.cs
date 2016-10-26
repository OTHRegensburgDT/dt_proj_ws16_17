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
        private bool isInit;

        public bool isInitialized()
        {
            return isInit;
        }

        public event Action newSensordata
        {
            add { _dataArrived += value; }
            remove {_dataArrived -= value;}
        }
        public UartCommunicator(String portName)
        {
            isInit = false;
            uart = new SerialPort(portName);
            port_Init();
        }
        public UartCommunicator(SerialPort port)
	    {
            isInit = false;
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
            byte[] buf = new byte[1024];
            //TODO: parse Parameter struct to intermediate byte[]

            //TODO: parse intermediate byte[] to final byte[] for sending
            try
            {
                uart.Write(buf, 0, buf.Length);
                retVal = true;
            }
            catch (Exception e)
            {
                throw new SystemException("Could not send Params! Error: " + e.ToString());
            }
            return retVal;
        }
        public bool SetParams(RegulationParams para)
        {
            bool retVal;
            retVal = false;

            if (para != null)
            {
                retVal = true;
                regPara = para;
            }

            return retVal;
        }

        public Sensordata getData()
        {
            return recData;
        }
        private void port_Init()
        {
            try
            {
                uart.BaudRate = 19200;
                uart.DataBits = 8;
                uart.StopBits = StopBits.One;
                uart.Handshake = Handshake.None;
                uart.Parity = Parity.Even;
                uart.DtrEnable = false;
                uart.RtsEnable = false;
                uart.DataReceived += uart_DataReceived;
                uart.Open();
                isInit = true;
            }
            catch(Exception e)
            {
                throw new SystemException("Uart init failed! Message: "+ e.ToString());
            }
        }
        private void port_Deinit()
        {
            try
            {
                uart.Close();
                isInit = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void uart_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //read serial port
            SerialPort spL = (SerialPort)sender;
            const int bufSize = 1024;
            byte[] buf = new byte[bufSize];
            spL.Read(buf, 0, bufSize);
            //parse message
            buf = Frameparser.DecapsuleFrame(buf);
            recData = Protoparser.ByArrtoSData(buf);

            _dataArrived();
        }
    }
}
