using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents the base request interface.
	/// </summary>
	interface IMessage
	{
		/// <summary>
		/// The message to be sent.
		/// </summary>
		string Message { get; }
	}
}
