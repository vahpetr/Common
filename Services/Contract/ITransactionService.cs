namespace Common.Services.Contract
{
    /// <summary>
    /// Сервис транзакции
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Начать транзакцию
        /// </summary>
        void Begin();

        /// <summary>
        /// Завершить транзакцию
        /// </summary>
        void Complete();

        /// <summary>
        /// Отменить транзакцию
        /// </summary>
        void Rollback();
    }
}