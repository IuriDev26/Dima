using Dima.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Reports;

public class IncomesAndExpensesViewMapping : IEntityTypeConfiguration<IncomesAndExpenses>
{
    public void Configure(EntityTypeBuilder<IncomesAndExpenses> builder)
    => builder.HasNoKey().ToView("VIEW_INCOMES_AND_EXPENSES");
    
}