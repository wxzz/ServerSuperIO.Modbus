using ServerSuperIO.Modbus.Device;
using ServerSuperIO.Modbus.Message;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModbus.IO;

namespace TestModbus
{
    public abstract class TestMaster
	{
		private IStreamResource _sr;
		private IModbusMaster _modbusMaster;

		private byte slaveId = 1;
		private ushort startAddress = 0;
		private ushort numRegisters = 5;

		public TestMaster(IStreamResource sr, IModbusMaster modbusMaster)
		{
			_sr = sr;
			_modbusMaster = modbusMaster;
		}

		public void TestReadCoils()
		{
			IModbusMessage request;
			byte[] sendBytes = _modbusMaster.BuildReadCoilsCommand(slaveId, startAddress, numRegisters, out request);
			_sr.Write(sendBytes, 0, sendBytes.Length);

			byte[] readBuffer = new byte[1024];
			int num = _sr.Read(readBuffer, 0, readBuffer.Length);
			byte[] readBytes = new byte[num];
			Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

			bool[] response = _modbusMaster.GetReadCoilsResponse(readBytes, numRegisters, request);
		}

		public void TestReadInputs()
		{
			IModbusMessage request;
			byte[] sendBytes = _modbusMaster.BuildReadInputsCommand(slaveId, startAddress, numRegisters, out request);
			_sr.Write(sendBytes, 0, sendBytes.Length);

			byte[] readBuffer = new byte[1024];
			int num = _sr.Read(readBuffer, 0, readBuffer.Length);
			byte[] readBytes = new byte[num];
			Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

			bool[] response = _modbusMaster.GetReadInputsResponse(readBytes, numRegisters, request);
		}
		
		public void TestReadHoldingRegisters()
		{
			IModbusMessage request;
			byte[] sendBytes = _modbusMaster.BuildReadHoldingRegistersCommand(slaveId, startAddress, numRegisters, out request);
			_sr.Write(sendBytes, 0, sendBytes.Length);

			byte[] readBuffer = new byte[1024];
			int num = _sr.Read(readBuffer, 0, readBuffer.Length);
			byte[] readBytes = new byte[num];
			Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

			ushort[] response = _modbusMaster.GetReadHoldingRegistersResponse(readBytes, request);
		}

		public void TestReadInputRegisters()
		{
			IModbusMessage request;
			byte[] sendBytes = _modbusMaster.BuildReadInputRegistersCommand(slaveId, startAddress, numRegisters, out request);
			_sr.Write(sendBytes, 0, sendBytes.Length);

			byte[] readBuffer = new byte[1024];
			int num = _sr.Read(readBuffer, 0, readBuffer.Length);
			byte[] readBytes = new byte[num];
			Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

			ushort[] response = _modbusMaster.GetReadInputRegistersResponse(readBytes, request);
		}

		public void TestWriteSingleCoil()
		{
			try
			{
				IModbusMessage request;
				byte[] sendBytes = _modbusMaster.BuildWriteSingleCoilCommand(slaveId, startAddress, true, out request);
				_sr.Write(sendBytes, 0, sendBytes.Length);

				byte[] readBuffer = new byte[1024];
				int num = _sr.Read(readBuffer, 0, readBuffer.Length);
				byte[] readBytes = new byte[num];
				Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

				_modbusMaster.ValidateWriteSingleCoilResponse(readBytes, request);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void TestWriteSingleRegister()
		{
			try
			{
				IModbusMessage request;
				byte[] sendBytes = _modbusMaster.BuildWriteSingleRegisterCommand(slaveId, startAddress, 123, out request);
				_sr.Write(sendBytes, 0, sendBytes.Length);

				byte[] readBuffer = new byte[1024];
				int num = _sr.Read(readBuffer, 0, readBuffer.Length);
				byte[] readBytes = new byte[num];
				Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

				_modbusMaster.ValidateWriteSingleRegisterResponse(readBytes, request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void TestWriteMultipleCoils()
		{
			try
			{
				IModbusMessage request;
				byte[] sendBytes = _modbusMaster.BuildWriteMultipleCoilsCommand(slaveId, startAddress, new bool[] { true,true,true}, out request);
				_sr.Write(sendBytes, 0, sendBytes.Length);

				byte[] readBuffer = new byte[1024];
				int num = _sr.Read(readBuffer, 0, readBuffer.Length);
				byte[] readBytes = new byte[num];
				Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

				_modbusMaster.ValidateWriteMultipleCoilsResponse(readBytes, request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void TestWriteMultipleRegisters()
		{
			try
			{
				IModbusMessage request;
				byte[] sendBytes = _modbusMaster.BuildWriteMultipleRegistersCommand(slaveId, startAddress, new ushort[] { 1,2,3}, out request);
				_sr.Write(sendBytes, 0, sendBytes.Length);

				byte[] readBuffer = new byte[1024];
				int num = _sr.Read(readBuffer, 0, readBuffer.Length);
				byte[] readBytes = new byte[num];
				Buffer.BlockCopy(readBuffer, 0, readBytes, 0, num);

				_modbusMaster.ValidateWriteMultipleRegistersResponse(readBytes, request);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
