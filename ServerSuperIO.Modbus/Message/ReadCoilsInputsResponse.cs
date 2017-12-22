using System;
using System.Linq;
using ServerSuperIO.Modbus.Data;
using ServerSuperIO.Modbus.Common;

namespace ServerSuperIO.Modbus.Message
{
	internal class ReadCoilsInputsResponse : AbstractModbusMessageWithData<DiscreteCollection>
    {
        public ReadCoilsInputsResponse()
        {
        }

        public ReadCoilsInputsResponse(byte functionCode, byte slaveAddress, byte byteCount, DiscreteCollection data)
            : base(slaveAddress, functionCode)
        {
            ByteCount = byteCount;
            Data = data;
        }

        public byte ByteCount
        {
            get { return MessageImpl.ByteCount.Value; }
            set { MessageImpl.ByteCount = value; }
        }

        public override int MinimumFrameSize
        {
            get { return 3; }
        }

        public override string ToString()
        {
            string msg = $"Read {Data.Count()} {(FunctionCode == Modbus.ReadInputs ? "inputs" : "coils")} - {Data}.";
            return msg;
        }

        protected override void InitializeUnique(byte[] frame)
        {
            if (frame.Length < 3 + frame[2])
            {
                throw new FormatException("Message frame data segment does not contain enough bytes.");
            }

            ByteCount = frame[2];
            Data = new DiscreteCollection(frame.Slice(3, ByteCount).ToArray());
        }
    }
}
