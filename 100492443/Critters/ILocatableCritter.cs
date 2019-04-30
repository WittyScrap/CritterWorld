using CritterRobots.AI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Critters
{
	/// <summary>
	/// Represets a critter whose location can be kept track of.
	/// </summary>
	public interface ILocatableCritter
	{
		/// <summary>
		/// The current velocity of the critter.
		/// </summary>
		Vector Velocity { get; }

		/// <summary>
		/// The current location of the critter.
		/// </summary>
		Point Location { get; }

		/// <summary>
		/// The detected level's map.
		/// </summary>
		Map DetectedMap { get; }
	}
}
