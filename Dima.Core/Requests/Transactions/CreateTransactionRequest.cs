using System.ComponentModel.DataAnnotations;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class CreateTransactionRequest : Request
{
    [Required(ErrorMessage = "Título é obrigatório")]
    public string Title { get; set; } = string.Empty;

    public DateTime? PaidOrReceivedAt { get; set; } = null;

    [Required(ErrorMessage = "Tipo da transação é obrigatório")]
    public ETransactionType Type { get; set; }

    [Required(ErrorMessage = "Valor é obrigatório")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public long CategoryId { get; set; }
}