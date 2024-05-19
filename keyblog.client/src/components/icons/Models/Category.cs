namespace KeyBlog.Data.Models;

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ParentId { get; set; }
    public bool Visible { get; set; } = true;
}
