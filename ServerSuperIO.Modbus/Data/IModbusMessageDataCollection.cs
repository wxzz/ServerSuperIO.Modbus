using System.Diagnostics.CodeAnalysis;
namespace ServerSuperIO.Modbus.Data
{


	/// <summary>
	///     Modbus message containing data.
	/// </summary>
	internal interface IModbusMessageDataCollection
    {
        /// <summary>
        ///     Gets the network bytes.
        /// </summary>
        byte[] NetworkBytes { get; }

        /// <summary>
        ///     Gets the byte count.
        /// </summary>
        byte ByteCount { get; }
    }
}
