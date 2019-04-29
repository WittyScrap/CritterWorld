using CritterRobots.Critters;
using CritterRobots.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents a series of raycast that are able to detect
	/// objects of a certain type.
	/// </summary>
	public partial class CritterEye
	{
		/// <summary>
		/// Enumerator to represent what type
		/// of contents are present within a given
		/// tile.
		/// </summary>
		[Flags]
		public enum Entity
		{
			Empty = 0,
			Terrain = 1 << 0,
			Gift = 1 << 1,
			Food = 1 << 2,
			Bomb = 1 << 3,
			EscapeHatch = 1 << 4,
			Critter = 1 << 5,
			All = Terrain | Gift | Food | Bomb | EscapeHatch,
			Threats = Critter | Bomb | Terrain
		}

		/// <summary>
		/// The precision (or "resolution") of the eye
		/// is the amount of rays fired outwards from the
		/// center of the critter.
		/// </summary>
		public int Precision { get; }

		/// <summary>
		/// Collection containing all the detected food items.
		/// </summary>
		private List<DetectedEntity> InternalDetectedFood { get; set; } = new List<DetectedEntity>();

		/// <summary>
		/// Collection containing all the detected gifts.
		/// </summary>
		private List<DetectedEntity> InternalDetectedGifts { get; set; } = new List<DetectedEntity>();

		/// <summary>
		/// Collection containing all the detected threats (Critters, Bombs and Walls).
		/// </summary>
		private List<DetectedEntity> InternalDetectedThreats { get; set; } = new List<DetectedEntity>();

		/// <summary>
		/// Refreshes this eye's detected states through a SEE message.
		/// </summary>
		/// <param name="message"></param>
		public void Update(SeeMessage message, Point critterLocation, Vector critterForward)
		{
			InternalDetectedThreats.Clear();

			// Collect every entity and store it in the linked list in a sorted manner.
			foreach (DetectedEntity entity in message.Inform(critterLocation, critterForward))
			{
				
			}
		}



		/// <summary>
		/// Indicates the location of the escape hatch.
		/// </summary>
		private DetectedEntity DetectedEscapeHatch { get; set; } = null;

		/// <summary>
		/// Returns a sequence of rays given this eye's accuracy.
		/// </summary>
		/// <returns>This eye's "retina".</returns>
		public IEnumerable<Ray> GetRays(Point critterLocation, Vector critterForward)
		{
			for (double angle = 0; angle < 2 * Math.PI; angle += Math.PI * 2 / Precision)
			{
				yield return new Ray(critterLocation, critterForward.Rotated(angle));
			}
		}

		/// <summary>
		/// Returns one specific ray.
		/// </summary>
		public Ray GetRay(Point critterLocation, Vector critterForward, int rayID)
		{
			if (rayID < 0 || rayID >= Precision)
			{
				throw new ArgumentOutOfRangeException("rayID");
			}
			double angle = (Math.PI * 2 / Precision) * rayID;
			return new Ray(critterLocation, critterForward.Rotated(angle));
		}

		/// <summary>
		/// Generic version of all check methods.
		/// </summary>
		private decimal CheckEye(Point critterLocation, Vector critterForward, int eyeID, double angularThreshold, double maximumDistance, IReadOnlyCollection<DetectedEntity> collectionSource)
		{
			Ray selectedRay = GetRay(critterLocation, critterForward, eyeID);
			double angle = Vector.Dot(critterForward, selectedRay.Direction);

			foreach (var detectedEntity in collectionSource)
			{
				if (Math.Abs(angle - detectedEntity.Rotation) < angularThreshold)
				{
					return (decimal)(detectedEntity.Distance / maximumDistance);
				}
			}

			return 0m;
		}

		/// <summary>
		/// Checks if the given eye cell can see the specified
		/// entities, and if so returns a scalar from 0 to 1 to
		/// indicate the distance from it to the critter.
		/// </summary>
		/// <param name="eyeID">Which cell should be looked through.</param>
		/// <param name="checkEntity">Which entities should be looked for.</param>
		/// <param name="angularThreshold">The maximum angular distance for an object to be considered detected.</param>
		/// <returns>A scalar from 0 to 1 to indicate the distance from it to the critter.</returns>
		public decimal CheckFood(Point critterLocation, Vector critterForward, int eyeID, double angularThreshold = 0.1, double maximumDistance = 100.0)
		{
			return CheckEye(critterLocation, critterForward, eyeID, angularThreshold, maximumDistance, DetectedFood);
		}

		/// <summary>
		/// Creates a new critter eye.
		/// </summary>
		/// <param name="precision">The density of the eye's "retina".</param>
		public CritterEye(int precision)
		{
			Precision = precision;
		}
	}
}
