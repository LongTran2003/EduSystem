using EduSystem.DataAccess.DBContext;
using EduSystem.DataAccess.IRepositories;
using EduSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduSystem.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public IAnswerRepository Answer { get; private set; }
        public ILessonRepository Lesson { get; private set; }
        public ILessonContentRepository LessonContent { get; private set; }
        public IQuestionRepository Question { get; private set; }
        public IQuizRepository Quiz { get; private set; }
        public IQuizQuestionRepository QuizQuestion { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IStudentProgressRepository StudentProgress { get; private set; }
        public IStudentAnswerRepository StudentAnswer { get; private set; }
        public IStudentQuizAttemptRepository StudentQuizAttempt { get; private set; }
        public ISubjectRepository Subject { get; private set; }
        public ITeacherRepository Teacher { get; private set; }


        public UnitOfWork(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Answer = new AnswerRepository(_context);
            Lesson = new LessonRepository(_context);
            LessonContent = new LessonContentRepository(_context);
            Question = new QuestionRepository(_context);
            Quiz = new QuizRepository(_context);
            QuizQuestion = new QuizQuestionRepository(_context);
            StudentAnswer = new StudentAnswerRepository(_context);
            StudentQuizAttempt = new StudentQuizAttemptRepository(_context);
            StudentProgress = new StudentProgressRepository(_context);
            Student = new StudentRepository(_context);
            Subject = new SubjectRepository(_context);
            Teacher = new TeacherRepository(_context);
        }



        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
