using EduSystem.Models.Entities;
using EduSystem.Repository.DBContext;
using EduSystem.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduSystem.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public IAnswerRepository Answer { get; private set; }
        public ILessonRepository Lesson { get; private set; }
        public ILessonContentRepository LessonContent { get; private set; }
        public IMatrixRepository Matrix { get; private set; }
        public IMatrixDetailRepository MatrixDetail { get; private set; }
        public IQuestionRepository Question { get; private set; }
        public IQuizRepository Quiz { get; private set; }
        public IQuizQuestionRepository QuizQuestion { get; private set; }
        public IQuizAttemptRepository QuizAttempt { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IStudentProgressRepository StudentProgress { get; private set; }
        public IStudentAnswerRepository StudentAnswer { get; private set; }
        public ITeacherRepository Teacher { get; private set; }
        public IUnitRepository Unit { get; private set; }


        public UnitOfWork(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Answer = new AnswerRepository(_context);
            Lesson = new LessonRepository(_context);
            LessonContent = new LessonContentRepository(_context);
            Matrix = new MatrixRepository(_context);
            MatrixDetail = new MatrixDetailRepository(_context);
            Question = new QuestionRepository(_context);
            Quiz = new QuizRepository(_context);
            QuizQuestion = new QuizQuestionRepository(_context);
            QuizAttempt = new QuizAttemptRepository(_context);
            Student = new StudentRepository(_context);
            StudentAnswer = new StudentAnswerRepository(_context);
            StudentProgress = new StudentProgressRepository(_context);
            Teacher = new TeacherRepository(_context);
            Unit = new UnitRepository(_context);
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
