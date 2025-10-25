using Microsoft.EntityFrameworkCore.Storage;

namespace EduSystem.DataAccess.IRepositories
{
    public interface IUnitOfWork
    {
        IAnswerRepository Answer { get; }
        ILessonRepository Lesson { get; }
        ILessonContentRepository LessonContent { get; }
        IQuestionRepository Question { get; }
        IQuizRepository Quiz { get; }
        IQuizQuestionRepository QuizQuestion { get; }
        IStudentRepository Student { get; }
        IStudentAnswerRepository StudentAnswer { get; }
        IStudentQuizAttemptRepository StudentQuizAttempt { get; }
        IStudentProgressRepository StudentProgress { get; }
        ISubjectRepository Subject { get; }
        ITeacherRepository Teacher { get; }
        
        
        



        Task<int> SaveAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
