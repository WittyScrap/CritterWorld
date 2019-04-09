using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Requests the size of the current playing area.
	/// </summary>
	class ArenaSizeRequest : TrackableRequest
	{
		public ArenaSizeRequest()
			: base ("GET_ARENA_SIZE")
		{ }
	}
}
