using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;


namespace KomModule
{
    public class UartCommunicator : ICommunicator
    {
        private Sensordata _recData;
        private RegulationParams _regPara;
        private Action _dataArrived;
        private readonly SerialPort _uart;
        private bool _isInit;

        public bool IsInitialized()
        {
            return _isInit;
        }

        public event Action NewSensordata
        {
            add { _dataArrived += value; }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_dataArrived != null) _dataArrived -= value;
            }
        }
        public UartCommunicator(string portName)
        {
            if (portName == null) throw new ArgumentNullException("portName");
            _isInit = false;
            _uart = new SerialPort(portName);
            port_Init();
        }
        public UartCommunicator(SerialPort port)
	    {
            _isInit = false;
            _uart = port;
            port_Init();
        }
        ~UartCommunicator()
        {
            port_Deinit();
        }
        public bool SendParams()
        {
            var i = 0;
            //TODO: parse Parameter struct to intermediate byte[]
            var buf = Protoparser.RParatoByArr(_regPara);
            //TODO: parse intermediate byte[] to final byte[] for sending
            buf = Frameparser.EncapsuleFrame(buf);
            try
            {
                // ReSharper disable once UnusedVariable
                foreach (var elem in buf)
                {
                    _uart.Write(buf, i, 1);
                    i++;
                    System.Threading.Thread.Sleep(30);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new SystemException("Could not send Params! Error: " + e.Message);
            }
        }
        public bool SetParams(RegulationParams para)
        {
            if (para == null) return false;
            _regPara = para;

            return true;
        }

        public Sensordata GetData()
        {
            return _recData;
        }
        private void port_Init()
        {
            try
            {
                _uart.BaudRate = 9600;
                _uart.DataBits = 8;
                _uart.StopBits = StopBits.One;
                _uart.Handshake = Handshake.None;
                _uart.Parity = Parity.Even;
                _uart.DtrEnable = false;
                _uart.RtsEnable = false;
                _uart.DataReceived += uart_DataReceived;
                _uart.ReceivedBytesThreshold = 10;
                _uart.Open();
                _isInit = true;
            }
            catch(Exception e)
            {
                throw new SystemException("Uart init failed! Message: " + e.Message);
            }
        }
        private void port_Deinit()
        {
            try
            {
                _uart.Close();
                _isInit = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void uart_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //read serial port
            var spL = (SerialPort)sender;
            var SofDel = new byte[2];
            do{
                spL.Read(SofDel, 0, 2);
            }while(SofDel[0] != 0x55 || SofDel[1] != 0xD5);

            var inLength = new byte[1];

            //read Framelength
            spL.Read(inLength, 0, 1);
            //subtract length field, as it was already read
            if (inLength[0] == 0 || inLength[0] == 1)
            {
                var a = spL.BytesToRead;
            }
            inLength[0]--;
            while(spL.BytesToRead < inLength[0])
            { //wait for Message end
            }
            //read Frame
            var buf = new byte[inLength[0]+1];
            //write full framesize in first element
            buf[0] = (byte)(inLength[0] + 1);
            //read rest of frame
            spL.Read(buf, 1, buf[0]-1);
            //parse message
            buf = Frameparser.DecapsuleFrame(buf);
            _recData = Protoparser.ByArrtoSData(buf);

            if (_dataArrived != null)
            {
                _dataArrived.Invoke();
            }
        }
    }
}
