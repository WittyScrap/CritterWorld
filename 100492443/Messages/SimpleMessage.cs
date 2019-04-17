using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterRobots.Messages
{
	/// <summary>
	/// Provides a simple messaging system
	/// implementation.
	/// </summary>
	class SimpleMessage : IMessage
	{
		/// <summary>
		/// The header for this message.
		/// </summary>
		public string Header { get; }

		/// <summary>
		/// The body of this message.
		/// </summary>
		public string Body { get; }

		/// <summary>
		/// The separators that should be used
		/// when splitting the body of the message.
		/// </summary>
		protected string Separators { get; }

		/// <summary>
		/// Provides a version of the body already
		/// separated in its components.
		/// </summary>
		protected string[] SplitBody { get; }

		/// <summary>
		/// Constructs a simple message from a source message.
		/// </summary>
		/// <param name="sourceMessage">Formatted source message/</param>
		public SimpleMessage(string sourceMessage) : this(sourceMessage, ":", ":")
		{ }

		/// <summary>
		/// Explicit constructor.
		/// </summary>
		protected SimpleMessage(string sourceMessage, string separators, string divisors)
		{
			Separators = separators;
			string[] headerBodySplit = sourceMessage.Split(divisors.ToCharArray(), 2);
			Header = headerBodySplit[0];
			Body = headerBodySplit[1];
			SplitBody = Body.Split(Separators.ToCharArray());
		}

		/// <summary>
		/// Extracts a string from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted string.</returns>
		public virtual string GetString(int offset)
		{
			if (offset < SplitBody.Length && offset > 0)
			{
				return SplitBody[offset];
			}
			throw new ArgumentOutOfRangeException("offset");
		}

		/// <summary>
		/// Extracts and parses an integer from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// </summary>
		/// <param name="separators">The separators.</param>
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted integer.</returns>
		public virtual int GetInteger(int offset)
		{
			if (offset < SplitBody.Length && offset > 0)
			{
				if (int.TryParse(SplitBody[offset], out int parsedValue))
				{
					return parsedValue;
				}
				throw new ArgumentException("Message component " + SplitBody[offset] + " could not be parsed to an integer.");
			}
			throw new ArgumentOutOfRangeException("offset");
		}

		/// <summary>
		/// Extracts and parses a double from the <see cref="Body"/>,
		/// by selecting the element in the array obtained
		/// by splitting the <see cref="Body"/>.
		/// </summary>
		/// <param name="offset">Which element should be extracted.</param>
		/// <returns>The extracted integer.</returns>
		public virtual double GetDouble(int offset)
		{
			if (offset < SplitBody.Length && offset > 0)
			{
				if (double.TryParse(SplitBody[offset], out double parsedValue))
				{
					return parsedValue;
				}
				throw new ArgumentException("Message component " + SplitBody[offset] + " could not be parsed to a double.");
			}
			throw new ArgumentOutOfRangeException("offset");
		}
	}
}
