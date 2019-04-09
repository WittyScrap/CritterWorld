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
	interface IRequest
	{
		/// <summary>
		/// The message to be sent.
		/// </summary>
		string Message { get; }

		/// <summary>
		/// Submits the message through the provided
		/// sender object.
		/// </summary>
		/// <param name="sender">The sender delegate object.</param>
		void Submit(Send sender);
	}
}
