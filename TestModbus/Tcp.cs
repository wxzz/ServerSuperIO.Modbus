using ServerSuperIO.Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModbus.IO;

namespace TestModbus
{
	public class Tcp : TestMaster
	{
		public Tcp(IStreamResource sr) : base(sr, ModbusMasterFactory.CreateSocket())
		{
		}
	}
}
