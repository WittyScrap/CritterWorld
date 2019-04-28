using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents an object that can be fully disconnected.
	/// </summary>
	public interface IDisconnectable
	{
		/// <summary>
		/// Clears all connections.
		/// </summary>
		void Disconnect();
	}
}
