using AutoMapper;
using EduSystem.Models.DTO.Authentication;
using EduSystem.Models.DTO.Lesson;
using EduSystem.Models.DTO.Question;
using EduSystem.Models.DTO.Quiz;
using EduSystem.Models.DTO.Student;
using EduSystem.Models.DTOs.Teacher;
using EduSystem.Models.Entities;

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

            //// Quiz to QuizDto
            //CreateMap<Quiz, CreateQuizDto>()
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel))
            //    .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
            //    .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit))
            //    .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.TotalQuestions))
            //    .ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => src.TotalPoints))
            //    .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            //    .ForMember(dest => dest.MaxAttempts, opt => opt.MapFrom(src => src.MaxAttempts))
            //    .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
            //    .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
            //    .ReverseMap();

            //CreateMap<Quiz, UpdateQuizDto>()
            //    .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //    .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel))
            //    .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
            //    .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit))
            //    .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.TotalQuestions))
            //    .ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => src.TotalPoints))
            //    .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            //    .ForMember(dest => dest.MaxAttempts, opt => opt.MapFrom(src => src.MaxAttempts))
            //    .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
            //    .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
            //    .ReverseMap();

            //// Question to QuestionDto
            //CreateMap<Question, CreateQuestionDto>()
            //    .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
            //    .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
            //    .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
            //    .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel))
            //    .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
            //    .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit))
            //    .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
            //    .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
            //    .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
            //    .ReverseMap();

            //CreateMap<Question, UpdateQuestionDto>()
            //    .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            //    .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
            //    .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
            //    .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
            //    .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel))
            //    .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
            //    .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit))
            //    .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
            //    .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
            //    .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId))
            //    .ReverseMap();

        }
    }
}
