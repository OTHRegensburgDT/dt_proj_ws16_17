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
        private const short SENSORDATALENGTH = 43;
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
            int i = 0;
            retVal = false;
            byte[] buf = new byte[1024];
            //TODO: parse Parameter struct to intermediate byte[]
            buf = Protoparser.RParatoByArr(regPara);
            //TODO: parse intermediate byte[] to final byte[] for sending
            buf = Frameparser.EncapsuleFrame(buf);
            try
            {
                foreach (byte elem in buf)
                {
                    uart.Write(buf, i, 1);
                    i++;
                    System.Threading.Thread.Sleep(30);
                }
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
                regPara = para;
                retVal = true;
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
                uart.BaudRate = 9600;
                uart.DataBits = 8;
                uart.StopBits = StopBits.One;
                uart.Handshake = Handshake.None;
                uart.Parity = Parity.Even;
                uart.DtrEnable = false;
                uart.RtsEnable = false;
                uart.DataReceived += uart_DataReceived;
                uart.ReceivedBytesThreshold = 10;
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
            byte[] inLength = new byte[1];
            byte[] buf;
            Task.Delay(100);
            //read Framelength
            spL.Read(inLength, 0, 1);
            //subtract length field, as it was already read
            inLength[0]--;
            while(spL.BytesToRead < inLength[0])
            { //wait for Message end
            }
            //read Frame
            buf = new byte[inLength[0]+1];
            //write full framesize in first element
            buf[0] = (byte)(inLength[0] + 1);
            //read rest of frame
            spL.Read(buf, 1, buf[0]-1);
                
            //parse message
            buf = Frameparser.DecapsuleFrame(buf);
            recData = Protoparser.ByArrtoSData(buf);

            _dataArrived();
        }
    }
}
