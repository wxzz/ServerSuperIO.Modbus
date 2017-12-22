using System.Diagnostics;
using System.IO;
using ServerSuperIO.Modbus.Transport;
using ServerSuperIO.Modbus.Message;

namespace ServerSuperIO.Modbus.Transport
{
	/// <summary>
	///     Transport for Serial protocols.
	///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal abstract class ModbusSerialTransport : ModbusTransport
    {
        private bool _checkFrame = true;

        public ModbusSerialTransport()
            : base()
        {
           
        }

        /// <summary>
        ///     Gets or sets a value indicating whether LRC/CRC frame checking is performed on messages.
        /// </summary>
        public bool CheckFrame
        {
            get { return _checkFrame; }
            set { _checkFrame = value; }
        }

        public override IModbusMessage CreateResponse<T>(byte[] frame)
        {
            IModbusMessage response = base.CreateResponse<T>(frame);

            // compare checksum
            if (CheckFrame && !ChecksumsMatch(response, frame))
            {
                string msg = $"Checksums failed to match {string.Join(", ", response.MessageFrame)} != {string.Join(", ", frame)}";
                throw new IOException(msg);
            }

            return response;
        }

        internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);
    }
}
