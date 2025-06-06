namespace Dima.Core.Models;

public class Category : Base
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<Transaction> Transactions { get; set; } = new();
}