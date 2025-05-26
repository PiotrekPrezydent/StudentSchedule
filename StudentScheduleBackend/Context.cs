using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend
{
    public class Context : DbContext
    {
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

        Context(DbContextOptions<Context> options) : base(options) { }

        static IConfiguration? _configuration = null;
        //lock singleton from multi-thearding creations of same object
        static readonly object _lock = new();

        internal DbSet<Student> Students { get; set; }
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Program> Programs { get; set; }
        internal DbSet<StudentProgram> StudentPrograms { get; set; }
        internal DbSet<Class> Classes { get; set; }
        internal DbSet<Classroom> Classrooms { get; set; }
        internal DbSet<Subject> Subjects { get; set; }

        public static Context Initialize(string? configPath)
        {
            if (_instance != null)
                return _instance;

            lock (_lock)
            {
                if (_instance != null)
                    return _instance;

                var optionsBuilder = new DbContextOptionsBuilder<Context>();
                var connectionString = "";

                if (configPath != null)
                {
                    _configuration = new ConfigurationBuilder()
                    .AddJsonFile(path: configPath, optional: false, reloadOnChange: true)
                    .Build();
                    connectionString = _configuration.GetConnectionString("DefaultConnection");
                }
                else
                {
                    connectionString = "Server=localhost\\SQLEXPRESS;Database=StudentSchedule;Trusted_Connection=True;TrustServerCertificate=True;";
                }

                optionsBuilder.UseSqlServer(connectionString);
                _instance = new Context(optionsBuilder.Options);
                return _instance;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
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
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Student)
                .WithOne(s => s.Account)
                .HasForeignKey<Account>(a => a.StudentId);

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