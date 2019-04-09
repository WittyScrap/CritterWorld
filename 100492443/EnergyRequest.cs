namespace UOD100492443.Critters.AI
{
	/// <summary>
	/// Requests the amount of energy left for a critter.
	/// </summary>
	class EnergyRequest : TrackableRequest
	{
		public EnergyRequest()
			: base ("GET_ENERGY")
		{ }
	}
}
