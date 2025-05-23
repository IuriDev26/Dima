CREATE OR ALTER VIEW VIEW_EXPENSES_BY_CATEGORY AS (
SELECT
    Transactions.UserId,
    Categories.Title AS Category,
    Year(Transactions.PaidOrReceivedAt) as Year,
    SUM(Transactions.Amount) AS Expenses
FROM TRANSACTIONS
INNER JOIN CATEGORIES
    ON CATEGORIES.ID = TRANSACTIONS.CATEGORYID
WHERE TRANSACTIONS.PAIDORRECEIVEDAT 
    BETWEEN DATEADD(MONTH, -11, CAST(GETDATE() AS DATE)) 
    AND DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
    AND Transactions.Type = 2
group by Transactions.UserId, Categories.Title, Transactions.PaidOrReceivedAt)