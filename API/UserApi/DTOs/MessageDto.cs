using UserAPI.Entities;

namespace UserAPI.DTOs;

public class MessageDto
{
    public string Id { get; set; }
    public string SenderUserName { get; set; }
    public string SenderPhotoUrl { get; set; }
    public string RecipientUserName { get; set; }
    public string RecipientPhotoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; }
}