using Microsoft.EntityFrameworkCore.Storage;

namespace EduSystem.Repository.IRepositories
{
    public interface IUnitOfWork
    {
        IAnswerRepository Answer { get; }
        ILessonRepository Lesson { get; }
        ILessonContentRepository LessonContent { get; }
        IMatrixRepository Matrix { get; }
        IMatrixDetailRepository MatrixDetail { get; }
        IQuestionRepository Question { get; }
        IQuizRepository Quiz { get; }
        IQuizAttemptRepository QuizAttempt { get; }
        IQuizQuestionRepository QuizQuestion { get; }
        IStudentRepository Student { get; }
        IStudentAnswerRepository StudentAnswer { get; }
        IStudentProgressRepository StudentProgress { get; }
        ITeacherRepository Teacher { get; }
        IUnitRepository Unit { get; }
        
        
        



        Task<int> SaveAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
