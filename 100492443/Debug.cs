using System;
using CritterController;
using System.IO;

/// <summary>
/// Project bounds namespace.
/// </summary>
namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Handles logging messages.
	/// </summary>
	class Debug
	{
		private Send m_loggerObject;
		private readonly string m_critterID;

		/// <summary>
		/// Logs raw text to either the logger
		/// or the console.
		/// </summary>
		/// <param name="rawMessage">The text to be represented as-is.</param>
		private void LogMessageRaw(string rawMessage)
		{
			if (m_loggerObject != null)
			{
				m_loggerObject(rawMessage);
			}
			else
			{
				Console.WriteLine(rawMessage);
			}
		}

		/// <summary>
		/// Logs a simple message either to the
		/// logger or the console.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		public void LogMessage(string message)
		{
			LogMessageRaw("[LOG:" + m_critterID + "] " + message);
		}

		/// <summary>
		/// Logs a warning message either to the
		/// logger or the console.
		/// </summary>
		/// <param name="warning">The warning to be logged.</param>
		public void LogWarning(string warning)
		{
			LogMessageRaw("[WARNING:" + m_critterID + "] " + warning);
		}

		/// <summary>
		/// Logs an error message either to the
		/// logger or the console.
		/// </summary>
		/// <param name="error">The error message.</param>
		public void LogError(string error)
		{
			LogMessageRaw("[ERROR:" + m_critterID + "] " + error);
		}

		/// <summary>
		/// Creates a new debugger object.
		/// </summary>
		/// <param name="loggerObject">The delegate used to log messages.</param>
		public Debug(Send loggerObject, string critterID)
		{
			m_loggerObject = loggerObject;
			m_critterID = critterID;
		}

		/// <summary>
		/// Resets the logger object to the given reference.
		/// </summary>
		/// <param name="logger">The new logger object.</param>
		public void UpdateLogger(Send logger)
		{
			m_loggerObject = logger;
		}
	}
}
