using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Represents a SCAN message request.
	/// </summary>
	class ScanRequest : TrackableRequest
	{
		public ScanRequest()
			: base ("SCAN")
		{ }
	}
}
