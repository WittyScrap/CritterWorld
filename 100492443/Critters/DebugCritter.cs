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
		/// This method is called once the critter has been initialized.
		/// </summary>
		protected override void OnInitialize()
		{
			SendMessage(DefaultMessages.RandomDestination);
		}

		/// <summary>
		/// This method is called when the destination has been reached.
		/// </summary>
		/// <param name="location">The current critter's location.</param>
		protected override void OnDestinationReached(Point location)
		{
			SendMessage(DefaultMessages.RandomDestination);
		}
	}
}
