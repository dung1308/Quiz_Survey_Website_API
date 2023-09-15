using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Models;


namespace survey_quiz_app.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<QuestionBank> QuestionBanks { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<CategoryList> CategoryList { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<ResultShow> ResultShows { get; set; }

    public DbSet<QuestionBankInteract> QuestionBankInteracts { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<QuestionBank>(entity =>
        //     entity.Property(x => x.Id)
        //     .IsRequired()
        //     .ValueGeneratedOnAdd()
        //     .HasDefaultValueSql("NEWSEQUENTIALID()"));
        // modelBuilder.Entity<QuestionBankInteract>()
        //     .HasOne(qbi => qbi.User)
        //     .WithOne(u => u.QuestionBankInteract)
        //     .HasForeignKey<User>(u => u.QuestionBankInteractId);

        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Role)
        //     .WithMany(r => r.Users)
        //     .HasForeignKey(u => u.RoleId);
        // modelBuilder.Entity<QuestionBank>()
        //     .HasMany(qb => qb.Questions)
        //     .WithOne(q => q.QuestionBank)
        //     .HasForeignKey(q => q.QuestionBankId);
        //  modelBuilder.Entity<QuestionBank>()
        //     .HasOne(c => c.CategoryList)
        //     .WithMany(cl => cl.QuestionBanks)
        //     .HasForeignKey(c => c.CategoryListId);
    }

}