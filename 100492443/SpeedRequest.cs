using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Represents a request to obtain the current critter's speed.
	/// </summary>
	class SpeedRequest : TrackableRequest
	{
		public SpeedRequest()
			: base("GET_SPEED")
		{ }
	}
}
