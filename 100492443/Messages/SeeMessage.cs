using CritterRobots.AI;
using CritterRobots.Critters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Represents the SEE message in an object
	/// oriented form.
	/// </summary>
	public class SeeMessage : SimpleMessage
	{
		/// <summary>
		/// Iteratively returns all relevant detected entities.
		/// </summary>
		public virtual IEnumerable<DetectedEntity> Inform(Point critterLocation, Vector critterForward)
		{
			foreach (string detectedObject in SplitBody)
			{
				string[] entityComponents = detectedObject.Split(':');

				if (entityComponents[0] == "Critter" || entityComponents[0] == "Bomb" || entityComponents[0] == "Terrain")
				{
					yield return new DetectedEntity(detectedObject, critterLocation, critterForward));
				}
			}
		}

		/// <summary>
		/// Constructs 
		/// </summary>
		/// <param name="sourceMessage"></param>
		public SeeMessage(string sourceMessage) : base(sourceMessage, "\n\t")
		{ }
	}
}
