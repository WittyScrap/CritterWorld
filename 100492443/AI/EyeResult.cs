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
		/// The distance and retlative angle to the nearest food item.
		/// </summary>
		public Item NearestFood { get; set; }

		/// <summary>
		/// The distance and retlative angle to the nearest gift.
		/// </summary>
		public Item NearestGift { get; set; }

		/// <summary>
		/// The distance and retlative angle to the nearest threat (critter or bomb).
		/// </summary>
		public Item NearestThreat { get; set; }

		/// <summary>
		/// The distance and retlative angle to the nearest terrain item.
		/// </summary>
		public Item NearestTerrain { get; set; }

		/// <summary>
		/// The distance and retlative angle of the escape hatch.
		/// </summary>
		public Item EscapeHatch { get; set; }
	}
}
