namespace AppCloudBlog.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<AdminAuditLog> AdminAuditLogs => Set<AdminAuditLog>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<PostTag> PostTags => Set<PostTag>();
    public DbSet<PostLike> PostLikes => Set<PostLike>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<PostBookmark> PostBookmarks => Set<PostBookmark>();
    public DbSet<UserFollow> UserFollows => Set<UserFollow>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PostTag>()
    .HasKey(pt => new { pt.PostId, pt.TagId });

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

    }
}
