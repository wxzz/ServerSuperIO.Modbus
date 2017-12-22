using ServerSuperIO.Modbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TestModbus.IO;
using TestModbus.Serial;

namespace TestModbus
{
	class Program
	{
		static void Main(string[] args)
		{
			//TestRtu();

			//TestAscii();

			//TestSocket();
		}

		private static void TestRtu()
		{
			using (SerialPortAdapter port = new SerialPortAdapter(GetSerialPort()))
			{
				Rtu rtu = new Rtu(port);

				//rtu.TestReadCoils();

				//rtu.TestWriteSingleCoil();

				//rtu.TestWriteMultipleCoils();

				//rtu.TestReadHoldingRegisters();

				//rtu.TestWriteSingleRegister();

				//rtu.TestWriteMultipleRegisters();

				//rtu.TestReadInputs();

				//rtu.TestReadInputRegisters();
			}
		}

		private static void TestAscii()
		{
			using (SerialPortAdapter port = new SerialPortAdapter(GetSerialPort()))
			{
				Ascii ascii = new Ascii(port);

				//ascii.TestReadCoils();

				//ascii.TestWriteSingleCoil();

				//ascii.TestWriteMultipleCoils();

				//ascii.TestReadHoldingRegisters();

				//ascii.TestWriteSingleRegister();

				//ascii.TestWriteMultipleRegisters();

				//ascii.TestReadInputs();

				//ascii.TestReadInputRegisters();
			}
		}

		private static void TestSocket()
		{
			using (TcpClientAdapter port = new TcpClientAdapter(GetTcpClient()))
			{
				Tcp tcp = new Tcp(port);

				//tcp.TestReadCoils();

				//tcp.TestWriteSingleCoil();

				//tcp.TestWriteMultipleCoils();

				tcp.TestReadHoldingRegisters();

				tcp.TestWriteSingleRegister();

				tcp.TestWriteMultipleRegisters();

				//tcp.TestReadInputs();

				//tcp.TestReadInputRegisters();
			}
		}


		private static SerialPort GetSerialPort()
		{
			SerialPort port = new SerialPort("COM1");
			// configure serial port
			port.BaudRate = 9600;
			port.DataBits = 8;
			port.Parity = Parity.Even;
			port.StopBits = StopBits.One;
			port.Open();
			port.ReadTimeout = 5000;
			port.WriteTimeout = 1000;
			return port;
		}

		private static TcpClient GetTcpClient()
		{
			TcpClient client = new TcpClient("127.0.0.1", 502);
			client.SendTimeout = 1000;
			client.ReceiveTimeout = 1000;
			return client;
		}
	}
}
