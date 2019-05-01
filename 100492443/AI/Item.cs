using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.AI
{
	/// <summary>
	/// Represents an input structure for a neural network,
	/// formed of the distance between a given item and an
	/// angle in world space for the given item.
	/// </summary>
	public struct Item : IComparable<Item>
	{
		/// <summary>
		/// The distance from the detected item to the critter.
		/// </summary>
		public double Distance { get; set; }

		/// <summary>
		/// The angle formed between the world space UP vector
		/// and the vector generated from the critter's position
		/// to the item's position.
		/// </summary>
		public double Angle { get; set; }

		/// <summary>
		/// Creates a new item.
		/// </summary>
		public Item(double distance, double angle)
		{
			Distance = distance;
			Angle = angle;
		}

		/// <summary>
		/// Compares this item's distance to the 
		/// given item's distance.
		/// </summary>
		public int CompareTo(Item other)
		{
			return Distance.CompareTo(other.Distance);
		}
	}
}
