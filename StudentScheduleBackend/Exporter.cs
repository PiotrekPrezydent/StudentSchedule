using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StudentScheduleBackend.Repositories;

namespace StudentScheduleBackend
{
    public static class Exporter
    {
        public static void CreateBackups(Context context, string path)
        {
            string fullPath = Path.GetFullPath(path);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            AccountRepository accountRepository = new(context);
            ClassRepository classRepository = new(context);
            ClassroomRepository classroomRepository = new(context);
            ProgramRepository programRepository = new(context);
            StudentProgramRepository studentProgramRepository = new(context);
            StudentRepository studentRepository = new(context);
            SubjectRepository subjectRepository = new(context);

            // Export all tables
            ExportTable(accountRepository.GetAll(), "accounts.json");
            ExportTable(classRepository.GetAll(), "classes.json");
            ExportTable(classroomRepository.GetAll(), "classrooms.json");
            ExportTable(programRepository.GetAll(), "programs.json");
            ExportTable(studentProgramRepository.GetAll(), "student_programs.json");
            ExportTable(studentRepository.GetAll(), "students.json");
            ExportTable(subjectRepository.GetAll(), "subjects.json");

            // Helper local function to serialize & write
            void ExportTable<T>(IEnumerable<T> data, string fileName)
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(fullPath, fileName), json);
            }

        }


    }
}
