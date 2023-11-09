#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required]
    public string WedderOne { get; set; }
    [Required]
    public string WedderTwo { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    public string Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? Creator { get; set; }

    public List<Attendance>  Assistants { get; set; } = new List<Attendance>();
}