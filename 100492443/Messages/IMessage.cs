using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Provides a basic interface on the
	/// methods and properties shared by 
	/// all messages.
	/// </summary>
	interface IMessage
	{
		/// <summary>
		/// The header for this message.
		/// </summary>
		string Header { get; }

		/// <summary>
		/// The body of this message.
		/// </summary>
		string Body { get; }

		/// <summary>
		/// Extracts a string from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted string.</returns>
		string GetString(int offset);

		/// <summary>
		/// Extracts and parses an integer from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// </summary>
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted integer.</returns>
		int GetInteger(int offset);

		/// <summary>
		/// Extracts and parses a double from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// </summary>
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted integer.</returns>
		double GetDouble(int offset);

		/// <summary>
		/// Extracts and parses a point from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// </summary>
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted point.</returns>
		Point GetPoint(int offset);

		/// <summary>
		/// Converts this message into a CritterWorld compatible string.
		/// </summary>
		/// <returns>A CritterWorld compatible formatted string.</returns>
		string Format();
	}
}
