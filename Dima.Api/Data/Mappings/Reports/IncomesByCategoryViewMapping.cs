using Dima.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Reports;

public class IncomesByCategoryViewMapping : IEntityTypeConfiguration<IncomesByCategory>
{
    public void Configure(EntityTypeBuilder<IncomesByCategory> builder)
    => builder.HasNoKey().ToView("VIEW_INCOMES_BY_CATEGORY");
}