using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOD100492443.Critters.AI;

namespace UOD100492443.Critters.Messages
{
	/// <summary>
	/// Represents a SCAN message.
	/// </summary>
	class ScanMessage : ITrackableMessage
	{
		/// <summary>
		/// The request number for this message.
		/// </summary>
		public int RequestID { get; private set; } = -1;

		/// <summary>
		/// The callback for this trackable message.
		/// </summary>
		public Action<ITrackableMessage> Callback { get => null; set { } }

		/// <summary>
		/// The header for this message.
		/// </summary>
		public string Header => "SCAN";

		/// <summary>
		/// HasSet containing all the detected objects by this message.
		/// </summary>
		private HashSet<object> DetectedObjects => new HashSet<object>();

		/// <summary>
		/// Contains all the objects detected in this message.
		/// </summary>
		public IReadOnlyCollection<object> Objects {
			get => DetectedObjects;
		}

		/// <summary>
		/// Turns this message into a string.
		/// </summary>
		/// <returns></returns>
		public string Compose()
		{
			throw new NotImplementedException();
		}

		public void FromString(string source)
		{
			throw new NotImplementedException();
		}
	}
}
