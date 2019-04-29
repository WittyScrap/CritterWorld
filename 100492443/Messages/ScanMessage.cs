using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterRobots.AI;
using CritterRobots.Critters;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Represents the SCAN message in an object
	/// oriented form.
	/// </summary>
	public class ScanMessage : SeeMessage, ITrackedMessage
	{
		/// <summary>
		/// This message's request ID.
		/// </summary>
		public int RequestID { get; }

		/// <summary>
		/// Constructs a new SCAN message.
		/// </summary>
		/// <param name="sourceMessage">The source formatted message string.</param>
		public ScanMessage(string sourceMessage) : base(sourceMessage)
		{
			RequestID = GetInteger(0);
		}

		/// <summary>
		/// Iteratively returns all relevant detected entities.
		/// </summary>
		public override IEnumerable<DetectedEntity> Inform(Point critterLocation, Vector critterForward)
		{
			foreach (string detectedObject in SplitBody.Skip(1))
			{
				string[] entityComponents = detectedObject.Split(':');

				if (entityComponents[0] == "Food" || entityComponents[0] == "Gift")
				{
					yield return new DetectedEntity(detectedObject, critterLocation, critterForward);
				}
			}
		}
	}
}
