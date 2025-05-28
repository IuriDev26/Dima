namespace Dima.Core.Models;

public class Voucher
{
    public long Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => DateTime.Now >= StartDate && 
                            DateTime.Now <= EndDate;

    public decimal Amount { get; set; }
}