using ServerSuperIO.Modbus.Message;
using ServerSuperIO.Modbus.Transport;
using System;
using System.Threading.Tasks;

namespace ServerSuperIO.Modbus.Device
{
    /// <summary>
    ///     Modbus master device.
    /// </summary>
    public interface IModbusMaster : IDisposable
    {

        byte[] BuildReadCoilsCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints,out IModbusMessage request);

		bool[] GetReadCoilsResponse(byte[] readBytes, int numberOfPoints,IModbusMessage request);

		byte[] BuildReadInputsCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request);

		bool[] GetReadInputsResponse(byte[] readBytes, int numberOfPoints, IModbusMessage request);

        byte[] BuildReadHoldingRegistersCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request);

		ushort[] GetReadHoldingRegistersResponse(byte[] readBytes, IModbusMessage request);

        byte[] BuildReadInputRegistersCommand(byte slaveAddress, ushort startAddress, ushort numberOfPoints, out IModbusMessage request);

		ushort[] GetReadInputRegistersResponse(byte[] readBytes, IModbusMessage request);

		byte[] BuildWriteSingleCoilCommand(byte slaveAddress, ushort coilAddress, bool value,out IModbusMessage request);

		void ValidateWriteSingleCoilResponse(byte[] readBytes,IModbusMessage request);

		byte[] BuildWriteSingleRegisterCommand(byte slaveAddress, ushort registerAddress, ushort value, out IModbusMessage request);

		void ValidateWriteSingleRegisterResponse(byte[] readBytes,IModbusMessage request);

		byte[] BuildWriteMultipleRegistersCommand(byte slaveAddress, ushort startAddress, ushort[] data,out IModbusMessage request);

		void ValidateWriteMultipleRegistersResponse(byte[] readBytes,IModbusMessage request);

		/// <summary>
		///    Writes a sequence of coils.
		/// </summary>
		/// <param name="slaveAddress">Address of the device to write to.</param>
		/// <param name="startAddress">Address to begin writing values.</param>
		/// <param name="data">Values to write.</param>
		/// <param name="request">Values to write.</param>
		byte[] BuildWriteMultipleCoilsCommand(byte slaveAddress, ushort startAddress, bool[] data, out IModbusMessage request);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="readBytes"></param>
		/// <returns></returns>
		void ValidateWriteMultipleCoilsResponse(byte[] readBytes, IModbusMessage request);
	}
}
