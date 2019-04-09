using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters
{
	/// <summary>
	/// Exception thrown by a critter or critter
	/// related class.
	/// </summary>
	class CritterException : Exception
	{
		public CritterException(string message) : base(message)
		{ }
	}
}
