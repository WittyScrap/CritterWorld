using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Interfaces
{
	/// <summary>
	/// Indicates that this element can be "destroyed" even
	/// while it has not been yet collected by the GC.
	/// </summary>
	public interface IDestroyable
	{
		/// <summary>
		/// Checks whether or not this element was destroyed.
		/// </summary>
		bool IsDestroyed { get; }

		/// <summary>
		/// Destroys this element.
		/// This should take care of cleanup and should set "IsDestroyed" to true.
		/// It should be irreversible.
		/// </summary>
		void Destroy();
	}
}
