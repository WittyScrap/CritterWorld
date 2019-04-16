using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOD100492443.Critters.AI;

namespace UOD100492443.Critters.Messages
{
	/// <summary>
	/// Defines an error message recieved by the environment.
	/// </summary>
	class ErrorMessage : ISimpleMessage
	{
		/// <summary>
		/// Header for error messages.
		/// </summary>
		public string Header => "ERROR";

		/// <summary>
		/// The contents of the error message.
		/// </summary>
		public string Contents { get; set; } = "Undefined error.";

		/// <summary>
		/// Composes this error message into a string.
		/// </summary>
		/// <returns></returns>
		public string Compose()
		{
			return Header + ":" + Contents;
		}

		/// <summary>
		/// Parses the contents of a compatible error
		/// string and arranges them into this object.
		/// </summary>
		/// <param name="source">The source error string.</param>
		public void FromString(string source)
		{
			string[] components = source.Split(':');
			Contents = components[1];
		}
	}
}
