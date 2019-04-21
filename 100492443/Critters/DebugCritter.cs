using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterRobots.Messages;

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
		
		/// <summary>
		/// This is just a debug critter don't expect much.
		/// </summary>
		public override void LaunchUI()
		{
			throw new NotImplementedException();
		}
	}
}
