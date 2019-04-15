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
		string Header { get; }

		/// <summary>
		/// Creates a string equivalent of this message
		/// to be sent to the environment.
		/// </summary>
		/// <returns>A string equivalent of this message.</returns>
		string Compose();

		/// <summary>
		/// Offers the same behaviour as <see cref="Compose"/>.
		/// </summary>
		/// <returns>A string equivalent of this message.</returns>
		string ToString();

		/// <summary>
		/// Parses a string into an instance of this message object.
		/// </summary>
		/// <param name="source">The formatted parsable string.</param>
		/// <returns>Message equivalent of the string.</returns>
		void FromString(string source);
	}
}
