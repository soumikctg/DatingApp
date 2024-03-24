namespace UserAPI.Helpers;

public class PaginationHeader
{
    public PaginationHeader(int currentPage, int itemsPerPage, long totalItems, int totalPages)
    {
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }

    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public long TotalItems { get; set; }
    public int TotalPages { get; set; }
}