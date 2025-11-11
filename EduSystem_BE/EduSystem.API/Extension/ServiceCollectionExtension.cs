using EduSystem.DataAccess.IRepositories;
using EduSystem.DataAccess.Repositories;
using EduSystem.Services.IServices;
using EduSystem.Services.Mapping;
using EduSystem.Services.Services;
using EduSystem.Services.Services.CloudinaryModule.Invoker;
using StackExchange.Redis;

namespace EduSystem.API.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,
    ConfigurationManager builderConfiguration)
        {
            // Đọc chuỗi kết nối Redis từ file cấu hình
            var redisConnectionString = builderConfiguration.GetValue<string>("Redis:ConnectionString");
            // Đăng ký IConnectionMultiplexer
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<CloudinaryServiceControl>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IManageUserAccountService, ManageUserAccountService>();

            // add services here
            services.AddScoped<IAutoMapperService, AutoMapperService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ILessonContentService, LessonContentService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IMatrixService, MatrixService>();
            services.AddScoped<IMatrixDetailService, MatrixDetailService>();
            services.AddScoped<IQuizAttemptService, QuizAttemptService>();
            services.AddScoped<IStudentAnswerService, StudentAnswerService>();
            services.AddScoped<IQuizQuestionService, QuizQuestionService>();


            return services;
        }
    }
}
