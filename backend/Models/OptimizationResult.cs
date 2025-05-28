using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class OptimizationResult
    {
        [Key]
        public int Id { get; set; }
        public int Cars { get; set; }
        public int Trucks { get; set; }
        public double Revenue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
