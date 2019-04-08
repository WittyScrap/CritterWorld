using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100492443.Critters.AI
{
	/// <summary>
	/// Handles parsing objects from text data
	/// to objects.
	/// </summary>
	static class ObjectParser
	{
		/// <summary>
		/// The cached data from all <see cref="ObjectNameAttribute"/>s.
		/// </summary>
		private static Dictionary<string, Type> static_CachedData = new Dictionary<string, Type>();

		/// <summary>
		/// Returns a previously cached type from a specific name.
		/// </summary>
		/// <param name="objectName">The object name.</param>
		/// <returns>The type of the specified <see cref="ReadonlyObject"/> to represent the real object.</returns>
		public static Type FromName(string objectName)
		{
			if (static_CachedData.ContainsKey(objectName))
			{
				return static_CachedData[objectName];
			}

			return null;
		}

		/// <summary>
		/// Caches the specified class type with a given realobjectname.
		/// </summary>
		/// <param name="realObjectName">The actual name of the object.</param>
		/// <param name="readOnlyObjectType">The <see cref="ReadonlyObject"/> implementation.</param>
		public static void CacheObject(string realObjectName, Type readOnlyObjectType)
		{
			static_CachedData[realObjectName] = readOnlyObjectType;
		}
	}
}
