using System.Threading.Tasks;

namespace Common.Services.Contract
{
    /// <summary>
    /// ��������� ������� �������������� ������
    /// </summary>
    /// <typeparam name="TEntity">���������</typeparam>
    public interface IEditService<in TEntity>
        where TEntity : class
    {
        /// <summary>
        /// �������� ���� ��������
        /// </summary>
        /// <param name="entity">������ ���� ��������</param>
        void Add(TEntity entity);

        /// <summary>
        /// ���������� �������� ���� ��������
        /// </summary>
        /// <param name="entity">������ ���� ��������</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// �������� ���� ��������
        /// </summary>
        /// <param name="currEntity">����������� ��������</param>
        /// <param name="prevEntity">�������� �� ���� ������</param>
        void Update(TEntity currEntity, TEntity prevEntity);

        /// <summary>
        /// ���������� �������� ���� ��������
        /// </summary>
        /// <param name="currEntity">����������� ��������</param>
        /// <param name="prevEntity">�������� �� ���� ������</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity currEntity, TEntity prevEntity);

        /// <summary>
        /// ������� ��������
        /// </summary>
        /// <param name="entity">��������</param>
        void Remove(TEntity entity);

        /// <summary>
        /// ���������� ������� ��������
        /// </summary>
        /// <param name="entity">��������</param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// ��������� ��� ���������
        /// </summary>
        /// <returns>���������� ��������� ����� � ����</returns>
        int Commit();

        /// <summary>
        /// ���������� ��������� ��� ���������
        /// </summary>
        /// <returns>���������� ��������� ����� � ����</returns>
        Task<int> CommitAsync();
    }
}