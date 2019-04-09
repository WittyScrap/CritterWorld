namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Requests the current location of a critter.
	/// </summary>
	class LocationRequest : TrackableRequest
	{
		public LocationRequest()
			: base ("GET_LOCATION")
		{ }
	}
}
