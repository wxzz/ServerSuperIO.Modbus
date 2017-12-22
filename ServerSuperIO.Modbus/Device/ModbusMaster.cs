using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerSuperIO.Modbus.Data;
using ServerSuperIO.Modbus.Message;
using ServerSuperIO.Modbus.Transport;

namespace ServerSuperIO.Modbus.Device
{
	internal class ModbusMaster : IModbusMaster
	{
		private ModbusTransport Transport { set; get; }

		public ModbusMaster(ModbusTransport transport)
		{
			Transport = transport;
		}

		public byte[] BuildReadCoilsCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request)
		{
			ValidateNumberOfPoints("numberOfPoints", numberOfPoints, 2000);

			request = new ReadCoilsInputsRequest(Modbus.ReadCoils, slaveAddress, startAddress, numberOfPoints);
			return Transport.BuildMessageFrame(request);
		}

		public bool[] GetReadCoilsResponse(byte[] readBytes,int numberOfPoints, IModbusMessage request)
		{
			ReadCoilsInputsResponse response =(ReadCoilsInputsResponse)Transport.CreateResponse<ReadCoilsInputsResponse>(readBytes);

			Transport.ValidateResponse(request, response);

			return response.Data.Take(numberOfPoints).ToArray();
		}

		public byte[] BuildReadInputsCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request)
		{
			ValidateNumberOfPoints("numberOfPoints", numberOfPoints, 2000);

			request = new ReadCoilsInputsRequest(Modbus.ReadInputs, slaveAddress, startAddress, numberOfPoints);
			return Transport.BuildMessageFrame(request);
		}

		public bool[] GetReadInputsResponse(byte[] readBytes, int numberOfPoints, IModbusMessage request)
		{
			ReadCoilsInputsResponse response = (ReadCoilsInputsResponse)Transport.CreateResponse<ReadCoilsInputsResponse>(readBytes);

			Transport.ValidateResponse(request, response);

			return response.Data.Take(numberOfPoints).ToArray();
		}

		public byte[] BuildReadHoldingRegistersCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request)
		{
			ValidateNumberOfPoints("numberOfPoints", numberOfPoints, 125);

			request = new ReadHoldingInputRegistersRequest(Modbus.ReadHoldingRegisters, slaveAddress, startAddress, numberOfPoints);

			return Transport.BuildMessageFrame(request);
		}

		public ushort[] GetReadHoldingRegistersResponse(byte[] readBytes, IModbusMessage request)
		{
			ReadHoldingInputRegistersResponse response = (ReadHoldingInputRegistersResponse)Transport.CreateResponse<ReadHoldingInputRegistersResponse>(readBytes);

			Transport.ValidateResponse(request, response);

			return response.Data.ToArray();
		}

		public byte[] BuildReadInputRegistersCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request)
		{
			ValidateNumberOfPoints("numberOfPoints", numberOfPoints, 125);

			request = new ReadHoldingInputRegistersRequest(Modbus.ReadInputRegisters, slaveAddress, startAddress, numberOfPoints);
			return Transport.BuildMessageFrame(request);
		}

		public ushort[] GetReadInputRegistersResponse(byte[] readBytes, IModbusMessage request)
		{
			ReadHoldingInputRegistersResponse response = (ReadHoldingInputRegistersResponse)Transport.CreateResponse<ReadHoldingInputRegistersResponse>(readBytes);

			Transport.ValidateResponse(request, response);

			return response.Data.ToArray();
		}

		public byte[] BuildWriteSingleCoilCommand(byte slaveAddress, ushort coilAddress, bool value,out IModbusMessage request)
		{
			request = new WriteSingleCoilRequestResponse(slaveAddress, coilAddress, value);
			return Transport.BuildMessageFrame(request);
		}

		public void ValidateWriteSingleCoilResponse(byte[] readBytes,IModbusMessage request)
		{
			WriteSingleCoilRequestResponse response = (WriteSingleCoilRequestResponse)Transport.CreateResponse<WriteSingleCoilRequestResponse>(readBytes);

			Transport.ValidateResponse(request, response);
		}

		public byte[] BuildWriteSingleRegisterCommand(byte slaveAddress, ushort registerAddress, ushort value, out IModbusMessage request)
		{
			request = new WriteSingleRegisterRequestResponse(
				slaveAddress,
				registerAddress,
				value);
			return Transport.BuildMessageFrame(request);
		}

		public void ValidateWriteSingleRegisterResponse(byte[] readBytes, IModbusMessage request)
		{
			WriteSingleRegisterRequestResponse response = (WriteSingleRegisterRequestResponse)Transport.CreateResponse<WriteSingleRegisterRequestResponse>(readBytes);

			Transport.ValidateResponse(request, response);
		}

		public byte[] BuildWriteMultipleCoilsCommand(byte slaveAddress, ushort startAddress, bool[] data,out IModbusMessage request)
		{
			ValidateData("data", data, 1968);

			request = new WriteMultipleCoilsRequest(
				slaveAddress,
				startAddress,
				new DiscreteCollection(data));

			return Transport.BuildMessageFrame(request);
		}

		public void ValidateWriteMultipleCoilsResponse(byte[] readBytes,IModbusMessage request)
		{
			WriteMultipleCoilsResponse response = (WriteMultipleCoilsResponse)Transport.CreateResponse<WriteMultipleCoilsResponse>(readBytes);

			Transport.ValidateResponse(request, response);
		}

		public byte[] BuildWriteMultipleRegistersCommand(byte slaveAddress, ushort startAddress, ushort[] data,out IModbusMessage request)
		{
			ValidateData("data", data, 123);

			request = new WriteMultipleRegistersRequest(
				slaveAddress,
				startAddress,
				new RegisterCollection(data));

			return Transport.BuildMessageFrame(request);
		}

		public void ValidateWriteMultipleRegistersResponse(byte[] readBytes,IModbusMessage request)
		{
			WriteMultipleRegistersResponse response = (WriteMultipleRegistersResponse)Transport.CreateResponse<WriteMultipleRegistersResponse>(readBytes);

			Transport.ValidateResponse(request, response);
		}

		public void Dispose()
		{
			Transport.Dispose();
		}

		private static void ValidateNumberOfPoints(string argumentName, ushort numberOfPoints, ushort maxNumberOfPoints)
		{
			if (numberOfPoints < 1 || numberOfPoints > maxNumberOfPoints)
			{
				string msg = $"Argument {argumentName} must be between 1 and {maxNumberOfPoints} inclusive.";
				throw new ArgumentException(msg);
			}
		}

		private void ValidateData<T>(string argumentName, T[] data, int maxDataLength)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (data.Length == 0 || data.Length > maxDataLength)
			{
				string msg = $"The length of argument {argumentName} must be between 1 and {maxDataLength} inclusive.";
				throw new ArgumentException(msg);
			}
		}
	}
}
