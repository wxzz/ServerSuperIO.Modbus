using ServerSuperIO.Modbus.Message;
using ServerSuperIO.Modbus.Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ServerSuperIO.Modbus.Transport
{
	/// <summary>
	///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class ModbusRtuTransport : ModbusSerialTransport
    {
        public const int RequestFrameStartLength = 7;

        public const int ResponseFrameStartLength = 4;

        public ModbusRtuTransport()
            : base()
        {
         
        }

        public static int RequestBytesToRead(byte[] frameStart)
        {
            byte functionCode = frameStart[1];
            int numBytes;

            switch (functionCode)
            {
                case Modbus.ReadCoils:
                case Modbus.ReadInputs:
                case Modbus.ReadHoldingRegisters:
                case Modbus.ReadInputRegisters:
                case Modbus.WriteSingleCoil:
                case Modbus.WriteSingleRegister:
                case Modbus.Diagnostics:
                    numBytes = 1;
                    break;
                case Modbus.WriteMultipleCoils:
                case Modbus.WriteMultipleRegisters:
                    byte byteCount = frameStart[6];
                    numBytes = byteCount + 2;
                    break;
                default:
                    string msg = $"Function code {functionCode} not supported.";
                    Debug.WriteLine(msg);
                    throw new NotImplementedException(msg);
            }

            return numBytes;
        }

        public static int ResponseBytesToRead(byte[] frameStart)
        {
            byte functionCode = frameStart[1];

            // exception response
            if (functionCode > Modbus.ExceptionOffset)
            {
                return 1;
            }

            int numBytes;
            switch (functionCode)
            {
                case Modbus.ReadCoils:
                case Modbus.ReadInputs:
                case Modbus.ReadHoldingRegisters:
                case Modbus.ReadInputRegisters:
                    numBytes = frameStart[2] + 1;
                    break;
                case Modbus.WriteSingleCoil:
                case Modbus.WriteSingleRegister:
                case Modbus.WriteMultipleCoils:
                case Modbus.WriteMultipleRegisters:
                case Modbus.Diagnostics:
                    numBytes = 4;
                    break;
                default:
                    string msg = $"Function code {functionCode} not supported.";
                    Debug.WriteLine(msg);
                    throw new NotImplementedException(msg);
            }

            return numBytes;
        }

        public override byte[] BuildMessageFrame(IModbusMessage message)
        {
            var messageFrame = message.MessageFrame;
            var crc = ModbusUtility.CalculateCrc(messageFrame);
            var messageBody = new MemoryStream(messageFrame.Length + crc.Length);

            messageBody.Write(messageFrame, 0, messageFrame.Length);
            messageBody.Write(crc, 0, crc.Length);

            return messageBody.ToArray();
        }

		public override byte[] GetResponseMessageFrame(byte[] readBytes)
		{
			return readBytes;
		}

		internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
        {
            return BitConverter.ToUInt16(messageFrame, messageFrame.Length - 2) ==
                BitConverter.ToUInt16(ModbusUtility.CalculateCrc(message.MessageFrame), 0);
        }
    }
}
