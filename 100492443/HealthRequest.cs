namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Requests the amount of health left for a critter.
	/// </summary>
	class HealthRequest : TrackableRequest
	{
		public HealthRequest()
			: base ("GET_HEALTH")
		{ }
	}
}
