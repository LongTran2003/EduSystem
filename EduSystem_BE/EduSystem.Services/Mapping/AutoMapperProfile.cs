using AutoMapper;
using EduSystem.Models.DTO.Authentication;
using EduSystem.Models.DTO.Lesson;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.DTO.Quiz;
using EduSystem.Models.DTO.Student;
using EduSystem.Models.DTO.Unit;
using EduSystem.Models.DTOs.Answer;
using EduSystem.Models.DTOs.LessonContent;
using EduSystem.Models.DTOs.Matrix;
using EduSystem.Models.DTOs.MatrixDetail;
using EduSystem.Models.DTOs.StudentAnswers;
using EduSystem.Models.DTOs.Teacher;
using EduSystem.Models.Entities;
using EduSystem.Utilities.Contants;

namespace EduSystem.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // SignUpStudentDTO to ApplicationUser
            CreateMap<SignUpStudentDto, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

            CreateMap<SignUpStudentDto, Student>();

            // SignUpTeacherDTO to ApplicationUser
            CreateMap<SignUpTeacherDto, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true));

            // Student to GetStudentDto
            CreateMap<Student, GetStudentDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.ApplicationUser.BirthDate))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ApplicationUser.Gender))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ApplicationUser.Address))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl));

            // UpdateUserProfileDto to ApplicationUser
            CreateMap<UpdateUserProfileDto, ApplicationUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ReverseMap();


            ////// Add more mappings as needed


            // Teacher to GetTeacherDto 
            CreateMap<Teacher, GetTeacherDto>()
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
                .ForMember(dest => dest.TeacherEmail, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.TeacherDOB, opt => opt.MapFrom(src => src.ApplicationUser.BirthDate))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ApplicationUser.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ApplicationUser.Address))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TeacherCode, opt => opt.MapFrom(src => src.TeacherCode))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.TeachingExperience, opt => opt.MapFrom(src => src.TeachingExperience))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating));

            // Lesson to LessonDto
            CreateMap<Lesson, LessonDto>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.UnitName))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.LessonName))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();
            
            CreateMap<Lesson, CreateLessonDto>()
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.LessonName))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ReverseMap();

            CreateMap<Lesson, UpdateLessonDto>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.LessonName))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ReverseMap();

            // Quiz to QuizDto
            CreateMap<Quiz, QuizDto>()
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.ApplicationUser.FullName))
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.QuizName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.PassingScore, opt => opt.MapFrom(src => src.PassingScore))
                .ReverseMap();
            
            CreateMap<Quiz, CreateQuizDto>()
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.QuizName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.PassingScore, opt => opt.MapFrom(src => src.PassingScore))
                .ReverseMap();
            
            CreateMap<Quiz, UpdateQuizDto>()
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.QuizName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.PassingScore, opt => opt.MapFrom(src => src.PassingScore))
                .ReverseMap();
            

            // Question to QuestionDto
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.ApplicationUser.FullName))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Where(a => a.Status != StaticOperationStatus.BaseEntity.Deleted)))
                .ReverseMap();
            
            CreateMap<Question, CreateQuestionDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Question, UpdateQuestionDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ReverseMap();
            
            // Unit to UnitDto
            CreateMap<Unit, UnitDto>()
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.ApplicationUser.FullName))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LearningObjectives, opt => opt.MapFrom(src => src.LearningObjectives))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();
            
            CreateMap<Unit, CreateUnitDto>()
                    .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LearningObjectives, opt => opt.MapFrom(src => src.LearningObjectives))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ReverseMap();
            
            CreateMap<Unit, UpdateUnitDto>()
                .ForMember(dest => dest.UnitId, opt => opt.Ignore())
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.LearningObjectives, opt => opt.MapFrom(src => src.LearningObjectives))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ReverseMap();
            
            // LessonContents to LessonContentDto
            CreateMap<LessonContent, LessonContentDto>()
                .ForMember(dest => dest.LessonContentId, opt => opt.MapFrom(src => src.LessonContentId))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
                .ForMember(dest => dest.ResourceUrl, opt => opt.MapFrom(src => src.ResourceUrl))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();
            
            CreateMap<LessonContent, CreateLessonContentDto>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
                .ForMember(dest => dest.ResourceUrl, opt => opt.MapFrom(src => src.ResourceUrl))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();
            
            CreateMap<LessonContent, UpdateLessonContentDto>()
                .ForMember(dest => dest.LessonContentId, opt => opt.MapFrom(src => src.LessonContentId))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.ResourceType, opt => opt.MapFrom(src => src.ResourceType))
                .ForMember(dest => dest.ResourceUrl, opt => opt.MapFrom(src => src.ResourceUrl))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            // Answer to AnswerDto
            CreateMap<Answer, AnswerDto>()
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Explanation, opt => opt.MapFrom(src => src.Explanation))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();

            CreateMap<Answer, CreateAnswerDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Explanation, opt => opt.MapFrom(src => src.Explanation))
                .ReverseMap();

            CreateMap<Answer, UpdateAnswerDto>()
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Explanation, opt => opt.MapFrom(src => src.Explanation))
                .ReverseMap();

            // Matrix to MatrixDto
            CreateMap<Matrix, MatrixDto>()
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.SkillFocus, opt => opt.MapFrom(src => src.SkillFocus))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();

            CreateMap<Matrix, CreateMatrixDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.SkillFocus, opt => opt.MapFrom(src => src.SkillFocus))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            CreateMap<Matrix, UpdateMatrixDto>()
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EnglishLevel, opt => opt.MapFrom(src => src.EnglishLevel))
                .ForMember(dest => dest.SkillFocus, opt => opt.MapFrom(src => src.SkillFocus))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            // MatrixDetail to MatrixDetailDto
            CreateMap<MatrixDetail, MatrixDetailDto>()
                .ForMember(dest => dest.DetailId, opt => opt.MapFrom(src => src.DetailId))
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.QuestionCount))
                .ForMember(dest => dest.ScorePerQuestion, opt => opt.MapFrom(src => src.ScorePerQuestion))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();  

            CreateMap<MatrixDetail, CreateMatrixDetailDto>()
                .ForMember(dest => dest.MatrixId, opt => opt.MapFrom(src => src.MatrixId))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.QuestionCount))
                .ForMember(dest => dest.ScorePerQuestion, opt => opt.MapFrom(src => src.ScorePerQuestion))
                .ReverseMap();

            CreateMap<MatrixDetail, UpdateMatrixDetailDto>()
                .ForMember(dest => dest.DetailId, opt => opt.MapFrom(src => src.DetailId))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.QuestionCount))
                .ForMember(dest => dest.ScorePerQuestion, opt => opt.MapFrom(src => src.ScorePerQuestion))
                .ReverseMap();

            // StudentAnswer to StudentAnswerDto
            CreateMap<StudentAnswer, StudentAnswerDto>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.AttemptId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.Content))
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.Answer != null ? src.Answer.Content : null))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.TeacherFeedback, opt => opt.MapFrom(src => src.TeacherFeedback))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => src.UpdatedTime))
                .ReverseMap();

            CreateMap<StudentAnswer, CreateStudentAnswerDto>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.AttemptId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.TeacherFeedback, opt => opt.MapFrom(src => src.TeacherFeedback))
                .ReverseMap();

            CreateMap<StudentAnswer, UpdateStudentAnswerDto>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.AttemptId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.TeacherFeedback, opt => opt.MapFrom(src => src.TeacherFeedback))
                .ReverseMap();


        }
    }
}
