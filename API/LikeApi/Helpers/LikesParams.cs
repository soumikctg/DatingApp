namespace LikeApi.Helpers;

public class LikesParams : PaginationParams
{
    public string Username { get; set; }
    public string Predicate { get; set; }
}