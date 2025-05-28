using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend
{
    public class Context : DbContext
    {
        //Those string should be encrypted in code or be getted from server, but i dont get paid enough to do so...
        const string ReadWriteLogin = "ReadWriteUser";
        const string ReadWritePass = "adminpass";

        const string ReadLogin = "ReadOnlyUser";
        const string ReadPass = "readpass";

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

        internal DbSet<Student> Students { get; set; }
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Program> Programs { get; set; }
        internal DbSet<StudentProgram> StudentPrograms { get; set; }
        internal DbSet<Class> Classes { get; set; }
        internal DbSet<Classroom> Classrooms { get; set; }
        internal DbSet<Subject> Subjects { get; set; }

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

            var account = c.Accounts.FirstOrDefault(a => a.Login == login && a.Password == password);

            if (account == null)
                throw new Exception("Login or password incorrect.");

            string finalLogin = account.IsAdmin ? ReadWriteLogin : ReadLogin;
            string finalPass = account.IsAdmin ? ReadWritePass : ReadPass;

            accountId = account.Id;

            return $"Server={server};Database={db};User Id={finalLogin};Password={finalPass};TrustServerCertificate={trust};";
        }

        public void Flush() => _instance = null;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraints
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.IndexNumber)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Login)
                .IsUnique();

            // One-to-one: Student <-> Account
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Account)
                .WithOne(a => a.Student)
                .HasForeignKey<Student>(s => s.AccountId);

            // Many-to-many: Student <-> Program
            modelBuilder.Entity<StudentProgram>()
                .HasKey(sp => new { sp.StudentId, sp.ProgramId });

            modelBuilder.Entity<StudentProgram>()
                .HasOne(sp => sp.Student)
                .WithMany(s => s.StudentPrograms)
                .HasForeignKey(sp => sp.StudentId);

            modelBuilder.Entity<StudentProgram>()
                .HasOne(sp => sp.Program)
                .WithMany(p => p.StudentPrograms)
                .HasForeignKey(sp => sp.ProgramId);

            // One-to-many: Program -> Classes
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Program)
                .WithMany(p => p.Classes)
                .HasForeignKey(c => c.ProgramId);

            // One-to-many: Subject -> Classes
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Classes)
                .HasForeignKey(c => c.SubjectId);

            // One-to-many: Classroom -> Classes
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Classroom)
                .WithMany(cr => cr.Classes)
                .HasForeignKey(c => c.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}