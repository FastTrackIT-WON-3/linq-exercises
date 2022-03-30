using System;

namespace LinqExercises
{
    public class Student : Person
    {
        public Student(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            Gender gender,
            string universityName)
            : base (firstName, lastName, dateOfBirth, gender)
        {
            UniversityName = universityName;
        }

        public string UniversityName { get; }
    }
}
