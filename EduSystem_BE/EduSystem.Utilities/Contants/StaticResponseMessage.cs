namespace EduSystem.Utilities.Contants
{
    public class StaticResponseMessage
    {
        public static class User
        {
            public const string Created = "User created successfully";
            public const string Deleted = "User deleted successfully";
            public const string RetrievedAll = "All users retrieved successfully";
            public const string Retrieved = "User retrieved successfully";
            public const string Updated = "User updated successfully";
            public const string NotFound = "User not found";
            public const string NotCreated = "Failed to create user";
            public const string NotDeleted = "Failed to delete user";
            public const string NotUpdated = "Failed to update user";
            public const string NotRetrieved = "Failed to retrieve user(s)";
            public const string NotAuthorized = "User not authorized";
            public const string UnAuthorized = "User not authorized for this function";
        }

        public static class Student
        {
            public const string Invalid = "Invalid student information provided";
            public const string NotFound = "No student found";
            public const string Found = "Student found";
            public const string Retrieved = "Student information retrieved successfully";
            public const string NotRetrieved = "Student information cannot retrieve";
            public const string NotExisted = "Student is not exist!";
            public const string Updated = "Student updated successfully";
        }

        public static class Teacher
        {
            public const string Invalid = "Invalid teacher information provided";
            public const string NotFound = "No teacher found";
            public const string Found = "Teacher found";
            public const string Retrieved = "Teacher information retrieved successfully";
            public const string NotExisted = "Teacher is not exist!";
            public const string Updated = "Teacher updated successfully";

        }
        
        public static class LessonContent
        {
            public const string Created = "Lesson Content(s) created successfully";
            public const string NotCreated = "Lesson Content(s) cannot create beacause: ";
            public const string Updated = "Lesson Content updated successfully";
            public const string NotUpdated = "Lesson Content cannot update";
            public const string NotFound = "No Lesson Content found";
            public const string Found = "Lesson Content found";
            public const string Retrieved = "Lesson Content information retrieved successfully";
            public const string NotRetrieved = "Lesson Content information cannot retrieved";
            public const string Deleted = "Lesson Content deleted successfully";
            public const string NotDeleted = "Lesson Content cannot delete";

        }

        public static class Lesson
        {
            public const string Created = "Lesson(s) created successfully";
            public const string NotCreated = "Lesson(s) cannot create beacause: ";
            public const string Updated = "Lesson(s) updated successfully";
            public const string NotUpdated = "Lesson cannot update";
            public const string NotFound = "Lesson not found";
            public const string Found = "Lesson found";
            public const string Retrieved = "Lesson information retrieved successfully";
            public const string NotRetrieved = "Lesson information cannot retrieved";
            public const string Deleted = "Lesson(s) deleted successfully";
            public const string NotDeleted = "Lesson cannot delete";
        }
        
        public static class Quiz
        {
            public const string Created = "Quiz(zes) created successfully";
            public const string NotCreated = "Quiz(zes) cannot create beacause: ";
            public const string AlreadyExist = "Quiz already exist: ";
            public const string Updated = "Quiz(zes) updated successfully";
            public const string NotUpdated = "Quiz cannot update";
            public const string NotFound = "Quiz not found";
            public const string Found = "Quiz found";
            public const string Retrieved = "Quiz information retrieved successfully";
            public const string NotRetrieved = "Quiz information cannot retrieved";
            public const string Deleted = "Quiz(zes) deleted successfully";
            public const string NotDeleted = "Quiz cannot delete";
        }
        
        public static class Question
        {
            public const string Created = "Question(s) created successfully";
            public const string NotCreated = "Question(s) cannot create beacause: ";
            public const string AlreadyExist = "Question already exist: ";
            public const string Updated = "Question(s) updated successfully";
            public const string NotUpdated = "Question cannot update";
            public const string NotFound = "Question not found";
            public const string Found = "Question found";
            public const string Retrieved = "Question information retrieved successfully";
            public const string NotRetrieved = "Question information cannot retrieved";
            public const string Deleted = "Question(s) deleted successfully";
            public const string NotDeleted = "Question cannot delete";
        }
        
        public static class Unit
        {
            public const string Created = "Unit(s) created successfully";
            public const string NotCreated = "Unit(s) cannot create beacause: ";
            public const string AlreadyExist = "Unit(s) already exist ";
            public const string Updated = "Unit(s) updated successfully";
            public const string NotUpdated = "Unit cannot update";
            public const string NotFound = "Unit not found";
            public const string Found = "Unit found";
            public const string Retrieved = "Unit information retrieved successfully";
            public const string NotRetrieved = "Unit information cannot retrieved";
            public const string Deleted = "Unit(s) deleted successfully";
            public const string NotDeleted = "Unit cannot delete";
        }

        public static class Answer
        {
            public const string Created = "Answer(s) created successfully";
            public const string NotCreated = "Answer(s) cannot create beacause: ";
            public const string AlreadyExist = "Answer(s) already exist ";
            public const string Updated = "Answer(s) updated successfully";
            public const string NotUpdated = "Answer cannot update";
            public const string NotFound = "Answer not found";
            public const string Found = "Answer found";
            public const string Retrieved = "Answer information retrieved successfully";
            public const string NotRetrieved = "Answer information cannot retrieved";
            public const string Deleted = "Answer(s) deleted successfully";
            public const string NotDeleted = "Answer cannot delete";
        }
    }
}
