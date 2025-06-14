﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using StudentScheduleBackend.Attributes;

namespace StudentScheduleBackend.Entities
{
    [Table("StudentPrograms")]
    public class StudentProgram : Entity
    {
        [ForeignKeyOf(typeof(Student))]
        public int StudentId { get; set; }

        [ForeignKeyOf(typeof(Program))]
        public int ProgramId { get; set; }

        [JsonIgnore]
        public Student? Student { get; set; }

        [JsonIgnore]
        public Program? Program { get; set; }

        public StudentProgram() { }

        public StudentProgram(int studentId, int programId)
        {
            StudentId = studentId;
            ProgramId = programId;
        }
    }
}