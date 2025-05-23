CREATE OR ALTER VIEW VIEW_INCOMES_AND_EXPENSES AS (
SELECT
    Transactions.UserId,
    MONTH(Transactions.PaidOrReceivedAt) AS Month,
    YEAR(Transactions.PaidOrReceivedAt) AS Year,
    SUM(CASE WHEN Transactions.Type = 1 THEN Transactions.Amount ELSE 0 END ) AS Incomes,
    SUM(CASE WHEN Transactions.Type = 2 THEN Transactions.Amount ELSE 0 END ) AS Expenses
FROM Transactions
WHERE Transactions.PaidOrReceivedAt
    BETWEEN DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
    AND DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
GROUP BY Transactions.UserId,
    MONTH(Transactions.PaidOrReceivedAt),
    YEAR(Transactions.PaidOrReceivedAt))
