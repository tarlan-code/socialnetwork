using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zust.Domain.Entities;

namespace Zust.Persistence.Contexts
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PrivacySetting> PrivacySettings { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventAttend> EventAttends { get; set; }
        public DbSet<Contact> Contacts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().Property(u => u.UserName).HasMaxLength(30);

            modelBuilder.Entity<Message>()
        .HasOne(e => e.Receiver)
        .WithMany(e => e.ReceiverMessages)
        .HasForeignKey(e => e.ReceiverId)
        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<PostTag>()
       .HasOne(e => e.AppUser)
       .WithMany(e => e.PostTags)
       .HasForeignKey(e => e.AppUserId)
       .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friend>()
       .HasOne(e => e.Receiver)
       .WithMany(e => e.ReceiverFriends)
       .HasForeignKey(e => e.ReceiveId)
       .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Friend>()
       .HasOne(e => e.Sender)
       .WithMany(e => e.SenderFriends)
       .HasForeignKey(e => e.SenderId)
       .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Comment>()
       .HasOne(e => e.AppUser)
       .WithMany(e => e.Comments)
       .HasForeignKey(e => e.AppUserId)
       .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Comment>().
     HasOne(e => e.Replied).
     WithMany().
     HasForeignKey(m => m.RepliedId);


            modelBuilder.Entity<Friend>(builder =>
            {
                builder
                    .HasIndex(e => new { e.SenderId, e.ReceiveId })
                    .IsUnique();
                builder
                    .HasIndex(e => new { e.ReceiveId, e.SenderId })
                    .IsUnique();
            });


            modelBuilder.Entity<PostReaction>()
        .HasOne(e => e.AppUser)
        .WithMany(e => e.PostReactions)
        .HasForeignKey(e => e.AppUserId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
        .HasOne(e => e.Receiver)
        .WithMany(e => e.Notifications)
        .HasForeignKey(e => e.ReceiverId)
        .OnDelete(DeleteBehavior.NoAction);

             modelBuilder.Entity<EventAttend>()
        .HasOne(e => e.Event)
        .WithMany(e => e.EventAttends)
        .HasForeignKey(e => e.EventId)
        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
