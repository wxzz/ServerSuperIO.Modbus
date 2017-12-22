using ServerSuperIO.Modbus;
using ServerSuperIO.Modbus.Device;
using ServerSuperIO.Modbus.Message;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModbus.IO;

namespace TestModbus
{
	public class Rtu:TestMaster
	{
		public Rtu(IStreamResource sr) :base(sr, ModbusMasterFactory.CreateRtu())
		{
		}
	}
}
