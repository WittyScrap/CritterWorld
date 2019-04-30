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
	struct EyeResult
	{
		/// <summary>
		/// The distance to the nearest food item.
		/// </summary>
		public double NearestFood { get; set; }

		/// <summary>
		/// The distance to the nearest gift.
		/// </summary>
		public double NearestGift { get; set; }

		/// <summary>
		/// The distance to the nearest threat (critter or bomb).
		/// </summary>
		public double NearestThreat { get; set; }

		/// <summary>
		/// The distance to either the nearest terrain item or the
		/// map boundary.
		/// </summary>
		public double NearestTerrain { get; set; }
	}
}
