namespace Recall.Data
{
    using Microsoft.EntityFrameworkCore;
    using Recall.Data.Models.Core;
    using Recall.Data.Models.Meta;

    public class RecallDbContext : DbContext
    {
        public RecallDbContext(DbContextOptions<RecallDbContext> options)
            : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<Directory> Directories { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Note> Notes { get; set; } 

        public DbSet<ExtensionAddedVideo> ExtensionAddedVideos { get; set; }

        public DbSet<Connection> Connections { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<VideoTopic> VideosTopics { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Directory>(d =>
            {
                d.HasOne(x => x.User)
                .WithMany(x => x.Directories)
                .HasForeignKey(x => x.UserId);

                d.HasOne(x => x.ParentDirectory)
                .WithMany(x => x.Subdirectories)
                .HasForeignKey(x => x.ParentDirectoryId);

                d.HasOne(x => x.RootUser)
                .WithOne(x => x.RootDirectory)
                .HasForeignKey<User>(x => x.RootDirectoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<User>(u =>
            {
                u.HasOne(x => x.RootDirectory)
                 .WithOne(x => x.RootUser)
                 .HasForeignKey<User>(x => x.RootDirectoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Topic>(t =>
            {
                t.HasOne(x => x.ParentTopic)
                .WithMany(x => x.Subtopics)
                .HasForeignKey(x => x.ParentTopicId);
            });

            builder.Entity<Connection>(c => {
                c.HasKey(x => new { x.VideoOneId, x.VideoTwoId });

                c.HasOne(x => x.VideoOne)
                 .WithMany(x => x.ConnectedTo)
                 .HasForeignKey(x => x.VideoOneId);

                c.HasOne(x => x.VideoTwo)
                 .WithMany(x => x.ConnectedFrom)
                 .HasForeignKey(x => x.VideoTwoId);
            });

            builder.Entity<VideoTopic>(vt =>
            {
                vt.HasKey(x => new { x.VideoId, x.TopicId });

                vt.HasOne(x => x.Video)
                .WithMany(x => x.Topics)
                .HasForeignKey(x => x.VideoId);

                vt.HasOne(x => x.Topic)
                .WithMany(x => x.Videos)
                .HasForeignKey(x => x.TopicId);
            });

            base.OnModelCreating(builder);
        }
    }
}
