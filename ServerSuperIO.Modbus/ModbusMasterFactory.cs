using ServerSuperIO.Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSuperIO.Modbus
{
	public static class ModbusMasterFactory
	{
		public static IModbusMaster CreateRtu()
		{
			return new ModbusMaster(new Transport.ModbusRtuTransport());
		}

		public static IModbusMaster CreateAscii()
		{
			return new ModbusMaster(new Transport.ModbusAsciiTransport());
		}

		public static IModbusMaster CreateSocket()
		{
			return new ModbusMaster(new Transport.ModbusSocketTransport());
		}
	}
}
