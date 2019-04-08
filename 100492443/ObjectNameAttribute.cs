using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Defines a custom attribute to define
	/// the real name of detected scan objects.
	/// </summary>
	class ObjectNameAttribute : Attribute
	{
		/// <summary>
		/// Define the real name of the object
		/// this <see cref="ReadonlyObject"/> implements.
		/// </summary>
		public ObjectNameAttribute(string realName, Type classType)
		{
			ObjectParser.CacheObject(realName, classType);
		}
	}
}
