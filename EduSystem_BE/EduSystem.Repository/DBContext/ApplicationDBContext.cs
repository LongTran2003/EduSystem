using EduSystem.Models.Entities;
using EduSystem.Repository.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.Repository.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // DbSets sorted alphabetically
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonContent> LessonContents { get; set; }
        public DbSet<Matrix> Matrices { get; set; }
        public DbSet<MatrixDetail> MatrixDetails { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<StudentProgress> StudentProgresses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Unit> Units { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin account
            ApplicationDbContextSeed.SeedAdminAccount(modelBuilder);

            // Thêm các cấu hình khác nếu cần
            // Answer configuration
            modelBuilder.Entity<Answer>()
                .HasKey(a => a.AnswerId);
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            // Lesson configuration
            modelBuilder.Entity<Lesson>()
                .HasKey(l => l.LessonId);
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Unit)
                .WithMany(u => u.Lessons)
                .HasForeignKey(l => l.UnitId);

            // LessonContent configuration
            modelBuilder.Entity<LessonContent>()
                .HasKey(lc => lc.LessonContentId);
            modelBuilder.Entity<LessonContent>()
                .HasOne(lc => lc.Lesson)
                .WithMany(l => l.LessonContents)
                .HasForeignKey(lc => lc.LessonId);

            // Matrix configuration
            modelBuilder.Entity<Matrix>()
                .HasKey(m => m.MatrixId);
            modelBuilder.Entity<Matrix>()
                .HasOne(m => m.Teacher)
                .WithMany(t => t.Matrices)
                .HasForeignKey(m => m.TeacherId);

            // MatrixDetail configuration
            modelBuilder.Entity<MatrixDetail>()
                .HasKey(md => md.DetailId);
            modelBuilder.Entity<MatrixDetail>()
                .HasOne(md => md.Matrix)
                .WithMany(m => m.MatrixDetails)
                .HasForeignKey(md => md.MatrixId);

            // Question configuration
            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionId);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Teacher)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TeacherId);

            // Quiz configuration
            modelBuilder.Entity<Quiz>()
                .HasKey(q => q.QuizId);
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Matrix)
                .WithMany(m => m.Quizzes)
                .HasForeignKey(q => q.MatrixId);
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Teacher)
                .WithMany(t => t.Quizzes)
                .HasForeignKey(q => q.TeacherId);

            // QuizAttempt configuration
            modelBuilder.Entity<QuizAttempt>()
                .HasKey(qa => qa.QuizAttemptId);
            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.Student)
                .WithMany(s => s.QuizAttempts)
                .HasForeignKey(qa => qa.StudentId);
            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.Quiz)
                .WithMany(q => q.QuizAttempts)
                .HasForeignKey(qa => qa.QuizId);

            // QuizQuestion configuration (Composite Key)
            modelBuilder.Entity<QuizQuestion>()
                .HasKey(qq => new { qq.QuizId, qq.QuestionId });
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId);
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Question)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuestionId);

            // Student configuration
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);
            modelBuilder.Entity<Student>()
                .HasOne(s => s.ApplicationUser)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            // StudentAnswer configuration (Composite Key)
            modelBuilder.Entity<StudentAnswer>(entity =>
            {
                entity.HasKey(sa => sa.StudentAnswerId);

                // Mỗi Attempt chỉ có 1 câu trả lời cho 1 Question
                entity.HasIndex(sa => new { sa.AttemptId, sa.QuestionId }).IsUnique();

                entity.HasOne(sa => sa.QuizAttempt)
                      .WithMany(qa => qa.StudentAnswers)
                      .HasForeignKey(sa => sa.AttemptId)
                      .OnDelete(DeleteBehavior.Cascade);

                // IMPORTANT: bắt cặp đúng navigation để tránh shadow FK (*Id1)
                entity.HasOne(sa => sa.Question)
                      .WithMany(q => q.StudentAnswers)          // <-- dùng nav ở Question
                      .HasForeignKey(sa => sa.QuestionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sa => sa.Answer)
                      .WithMany(a => a.StudentAnswers)          // <-- dùng nav ở Answer
                      .HasForeignKey(sa => sa.AnswerId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // StudentProgress configuration (Composite Key)
            modelBuilder.Entity<StudentProgress>(entity =>
            {
                entity.HasKey(sp => sp.ProgressId);

                entity.HasOne(sp => sp.Student)
                    .WithMany(s => s.StudentProgresses)
                    .HasForeignKey(sp => sp.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sp => sp.Unit)
                    .WithMany()
                    .HasForeignKey(sp => sp.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(sp => new { sp.StudentId, sp.UnitId })
                    .IsUnique();
            });

            // Teacher configuration
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.TeacherId);
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.ApplicationUser)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            // Unit configuration
            modelBuilder.Entity<Unit>()
                .HasKey(u => u.UnitId);
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Teacher)
                .WithMany(t => t.Units)
                .HasForeignKey(u => u.TeacherId);

        }
    }
}
