using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;
using System.Net.Http.Json;

namespace StudentScheduleBackend
{
    public static class Exporter
    {
        public static void CreateBackups(Context context, string path)
        {
            string fullPath = Path.GetFullPath(path);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            //this should be autmated but i dont care lol
            var ar = new Repository<Account>(context);
            ExportTable(ar.GetAll(), "accounts.json");

            var cr = new Repository<Class>(context);
            ExportTable(cr.GetAll(), "classes.json");

            var clr = new Repository<Classroom>(context);
            ExportTable(clr.GetAll(), "classrooms.json");

            var st = new Repository<Student>(context);
            ExportTable(st.GetAll(), "students.json");

            var pr = new Repository<Program>(context);
            ExportTable(pr.GetAll(), "programs.json");

            var spr = new Repository<StudentProgram>(context);
            ExportTable(spr.GetAll(), "studentPrograms.json");

            var sr = new Repository<Subject>(context);
            ExportTable(sr.GetAll(), "subjects.json");


            // Helper local function to serialize & write
            void ExportTable<T>(IEnumerable<T> data, string fileName)
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                File.WriteAllText(Path.Combine(fullPath, fileName), json);
            }
        }
    }
}
