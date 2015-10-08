using System;
using System.Transactions;
using Common.Services.Contract;

namespace Common.Services
{
    /// <summary>
    /// Сервис транзакции
    /// </summary>
    public class TransactionService : ITransactionService, IDisposable
    {
        private static readonly TransactionOptions options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.MaximumTimeout
        };

        private TransactionScope transaction;

        /// <summary>
        /// Начать транзакцию
        /// </summary>
        public void Begin()
        {
            transaction = new TransactionScope(
                TransactionScopeOption.Required,
                options,
                TransactionScopeAsyncFlowOption.Enabled
                );
        }

        /// <summary>
        /// Завершить транзакцию
        /// </summary>
        public void Complete()
        {
            transaction.Complete();
            transaction.Dispose();
        }

        /// <summary>
        /// Отменить транзакцию
        /// </summary>
        public void Rollback()
        {
            transaction.Dispose();
        }

        public void Dispose()
        {
            if (transaction == null) return;
            transaction.Dispose();
        }
    }
}