namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Requests the amount of time left before the end of
	/// the level.
	/// </summary>
	class TimeRemainingRequest : TrackableRequest
	{
		public TimeRemainingRequest()
			: base("GET_LEVEL_TIME_REMAINING")
		{ }
	}
}
