namespace Common.Services.Contract
{
    public interface ITransactionService
    {
        /// <summary>
        /// Начать транзакцию
        /// </summary>
        void Begin();

        /// <summary>
        /// Завершить транзакцию
        /// </summary>
        void Commit();

        /// <summary>
        /// Отменить транзакцию
        /// </summary>
        void Rollback();
    }
}