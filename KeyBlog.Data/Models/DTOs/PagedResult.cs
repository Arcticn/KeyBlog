namespace KeyBlog.Data.Models.DTOs;

public class PagedResult<T>
{
    public List<T>? Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
