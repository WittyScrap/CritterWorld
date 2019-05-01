using CritterRobots.AI;
using CritterRobots.Critters;
using CritterRobots.Critters.Controllers;
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
		/// The internal body, skipping the first element.
		/// </summary>
		protected IEnumerable<string> CorrectedBody {
			get
			{
				return SplitBody.Skip(1);
			}
		}

		/// <summary>
		/// Iteratively returns all the entities that were detected by this message.
		/// </summary>
		public IEnumerable<LocatableEntity> GetEntities()
		{
			if (SplitBody[1] == "Nothing")
			{
				yield break;
			}

			foreach (string unparsedEntity in CorrectedBody)
			{
				string[] entityComponents = unparsedEntity.Split(':');
				string entityType = entityComponents[0];
				string entityPosition = entityComponents[1];

				if (Enum.TryParse(entityType, out LocatableEntity.Entity e) && Critter.TryParsePoint(entityPosition, out Point parsedPoint))
				{
					yield return new LocatableEntity(e, parsedPoint);
				}
			}
		}

		/// <summary>
		/// Iteratively returns all entities that match the predicate.
		/// </summary>
		/// <param name="predicate">The predicate for the entities to match.</param>
		public IEnumerable<LocatableEntity> GetEntities(Predicate<LocatableEntity> predicate)
		{
			foreach (LocatableEntity parsedEntity in GetEntities())
			{
				if (predicate(parsedEntity))
				{
					yield return parsedEntity;
				}
			}
		}

		/// <summary>
		/// Constructs 
		/// </summary>
		/// <param name="sourceMessage"></param>
		public SeeMessage(string sourceMessage) : base(sourceMessage, "\n\t")
		{
			
		}
	}
}
