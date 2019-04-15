using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Represents a simple message without reference IDs.
	/// </summary>
	class SeeMessage : ISimpleMessage
	{
		/// <summary>
		/// HasSet containing all the detected objects by this message.
		/// </summary>
		private HashSet<object> DetectedObjects => new HashSet<object>();

		/// <summary>
		/// Contains all the objects detected in this message.
		/// </summary>
		public IReadOnlyCollection<object> Objects {
			get => DetectedObjects;
		}

		/// <summary>
		/// The header for this message.
		/// </summary>
		public string Header => "SEE";

		/// <summary>
		/// Composes this message into a string.
		/// </summary>
		/// <returns></returns>
		public string Compose()
		{
			return Header;
		}

		/// <summary>
		/// Parses a message from a string.
		/// </summary>
		/// <param name="source"></param>
		public void FromString(string source)
		{
			string[] messageSections = source.Split('\n');
			string detectedElements = messageSections[1];
			string[] elementsList = detectedElements.Split('\t');
			foreach (string element in elementsList)
			{
				DetectedObjects.Add(element);
			}
		}
	}
}
