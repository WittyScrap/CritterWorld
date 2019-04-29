using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterController;
using CritterRobots.Critters.Controllers;

namespace CritterRobots.Critters
{
	/// <summary>
	/// Creates a series of critters of the necessary types.
	/// </summary>
	class CritterFactory : ICritterControllerFactory
	{
		/// <summary>
		/// It's me! This comment is unnecessary please remove it.
		/// If you're seeing this, then I forgot to.
		/// Have a nice day.
		/// </summary>
		public string Author => "Francesco Litrico";

		/// <summary>
		/// Generates an array of all the critters that need to
		/// exist in the competition.
		/// </summary>
		/// <returns>
		/// The generated array of critters.
		/// A bag of bugs.
		/// Wouldn't want to touch that.
		/// </returns>
		public ICritterController[] GetCritterControllers()
		{
			List<ICritterController> critters = new List<ICritterController>();
			int students = 24;
			critters.Add(new CritterTeacher());
			while (students > 0)
			{
				critters.Add(new CritterStudent(students));
				students--;
			}
			return critters.ToArray();
		}
	}
}
