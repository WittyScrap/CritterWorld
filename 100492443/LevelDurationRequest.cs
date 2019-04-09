using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Requests the amount of time elapsed since the
	/// beginning of the level.
	/// </summary>
	class LevelDurationRequest : TrackableRequest
	{
		public LevelDurationRequest()
			: base("GET_LEVEL_DURATION")
		{ }
	}
}
