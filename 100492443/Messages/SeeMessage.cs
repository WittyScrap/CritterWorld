using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Represents the SEE message in an object
	/// oriented form.
	/// </summary>
	public class SeeMessage : SimpleMessage
	{
		/// <summary>
		/// Constructs 
		/// </summary>
		/// <param name="sourceMessage"></param>
		public SeeMessage(string sourceMessage) : base(sourceMessage, "\n\t")
		{ }
	}
}
