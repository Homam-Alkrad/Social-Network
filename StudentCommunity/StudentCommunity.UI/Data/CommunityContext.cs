using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Models;

namespace StudentCommunity.UI.Data
{
	public class CommunityContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Post> Posts { get; set; }
		public DbSet<University> Universities { get; set; }
		public DbSet<PostsIntractions> PostsIntractions { get; set; }
		public DbSet<College> Colleges { get; set; }
		public DbSet<Major> Majors { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<LibraryResource> LibraryResources { get; set; }
		public DbSet<ResourcesIntractions> Intractions { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<RecommendedToYou> RecommendedToYou { get; set; }
		public DbSet<CompetitionTeam> CompetitionTeams { get; set; }
		public DbSet<AnnouncedCompetition> AnnouncedCompetition { get; set; }
		public DbSet<Questions> Questions { get; set; }
		public DbSet<CompetitionQuestion> CompetitionQuestions { get; set; }
		public DbSet<QuestionAnswer> QuestionsAnswers { get; set; }
		public DbSet<CompetitionMember> CompetitionMembers { get; set; }
		public DbSet<FriendRequest> FriendRequests { get; set; }
		public DbSet<Competition> Competitions { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }
		public DbSet<Assessment> Feadbacks { get; set; }
		public DbSet<Advertising> Advertisings { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public CommunityContext() { }

		public CommunityContext(DbContextOptions<CommunityContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>().ToTable("Students");
			modelBuilder.Entity<LibraryResource>().ToTable("Resources");
			modelBuilder.Entity<CompetitionTeam>().ToTable("Teams");
			modelBuilder.Entity<CompetitionQuestion>().ToTable("QuestionBank");

			modelBuilder.Entity<PostsIntractions>()
				.HasKey(pum => new { pum.UserId, pum.PostId });

			modelBuilder.Entity<PostsIntractions>()
				.HasOne(pum => pum.User)
				.WithMany(u => u.PostUser)
				.HasForeignKey(pum => pum.UserId)
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<ResourcesIntractions>()
				.HasOne(ri => ri.resource)
				.WithMany(r => r.UserIntraction)
				.HasForeignKey(ri => ri.ResourceId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<FriendRequest>()
			.HasOne(f => f.SenderUser)
			.WithMany(u => u.SentFriendRequests)
			.HasForeignKey(f => f.SenderUserId)
			.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<FriendRequest>()
				.HasOne(f => f.ReceiverUser)
				.WithMany(u => u.ReceivedFriendRequests)
				.HasForeignKey(f => f.ReceiverUserId)
				.OnDelete(DeleteBehavior.NoAction);


			modelBuilder.Entity<Post>()
				   .HasOne(p => p.User)
				   .WithMany(u => u.Posts)
				   .HasForeignKey(p => p.UserId)
				   .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CompetitionQuestion>()
	 .HasKey(cq => new { cq.CompetitionId, cq.QuestionId });

			modelBuilder.Entity<CompetitionQuestion>()
				.HasOne(cq => cq.Competition)
				.WithMany(c => c.CompetitionQuestions)
				.HasForeignKey(cq => cq.CompetitionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CompetitionQuestion>()
				.HasOne(cq => cq.Question)
				.WithMany(q => q.CompetitionQuestions)
				.HasForeignKey(cq => cq.QuestionId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<QuestionAnswer>()
			.HasOne(qa => qa.CompetitionQuestion)
			.WithMany()
			.HasForeignKey(qa => qa.QuestionId)
			.OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<UserGroup>()
		 .HasKey(ug => new { ug.UserId, ug.GroupId });

			modelBuilder.Entity<UserGroup>()
				.HasOne(ug => ug.User)
				.WithMany(u => u.Groups)
				.HasForeignKey(ug => ug.UserId);

			modelBuilder.Entity<UserGroup>()
				.HasOne(ug => ug.Group)
				.WithMany(g => g.Users)
				.HasForeignKey(ug => ug.GroupId);

			modelBuilder.Entity<Room>()
				.HasOne(r => r.CompetitionTeam)
				.WithOne(c => c.Room)
				.HasForeignKey<CompetitionTeam>(c => c.RoomId)
				.OnDelete(DeleteBehavior.Restrict); 


		}

	}
}
