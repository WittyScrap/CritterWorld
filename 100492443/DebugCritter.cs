using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters.AI
{
	/// <summary>
	/// This critter is for use exclusively for debugging purposes.
	/// </summary>
	class DebugCritter : Critter
	{
		/// <summary>
		/// Creates a new testing critter.
		/// </summary>
		public DebugCritter() : base("~ The bug whomv'st debugs ~")
		{ }

		/// <summary>
		/// This method will call when the- ugh forget it it's 2am
		/// and this is just a testing thing it won't go anywhere.
		/// </summary>
		protected override void OnSee(string[] stuff)
		{
			Debugger.LogMessage("Initialized!");
			Responder("RANDOM_DESTINATION");
		}
	}
}
