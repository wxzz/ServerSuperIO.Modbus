using ServerSuperIO.Modbus.Common;
using ServerSuperIO.Modbus.Message;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ServerSuperIO.Modbus.Transport
{
	/// <summary>
	///     Transport for Internet protocols.
	///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal class ModbusSocketTransport : ModbusTransport
    {
        private static readonly object _transactionIdLock = new object();
        private ushort _transactionId;

        public ModbusSocketTransport()
            : base()
        {

        }

        internal static byte[] GetMbapHeader(IModbusMessage message)
        {
            byte[] transactionId = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)message.TransactionId));
            byte[] length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(message.ProtocolDataUnit.Length + 1)));

            var stream = new MemoryStream(7);
            stream.Write(transactionId, 0, transactionId.Length);
            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.Write(length, 0, length.Length);
            stream.WriteByte(message.SlaveAddress);

            return stream.ToArray();
        }

		internal virtual ushort GetNewTransactionId()
		{
			lock (_transactionIdLock)
			{
				_transactionId = _transactionId == ushort.MaxValue ? (ushort)1 : ++_transactionId;
			}

			return _transactionId;
		}

		public ushort GetTransactionId(byte[] fullFrame)
		{
			byte[] mbapHeader = fullFrame.Slice(0, 6).ToArray();

			return (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));
		}

		public override byte[] GetResponseMessageFrame(byte[] fullFrame)
		{
			return fullFrame.Slice(6, fullFrame.Length - 6).ToArray();
		}

		public override IModbusMessage CreateResponse<T>(byte[] readBytes)
		{
			ushort transid = GetTransactionId(readBytes);

			byte[] frameBytes = GetResponseMessageFrame(readBytes);

			IModbusMessage response = base.CreateResponse<T>(frameBytes);

			response.TransactionId = transid;

			return response;
		}

		internal IModbusMessage CreateMessageAndInitializeTransactionId<T>(byte[] fullFrame)
            where T : IModbusMessage, new()
        {
            byte[] mbapHeader = fullFrame.Slice(0, 6).ToArray();
            byte[] messageFrame = fullFrame.Slice(6, fullFrame.Length - 6).ToArray();

            IModbusMessage response = CreateResponse<T>(messageFrame);
            response.TransactionId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

            return response;
        }

        public override byte[] BuildMessageFrame(IModbusMessage message)
        {
			message.TransactionId = GetNewTransactionId();

			byte[] header = GetMbapHeader(message);
            byte[] pdu = message.ProtocolDataUnit;
            var messageBody = new MemoryStream(header.Length + pdu.Length);

            messageBody.Write(header, 0, header.Length);
            messageBody.Write(pdu, 0, pdu.Length);

            return messageBody.ToArray();
        }

        public override void ValidateResponse(IModbusMessage request, IModbusMessage response)
        {
            if (request.TransactionId != response.TransactionId)
            {
                string msg = $"Response was not of expected transaction ID. Expected {request.TransactionId}, received {response.TransactionId}.";
                throw new IOException(msg);
            }
        }
    }
}
