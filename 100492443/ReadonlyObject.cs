using System.Drawing;
using System;
using System.Collections.Generic;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace _100492443.Critters.AI
{
	/// <summary>
	/// Represents an object with a 2D position.
	/// The object cannot be written to, it is
	/// simply the result of SCAN and SEE operations.
	/// </summary>
	class ReadonlyObject : IEquatable<ReadonlyObject>
	{
		/// <summary>
		/// Creates a <see cref="ReadonlyObject"/> from a real object
		/// name.
		/// </summary>
		/// <param name="realObjectName">The actual name of the object, for example Critter.</param>
		/// <returns>An instance of a <see cref="ReadonlyObject"/> generated from the actual name.</returns>
		public static ReadonlyObject Create(string realObjectName, Point coordinates)
		{
			Type objectType = ObjectNameAttribute.GetClass(realObjectName);

			if (objectType != null && objectType.IsSubclassOf(typeof(ReadonlyObject)))
			{
				return (ReadonlyObject)Activator.CreateInstance(objectType, coordinates);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Parses a string of object data into
		/// the class fields.
		/// </summary>
		/// <param name="data">The object data (excluding position).</param>
		public virtual void ParseObjectData(ICollection<string> data)
		{

		}

		/// <summary>
		/// The position of this object.
		/// </summary>
		public Point Position { get; private set; }

		/// <summary>
		/// Creates a new readonly object with a
		/// given position.
		/// </summary>
		/// <param name="position">The position of this object.</param>
		public ReadonlyObject(Point position)
		{
			Position = position;
		}

		/// <summary>
		/// Equals override.
		/// </summary>
		public bool Equals(ReadonlyObject compareTo)
		{
			return GetType().Equals(compareTo.GetType());
		}
	}
}
