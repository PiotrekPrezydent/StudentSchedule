//using System.Text.Json;
//using Microsoft.EntityFrameworkCore;
//using StudentScheduleBackend.Entities;

//namespace StudentScheduleBackend
//{
//    public static class Seeder
//    {
//        public static void SeedAllFromJson(Context context, string folderPath)
//        {
//            context.Database.EnsureCreated();
//            SeedAccounts(context, Path.Combine(folderPath, "accounts.json"));
//            SeedStudents(context, Path.Combine(folderPath, "students.json"));
//            SeedPrograms(context, Path.Combine(folderPath, "programs.json"));
//            SeedStudentPrograms(context, Path.Combine(folderPath, "student_programs.json"));
//            SeedSubjects(context, Path.Combine(folderPath, "subjects.json"));
//            SeedClassrooms(context, Path.Combine(folderPath, "classrooms.json"));
//            SeedClasses(context, Path.Combine(folderPath, "classes.json"));

//            //login and pass for database connection that will be used for seeded database
//            context.Database.ExecuteSqlRaw($@"
//                    IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{Context.ReadWriteLogin}')
//                    BEGIN
//                        CREATE LOGIN [{Context.ReadWriteLogin}] WITH PASSWORD = '{Context.ReadWritePass}';
//                    END

//                    IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{Context.ReadLogin}')
//                    BEGIN
//                        CREATE LOGIN {Context.ReadLogin} WITH PASSWORD = '{Context.ReadPass}';
//                    END
//                ");

//            context.Database.ExecuteSqlRaw($@"
//                    USE StudentSchedule;

//                    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{Context.ReadWriteLogin}')
//                    BEGIN
//                        CREATE USER [{Context.ReadWriteLogin}] FOR LOGIN [{Context.ReadWriteLogin}];
//                        EXEC sp_addrolemember N'db_owner', N'{Context.ReadWriteLogin}';
//                    END

//                    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '{Context.ReadLogin}')
//                    BEGIN
//                        CREATE USER [{Context.ReadLogin}] FOR LOGIN [{Context.ReadLogin}];
//                        EXEC sp_addrolemember N'db_datareader', N'{Context.ReadLogin}';
//                    END
//                ");
//        }

//        static void SeedPrograms(Context context, string path)
//        {
//            if (!context._programs.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Program>>(jsonString);

//                foreach (var entity in data)
//                    entity.Id = 0;

//                if (data != null)
//                {
//                    context._programs.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedStudents(Context context, string path)
//        {
//            if (!context._students.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Student>>(jsonString);

//                foreach (var entity in data)
//                    entity.Id = 0;

//                if (data != null)
//                {
//                    context._students.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedAccounts(Context context, string path)
//        {
//            if (!context._accounts.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Account>>(jsonString);

//                //foreach (var entity in data)
//                //    entity.Id = 0;

//                if (data != null)
//                {
//                    context._accounts.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedStudentPrograms(Context context, string path)
//        {
//            if (!context._studentPrograms.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<StudentProgram>>(jsonString);

//                if (data != null)
//                {
//                    context._studentPrograms.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedSubjects(Context context, string path)
//        {
//            if (!context._subjects.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Subject>>(jsonString);

//                foreach (var entity in data)
//                    entity.Id = 0;

//                if (data != null)
//                {
//                    context._subjects.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedClassrooms(Context context, string path)
//        {
//            if (!context._classrooms.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Classroom>>(jsonString);

//                foreach (var entity in data)
//                    entity.Id = 0;

//                if (data != null)
//                {
//                    context._classrooms.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }

//        static void SeedClasses(Context context, string path)
//        {
//            if (!context._classes.Any() && File.Exists(path))
//            {
//                var jsonString = File.ReadAllText(path);
//                var data = JsonSerializer.Deserialize<List<Class>>(jsonString);

//                                foreach (var entity in data)
//                    entity.Id = 0;

//                if (data != null)
//                {
//                    context._classes.AddRange(data);
//                    context.SaveChanges();
//                }
//            }
//        }
//    }
//}
