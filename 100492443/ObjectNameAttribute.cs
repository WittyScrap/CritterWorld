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
		/// The cached data from all attributes.
		/// </summary>
		private static Dictionary<string, Type> static_CachedData = new Dictionary<string, Type>();

		/// <summary>
		/// Define the real name of the object
		/// this <see cref="ReadonlyObject"/> implements.
		/// </summary>
		public ObjectNameAttribute(string realName, Type classType)
		{
			static_CachedData[realName] = classType;
		}

		/// <summary>
		/// Returns the cached type for the specified real object name.
		/// </summary>
		public static Type GetClass(string realName)
		{
			if (static_CachedData.ContainsKey(realName))
			{
				return static_CachedData[realName];
			}

			return null;
		}
	}
}
