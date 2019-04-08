﻿using System.Drawing;
using System;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace _100492443.Critters.AI
{
	/// <summary>
	/// Represents a critter detected by a
	/// SCAN or SEE operation.
	/// </summary>
	[ObjectName("Critter", typeof(DetectedCritter))]
	class DetectedCritter : ReadonlyObject, IEquatable<DetectedCritter>
	{
		/// <summary>
		/// The name of the critter.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The health of the critter.
		/// </summary>
		public float Health { get; private set; }

		/// <summary>
		/// Caches a detected critter with a specific
		/// position, its name and its current health.
		/// </summary>
		/// <param name="position">The critter's position.</param>
		/// <param name="critterName">The critter's name.</param>
		/// <param name="critterHealth">The critter's health.</param>
		public DetectedCritter(Point position, string critterName, float critterHealth) : base(position)
		{
			Name = critterName;
			Health = critterHealth;
		}

		/// <summary>
		/// Equals override.
		/// </summary>
		public bool Equals(DetectedCritter compareTo)
		{
			return base.Equals(compareTo) && compareTo.Name.Equals(Name);
		}
	}
}
