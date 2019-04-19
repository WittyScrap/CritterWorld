using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Message templates for different purposes.
	/// </summary>
	static class DefaultMessages
	{
		/// <summary>
		/// Sets a random destination.
		/// </summary>
		public static SimpleMessage RandomDestination {
			get
			{
				return new SimpleMessage("RANDOM_DESTINATION");
			}
		}
	}
}
