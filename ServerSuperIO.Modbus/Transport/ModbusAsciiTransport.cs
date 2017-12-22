using System.Diagnostics;
using System.IO;
using System.Text;

using ServerSuperIO.Modbus.Message;
using ServerSuperIO.Modbus.Utility;

namespace ServerSuperIO.Modbus.Transport
{
	/// <summary>
	///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class ModbusAsciiTransport : ModbusSerialTransport
    {
        public ModbusAsciiTransport()
            : base()
        {
        }

        public override byte[] BuildMessageFrame(IModbusMessage message)
        {
            var msgFrame = message.MessageFrame;

            var msgFrameAscii = ModbusUtility.GetAsciiBytes(msgFrame);
            var lrcAscii = ModbusUtility.GetAsciiBytes(ModbusUtility.CalculateLrc(msgFrame));
            var nlAscii = Encoding.UTF8.GetBytes(Modbus.NewLine.ToCharArray());

            var frame = new MemoryStream(1 + msgFrameAscii.Length + lrcAscii.Length + nlAscii.Length);
            frame.WriteByte((byte)':');
            frame.Write(msgFrameAscii, 0, msgFrameAscii.Length);
            frame.Write(lrcAscii, 0, lrcAscii.Length);
            frame.Write(nlAscii, 0, nlAscii.Length);

            return frame.ToArray();
        }

        internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame)
        {
            return ModbusUtility.CalculateLrc(message.MessageFrame) == messageFrame[messageFrame.Length - 1];
        }

		public override IModbusMessage CreateResponse<T>(byte[] frame)
		{
			byte[] frameBytes = GetResponseMessageFrame(frame);

			return base.CreateResponse<T>(frameBytes);
		}

		public override byte[] GetResponseMessageFrame(byte[] revData)
		{
			string frameHex = System.Text.Encoding.ASCII.GetString(revData);

			frameHex = frameHex.Substring(1, frameHex.IndexOf(Modbus.NewLine) - 1);

			// convert hex to bytes
			byte[] frame = ModbusUtility.HexToBytes(frameHex);

			if (frame.Length < 3)
			{
				throw new IOException("Premature end of stream, message truncated.");
			}

			return frame;
		}
	}
}
