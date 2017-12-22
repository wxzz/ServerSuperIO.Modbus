using ServerSuperIO.Modbus.Message;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
namespace ServerSuperIO.Modbus.Transport
{
	/// <summary>
	/// Modbus transport.
	/// Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
	/// </summary>
	internal abstract class ModbusTransport : IDisposable
    {
		/// <summary>
		///     This constructor is called by the NullTransport.
		/// </summary>
		public ModbusTransport()
        {
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IModbusMessage CreateResponse<T>(byte[] frame)
            where T : IModbusMessage, new()
        {
            byte functionCode = frame[1];
            IModbusMessage response;

            // check for slave exception response else create message from frame
            if (functionCode > Modbus.ExceptionOffset)
            {
                response = ModbusMessageFactory.CreateModbusMessage<SlaveExceptionResponse>(frame);
            }
            else
            {
                response = ModbusMessageFactory.CreateModbusMessage<T>(frame);
            }

            return response;
        }

		public abstract byte[] GetResponseMessageFrame(byte[] readBytes);

		public virtual void ValidateResponse(IModbusMessage request, IModbusMessage response)
        {
            // always check the function code and slave address, regardless of transport protocol
            if (request.FunctionCode != response.FunctionCode)
            {
                string msg = $"Received response with unexpected Function Code. Expected {request.FunctionCode}, received {response.FunctionCode}.";
                throw new IOException(msg);
            }

            if (request.SlaveAddress != response.SlaveAddress)
            {
                string msg = $"Response slave address does not match request. Expected {response.SlaveAddress}, received {request.SlaveAddress}.";
                throw new IOException(msg);
            }

            // message specific validation
            var req = request as IModbusRequest;

            if (req != null)
            {
                req.ValidateResponse(response);
            }
        }

		public abstract byte[] BuildMessageFrame(IModbusMessage message);

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
