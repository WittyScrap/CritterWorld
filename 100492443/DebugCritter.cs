using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters.Controllers
{
	/// <summary>
	/// This critter is for use exclusively for debugging purposes.
	/// </summary>
	class DebugCritter : Critter
	{
		/// <summary>
		/// Creates a new testing critter.
		/// </summary>
		public DebugCritter() : base("~ The bug whoms't de-bugs ~")
		{ }
	}
}
