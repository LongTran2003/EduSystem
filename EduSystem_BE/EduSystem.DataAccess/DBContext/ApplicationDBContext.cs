using EduSystem.DataAccess.Seed;
using EduSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduSystem.DataAccess.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // DbSet các entity
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        // Các DbSet khác...
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonContent> LessonContents { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<StudentProgress> StudentProgresses { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<StudentQuizAttempt> StudentQuizAttempts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin account
            ApplicationDbContextSeed.SeedAdminAccount(modelBuilder);

            // Thêm các cấu hình khác nếu cần
            // Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);
            modelBuilder.Entity<Student>()
                .HasOne(s => s.ApplicationUser)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            // Teacher
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.TeacherId);
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.ApplicationUser)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            // Subject
            modelBuilder.Entity<Subject>()
                .HasKey(s => s.SubjectId);

            // Lesson
            modelBuilder.Entity<Lesson>()
                .HasKey(l => l.LessonId);
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Subject)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SubjectId);
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Teacher)
                .WithMany()
                .HasForeignKey(l => l.TeacherId);

            // LessonContent
            modelBuilder.Entity<LessonContent>()
                .HasKey(lc => lc.LessonContentId);
            modelBuilder.Entity<LessonContent>()
                .HasOne(lc => lc.Lesson)
                .WithMany(l => l.LessonContents)
                .HasForeignKey(lc => lc.LessonId);

            // Question
            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionId);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Subject)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SubjectId);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Lesson)
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.LessonId);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Teacher)
                .WithMany()
                .HasForeignKey(q => q.TeacherId);

            // Answer
            modelBuilder.Entity<Answer>()
                .HasKey(a => a.AnswerId);
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            // StudentAnswer
            modelBuilder.Entity<StudentAnswer>()
                .HasKey(sa => sa.StudentAnswerId);
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Student)
                .WithMany()
                .HasForeignKey(sa => sa.StudentId);
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId);
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Answer)
                .WithMany()
                .HasForeignKey(sa => sa.AnswerId);

            // StudentProgress
            modelBuilder.Entity<StudentProgress>()
                .HasKey(sp => sp.StudentProgressId);
            modelBuilder.Entity<StudentProgress>()
                .HasOne(sp => sp.Student)
                .WithMany()
                .HasForeignKey(sp => sp.StudentId);
            modelBuilder.Entity<StudentProgress>()
                .HasOne(sp => sp.Lesson)
                .WithMany(l => l.StudentProgresses)
                .HasForeignKey(sp => sp.LessonId);

            // Quiz
            modelBuilder.Entity<Quiz>()
                .HasKey(q => q.QuizId);
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Subject)
                .WithMany()
                .HasForeignKey(q => q.SubjectId);
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Teacher)
                .WithMany()
                .HasForeignKey(q => q.TeacherId);

            // QuizQuestion
            modelBuilder.Entity<QuizQuestion>()
                .HasKey(qq => qq.QuizQuestionId);
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId);
            modelBuilder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Question)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuestionId);

            // StudentQuizAttempt
            modelBuilder.Entity<StudentQuizAttempt>()
                .HasKey(sqa => sqa.StudentQuizAttemptId);
            modelBuilder.Entity<StudentQuizAttempt>()
                .HasOne(sqa => sqa.Student)
                .WithMany()
                .HasForeignKey(sqa => sqa.StudentId);
            modelBuilder.Entity<StudentQuizAttempt>()
                .HasOne(sqa => sqa.Quiz)
                .WithMany(q => q.StudentQuizAttempts)
                .HasForeignKey(sqa => sqa.QuizId);

        }
    }
}
