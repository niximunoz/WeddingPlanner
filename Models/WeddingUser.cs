#pragma warning disable CS8618
using WeddingPlanner.Models;

namespace WeddingPlanner.Models;
public class WeddingUser
{
    public Wedding? Wedding { get; set; }
    public User? Creator { get; set; }

    public List<User> ListAsist { get; set; } 
}