using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Represents a class that can be serialized to a string.
	/// </summary>
	interface ISerializable
	{
		/// <summary>
		/// Serializes this class 
		/// </summary>
		/// <returns></returns>
		string Serialize();
	}
}
