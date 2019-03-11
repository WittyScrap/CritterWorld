using System.Drawing;

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
	class ReadonlyObject
	{
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
	}
}
