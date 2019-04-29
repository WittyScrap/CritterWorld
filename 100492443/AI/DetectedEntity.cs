using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterRobots.Critters;
using CritterRobots.Critters.Controllers;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents an entity that has been detected by either
	/// a SEE or a SCAN message.
	/// </summary>
	public class DetectedEntity : IComparable<DetectedEntity>
	{
		/// <summary>
		/// The entity that was detected.
		/// </summary>
		public CritterEye.Entity Entity { get; }

		/// <summary>
		/// The absolute location of the entity in the arena.
		/// </summary>
		public Point Location { get; }

		/// <summary>
		/// The relative location of the entity to the holding critter.
		/// </summary>
		public Point RelativeLocation { get; }

		/// <summary>
		/// Normalized direction from the position of the critter to this
		/// entity.
		/// </summary>
		public Vector Direction { get; }

		/// <summary>
		/// The dot product between the direction from the position of the critter
		/// to this entity and the critter's forward direction.
		/// </summary>
		public double Rotation { get; }

		/// <summary>
		/// The distance between the critter and this entity.
		/// </summary>
		public double Distance { get; }

		/// <summary>
		/// Creates a new sight elment.
		/// </summary>
		/// <param name="entityType">The type of sight element detected.</param>
		/// <param name="location">The location of the detected sight element.</param>
		/// <param name="critterLocation">The location of the critter.</param>
		/// <param name="critterDirection">The forward direction of the critter.</param>
		public DetectedEntity(CritterEye.Entity entityType, Point location, Point critterLocation, Vector critterDirection)
		{
			Entity = entityType;
			Location = location;
			RelativeLocation = (Point)((Vector)location - critterLocation);
			Direction = ((Vector)location - critterLocation).Normalized;
			Rotation = Vector.Dot(critterDirection, Direction);
			Distance = ((Vector)RelativeLocation).Magnitude;
		}

		/// <summary>
		/// Creates a new sight element.
		/// </summary>
		/// <param name="sightElement">The formatted sight element's contents.</param>
		/// <param name="critterLocation">The location of the critter.</param>
		/// <param name="critterDirection">The forward direction of the critter.</param>
		public DetectedEntity(string sightElement, Point critterLocation, Vector critterDirection)
		{
			DissectEntity(sightElement, out CritterEye.Entity entity, out Point entityLocation);

			Entity = entity;
			Location = entityLocation;
			RelativeLocation = (Point)((Vector)Location - critterLocation);
			Direction = ((Vector)Location - critterLocation).Normalized;
			Rotation = Vector.Dot(critterDirection, Direction);
			Distance = ((Vector)RelativeLocation).Magnitude;
		}

		/// <summary>
		/// Extracts the entity type from the string block containing it.
		/// </summary>
		private static void DissectEntity(string sightElement, out CritterEye.Entity entity, out Point entityLocation)
		{
			StringBuilder entityName = new StringBuilder();
			StringBuilder entityDetails = new StringBuilder();

			int sightElementIndexer = 0;

			while (sightElement[sightElementIndexer] != ':' && sightElementIndexer < sightElement.Length)
			{
				entityName.Append(sightElement[sightElementIndexer]);
				sightElementIndexer++;
			}

			// Skip the ':'
			sightElementIndexer++;

			while (sightElementIndexer < sightElement.Length)
			{
				entityDetails.Append(sightElement[sightElementIndexer]);
				sightElementIndexer++;
			}

			if (Enum.TryParse(entityName.ToString(), out entity))
			{
				if (entity == CritterEye.Entity.Critter)
				{
					entityLocation = Critter.ParsePoint(entityDetails.ToString().Split(new char[] { ':' }, 2)[0]);
				}
				else
				{
					entityLocation = Critter.ParsePoint(entityDetails.ToString());
				}
			}
			else
			{
				entity = CritterEye.Entity.Empty;
				entityLocation = Point.Empty;
			}
		}

		/// <summary>
		/// Compares this entity against a different one.
		/// </summary>
		/// <param name="other">The entity to compare against.</param>
		public int CompareTo(DetectedEntity other)
		{
			return Distance.CompareTo(other.Distance);
		}
	}
}
