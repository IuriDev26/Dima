using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ReportHandler(AppDbContext context) : IReportHandler
{
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        try
        {
            var incomesAndExpensesList = await context.IncomesAndExpenses
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return new Response<List<IncomesAndExpenses>?>(incomesAndExpensesList, 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.StackTrace);
            return new Response<List<IncomesAndExpenses>?>(null, 500, "Erro ao obter entradas e saídas");
        }
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
    {
        try
        {
            var incomesByCategoryList = await context.IncomesByCategories
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();
            
            return new Response<List<IncomesByCategory>?>(incomesByCategoryList, 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.StackTrace);
            return new Response<List<IncomesByCategory>?>(null, 500, "Erro ao obter receitas por categoria");
        }
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
    {
        try
        {
            var expensesByCategoryList = await context.ExpensesByCategories
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();
            
            return new Response<List<ExpensesByCategory>?>(expensesByCategoryList, 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.StackTrace);
            return new Response<List<ExpensesByCategory>?>(null, 500, "Erro ao obter despesas por categoria");
        }
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        try
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var data = await context.Transactions.AsNoTracking()
                .Where(x => x.UserId == request.UserId &&
                            x.PaidOrReceivedAt >= startDate &&
                            x.PaidOrReceivedAt <= DateTime.Now)
                .GroupBy(x => 1)
                .Select(x => new FinancialSummary(request.UserId,
                    x.Where(z => z.Type == ETransactionType.Deposit).Sum(y => y.Amount),
                    x.Where(z => z.Type == ETransactionType.Withdraw).Sum(y => y.Amount)))
                .FirstOrDefaultAsync();

            return data is null
                ? new Response<FinancialSummary?>(new FinancialSummary(request.UserId, 0, 0), 200)
                : new Response<FinancialSummary?>(data, 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + ex.StackTrace);
            return new Response<FinancialSummary?>(null, 500, "Erro ao obter Resumo Financeiro");
        }
    }
}