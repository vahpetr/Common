using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repositories.Contract
{
    /// <summary>
    /// ��������� ��������� �������������� ������
    /// </summary>
    /// <typeparam name="TEntity">���������</typeparam>
    public interface IEditRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// �������� ���� ��������
        /// </summary>
        /// <param name="entity">������ ���� ��������</param>
        void Add(TEntity entity);

        /// <summary>
        /// �������� �������� ��������� ��� ���������
        /// </summary>
        /// <param name="entity">��������</param>
        /// <param name="expressions">��������</param>
        void Modified(TEntity entity, params Expression<Func<TEntity, object>>[] expressions);

        /// <summary>
        /// �������� ���� ��������
        /// </summary>
        /// <param name="entity">����������� ���� ��������</param>
        void Update(TEntity entity);

        /// <summary>
        /// ������� ��������
        /// </summary>
        /// <param name="entity">��������</param>
        void Remove(TEntity entity);

        /// <summary>
        /// ��������� ��� ���������
        /// </summary>
        /// <returns>���������� ��������� ����� � ����</returns>
        int SaveChanges();

        /// <summary>
        /// ���������� ��������� ��� ���������
        /// </summary>
        /// <returns>���������� ��������� ����� � ����</returns>
        Task<int> SaveChangesAsync();
    }
}