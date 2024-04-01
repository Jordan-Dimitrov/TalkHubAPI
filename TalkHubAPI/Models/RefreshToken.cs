using System.ComponentModel.DataAnnotations;

namespace TalkHubAPI.Models;

public partial class RefreshToken
{
    public int Id { get; set; }
    [Required]
    public string Token { get; set; } = null!;
    [Required]
    public DateTime TokenCreated { get; set; }
    [Required]
    public DateTime TokenExpires { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
