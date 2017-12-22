using System;
using ServerSuperIO.Modbus.Message;

namespace ServerSuperIO.Modbus.Transport
{
	internal class EmptyTransport : ModbusTransport
    {
		public override byte[] BuildMessageFrame(Message.IModbusMessage message)
        {
            throw new NotImplementedException();
        }

		public override byte[] GetResponseMessageFrame(byte[] readBytes)
		{
			throw new NotImplementedException();
		}
	}
}
