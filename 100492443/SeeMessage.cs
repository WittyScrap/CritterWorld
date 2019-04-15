using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a simple message without reference IDs.
	/// </summary>
	class SeeMessage : ISimpleMessage
	{
		/// <summary>
		/// Defines the header of this message to be SEE.
		/// </summary>
		public string Header => "SEE";

		public string Compose()
		{
			throw new NotImplementedException();
		}

		public bool FromString(string source, out IMessage message)
		{
			throw new NotImplementedException();
		}
	}
}
