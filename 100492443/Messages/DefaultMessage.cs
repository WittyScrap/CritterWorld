using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOD100492443.Critters.AI;

namespace UOD100492443.Critters.Messages
{
	/// <summary>
	/// Default message for any type that
	/// does not require any additional message
	/// details.
	/// </summary>
	class DefaultMessage : ISimpleMessage
	{
		/// <summary>
		/// No defined header.
		/// </summary>
		public string Header => "!DEFAULT!";

		/// <summary>
		/// No defined message.
		/// </summary>
		/// <returns></returns>
		public string Compose()
		{
			return string.Empty;
		}

		/// <summary>
		/// No defined object.
		/// </summary>
		/// <param name="source"></param>
		public void FromString(string source)
		{ }
	}
}
