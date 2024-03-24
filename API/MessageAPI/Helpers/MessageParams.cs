using System.ComponentModel.DataAnnotations;

namespace MessageAPI.Helpers;

public class MessageParams : PaginationParams
{
    [Required]
    public string Username { get; set; }
    public string Container { get; set; } = "Unread";
}