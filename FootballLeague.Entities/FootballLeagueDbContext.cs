using System;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Entities
{
	/// <summary>
	/// Represents a FootballLeaguee database context.
	/// </summary>
	public partial class FootballLeagueDbContext : DbContext
	{
		public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
			{
				throw new ArgumentNullException(nameof(modelBuilder));
			}

			modelBuilder.Entity<Team>(entity =>
			{
				entity.ToTable("Team");
				entity.Property(e => e.Id).HasColumnType("int").HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();
				entity.Property(t => t.Name).HasMaxLength(50).IsRequired(true);

				entity.HasKey(t => t.Id);

				entity
					.HasOne(t => t.Rank)
					.WithOne(r => r.Team)
					.HasForeignKey<Rank>(r => r.TeamId)
					.OnDelete(DeleteBehavior.Cascade);

				entity
					.HasMany(t => t.HomeMatches)
					.WithOne(m => m.HomeTeam)
					.HasPrincipalKey(t => t.Id)
					.HasForeignKey(m => m.HomeTeamId);

				entity
					.HasMany(t => t.AwayMatches)
					.WithOne(m => m.AwayTeam)
					.HasPrincipalKey(t => t.Id)
					.HasForeignKey(m => m.AwayTeamId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<Match>(entity =>
			{
				entity.ToTable("Match");
				entity.Property(e => e.Id).HasColumnType("int").HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();
				entity.Property(m => m.Date).HasColumnType("datetime").IsRequired(true);
				entity.Property(m => m.HomeTeamScore).HasColumnType("smallint").HasColumnName("HomeTeamScore").IsRequired(true);
				entity.Property(m => m.AwayTeamScore).HasColumnType("smallint").HasColumnName("AwayTeamScore").IsRequired(true);
				entity.Property(m => m.WinnerId).HasColumnType("int").HasColumnName("WinnerId").IsRequired(false);

				entity.HasKey(m => m.Id);

			});

			modelBuilder.Entity<Rank>(entity =>
			{
				entity.ToTable("Rank");
				entity.Property(e => e.Id).HasColumnType("int").HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();
				entity.Property(r => r.Points).HasColumnType("int").HasColumnName("Points").IsRequired(true);

				entity.HasKey(r => r.Id);
			});
		}

		public DbSet<Match> Matches { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Rank> Ranks { get; set; }
	}
}
