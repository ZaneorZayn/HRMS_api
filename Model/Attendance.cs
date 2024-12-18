﻿using HRMS_api.Enum;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HRMS_api.Model
{
    namespace EmployeeManagementAPI.Models
    {
        public class Attendance
        {
            [Key]
            [Required]
            public int AttendanceId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan? CheckInTime { get; set; }
            public TimeSpan? CheckOutTime { get; set; }

            //[JsonConverter(typeof(StringEnumConverter))]
            public Status Status { get; set; } // Enum: Present, Absent, etc.

            public Employee Employee { get; set; }
        }
    }

}
