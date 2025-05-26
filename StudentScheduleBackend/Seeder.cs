using System.Text.Json;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend
{
    public static class Seeder
    {
        public static void SeedAllFromJson(Context context, string folderPath)
        {
            context.Database.EnsureCreated();

            SeedPrograms(context, Path.Combine(folderPath, "programs.json"));
            SeedStudents(context, Path.Combine(folderPath, "students.json"));
            SeedAccounts(context, Path.Combine(folderPath, "accounts.json"));
            SeedStudentPrograms(context, Path.Combine(folderPath, "student_programs.json"));
            SeedSubjects(context, Path.Combine(folderPath, "subjects.json"));
            SeedClassrooms(context, Path.Combine(folderPath, "classrooms.json"));
            SeedClasses(context, Path.Combine(folderPath, "classes.json"));
        }

        static void SeedPrograms(Context context, string path)
        {
            if (!context.Programs.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Program>>(jsonString);
                if (data != null)
                {
                    context.Programs.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedStudents(Context context, string path)
        {
            if (!context.Students.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Student>>(jsonString);
                if (data != null)
                {
                    context.Students.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedAccounts(Context context, string path)
        {
            if (!context.Accounts.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Account>>(jsonString);
                if (data != null)
                {
                    context.Accounts.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedStudentPrograms(Context context, string path)
        {
            if (!context.StudentPrograms.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<StudentProgram>>(jsonString);
                if (data != null)
                {
                    context.StudentPrograms.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedSubjects(Context context, string path)
        {
            if (!context.Subjects.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Subject>>(jsonString);
                if (data != null)
                {
                    context.Subjects.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedClassrooms(Context context, string path)
        {
            if (!context.Classrooms.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Classroom>>(jsonString);
                if (data != null)
                {
                    context.Classrooms.AddRange(data);
                    context.SaveChanges();
                }
            }
        }

        static void SeedClasses(Context context, string path)
        {
            if (!context.Classes.Any() && File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<List<Class>>(jsonString);
                if (data != null)
                {
                    context.Classes.AddRange(data);
                    context.SaveChanges();
                }
            }
        }
    }
}
