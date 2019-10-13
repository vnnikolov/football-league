namespace FootballLeague.Entities
{
	/// <summary>
	/// Represents a team rank
	/// </summary>
	public partial class Rank
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the points.
		/// </summary>
		public int Points { get; set; }

		/// <summary>
		/// Gets or sets the team Id.
		/// </summary>
		public int TeamId { get; set; }

		#region Navigational properties

		public Team Team { get; set; } 

		#endregion
	}
}
