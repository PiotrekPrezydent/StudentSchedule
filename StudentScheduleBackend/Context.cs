using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend
{
    public class Context : DbContext
    {
#if DEBUG
        internal const string ReadWriteLogin = "ReadWriteUser";
        internal const string ReadWritePass = "adminpass";

        internal const string ReadLogin = "ReadOnlyUser";
        internal const string ReadPass = "readpass";
#else
        internal const string ReadWriteLogin = "SOMEENCRYPTEDVALUEFROMSERVER";
        internal const string ReadWritePass = "SOMEENCRYPTEDVALUEFROMSERVER";

        internal const string ReadLogin = "SOMEENCRYPTEDVALUEFROMSERVER";
        internal const string ReadPass = "SOMEENCRYPTEDVALUEFROMSERVER";
#endif

        public static Context Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Context is not initialized. Call Initialize(path) first.");
                return _instance;
            }
        }
        static Context? _instance;
        
        public Context(DbContextOptions<Context> options) : base(options) { }

        //lock singleton from multi-thearding creations of same object
        static readonly object _lock = new();

        //internal is used only for seeder and i should fix that, but i dont care lol
        internal DbSet<Student> _students { get; set; }
        internal DbSet<Account> _accounts { get; set; }
        internal DbSet<Program> _programs { get; set; }
        internal DbSet<StudentProgram> _studentPrograms { get; set; }
        internal DbSet<Class> _classes { get; set; }
        internal DbSet<Classroom> _classrooms { get; set; }
        internal DbSet<Subject> _subjects { get; set; }

        public IReadOnlyList<Entity> GetEntitiesByType(Type t)
        {
            if (t == typeof(Student))
                return _students.ToList();

            if (t == typeof(Account))
                return _accounts.ToList();

            if (t == typeof(Program))
                return _programs.ToList();

            if (t == typeof(StudentProgram))
                return _studentPrograms.ToList();

            if (t == typeof(Class))
                return _classes.ToList();

            if (t == typeof(Classroom))
                return _classrooms.ToList();

            if (t == typeof(Subject))
                return _subjects.ToList();

            throw new ArgumentException($"Unsupported entity type: {t.Name}");
        }

        public static Context Initialize(string connectionString)
        {
            if (_instance != null)
                return _instance;

            lock (_lock)
            {
                if (_instance != null)
                    return _instance;

                var optionsBuilder = new DbContextOptionsBuilder<Context>();

                optionsBuilder.UseSqlServer(connectionString);
                _instance = new Context(optionsBuilder.Options);
                return _instance;
            }
        }

        public static string BuildConnectionString(string configPath,string login, string password, out int accountId)
        {
            string jsonString = File.ReadAllText(configPath);
            JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

            string server = root.GetProperty("Server").GetString() ?? "";
            string db = root.GetProperty("Database").GetString() ?? "";
            string trust = root.GetProperty("TrustServerCertificate").GetBoolean().ToString() ?? "";

            string readAccountsConnection = $"Server={server};Database={db};User Id={ReadLogin};Password={ReadPass};TrustServerCertificate={trust}";

            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            optionsBuilder.UseSqlServer(readAccountsConnection);
            var c = new Context(optionsBuilder.Options);

            var account = c._accounts.FirstOrDefault(a => a.Login == login && a.Password == password);

            if (account == null)
                throw new Exception("Login or password incorrect.");

            string finalLogin = account.IsAdmin ? ReadWriteLogin : ReadLogin;
            string finalPass = account.IsAdmin ? ReadWritePass : ReadPass;

            accountId = account.Id;

            return $"Server={server};Database={db};User Id={finalLogin};Password={finalPass};TrustServerCertificate={trust};";
        }

        public void Flush() => _instance = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Account: Login unique
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Login)
                .IsUnique();

            // Student: IndexNumber unique
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.IndexNumber)
                .IsUnique();

            // One-to-one: Student <-> Account
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Account)
                .WithOne(a => a.Student)
                .HasForeignKey<Student>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie konta -> usuwa studenta

            // Many-to-many: Student <-> Program przez StudentProgram
            modelBuilder.Entity<StudentProgram>()
                .HasKey(sp => new { sp.StudentId, sp.ProgramId });

            modelBuilder.Entity<StudentProgram>()
                .HasOne(sp => sp.Student)
                .WithMany(s => s.StudentPrograms)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentProgram>()
                .HasOne(sp => sp.Program)
                .WithMany(p => p.StudentPrograms)
                .HasForeignKey(sp => sp.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Program -> Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Program)
                .WithMany(p => p.Classes)
                .HasForeignKey(c => c.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Subject -> Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Classes)
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Classroom -> Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Classroom)
                .WithMany(cl => cl.Classes)
                .HasForeignKey(c => c.ClassroomId);

            // Configure lengths and required fields (optional but recommended)
            modelBuilder.Entity<Account>()
                .Property(a => a.Login)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Account>()
                .Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Student>()
                .Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Student>()
                .Property(s => s.IndexNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Program>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Subject>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Classroom>()
                .Property(c => c.Building)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Classroom>()
                .Property(c => c.RoomNumber)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<Class>()
                .Property(c => c.Weekday)
                .IsRequired()
                .HasMaxLength(20);
        }

    }
}