using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// The results of a look call.
	/// </summary>
	public struct EyeResult
	{
		/// <summary>
		/// The distance and retlative angle to the nearest gift or food item.
		/// </summary>
		public Item NearestCollectable { get; set; }

		/// <summary>
		/// The distance and retlative angle to the nearest threat (critter, bomb or terrain).
		/// </summary>
		public Item NearestThreat { get; set; }

		/// <summary>
		/// The distance and retlative angle of the escape hatch.
		/// </summary>
		public Item EscapeHatch { get; set; }
	}
}
