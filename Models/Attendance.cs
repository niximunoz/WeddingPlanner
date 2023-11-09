using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeddingPlanner.Models;

public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public int WeddingId { get; set; }
    public User? User { get; set; }
    public Wedding? Wedding { get; set; }
}
