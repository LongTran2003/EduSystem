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
        }

        public static class Student
        {
            public const string Invalid = "Invalid student information provided";
            public const string NotFound = "No student found";
            public const string Found = "Student found";
            public const string Retrieved = "Student information retrieved successfully";
            public const string NotRetrieved = "Student information cannot retrieve";
            public const string NotExisted = "Student is not exist!";

        }

        public static class Teacher
        {
            public const string Invalid = "Invalid teacher information provided";
            public const string NotFound = "No teacher found";
            public const string Found = "Teacher found";
            public const string Retrieved = "Teacher information retrieved successfully";
            public const string NotExisted = "Teacher is not exist!";

        }
        
        public static class Subject
        {
            public const string Created = "Subject(s) created successfully";
            public const string NotCreated = "Subject(s) cannot create beacause: ";
            public const string Updated = "Subject updated successfully";
            public const string NotUpdated = "Subject cannot update";
            public const string NotFound = "No subject found";
            public const string Found = "Subject found";
            public const string Retrieved = "Subject information retrieved successfully";
            public const string NotRetrieved = "Subject information cannot retrieved";
            public const string Deleted = "Subject deleted successfully";
            public const string NotDeleted = "Subject cannot delete";

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
            public const string Updated = "Quiz(zes) updated successfully";
            public const string NotUpdated = "Quiz cannot update";
            public const string NotFound = "Quiz not found";
            public const string Found = "Quiz found";
            public const string Retrieved = "Quiz information retrieved successfully";
            public const string NotRetrieved = "Quiz information cannot retrieved";
            public const string Deleted = "Quiz(zes) deleted successfully";
            public const string NotDeleted = "Quiz cannot delete";
        }
    }
}
