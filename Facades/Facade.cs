using System;
using System.Threading.Tasks;
using Common.Facades.Contract;
using Common.Models.Dto;
using Common.Models.Filters;
using Common.Services.Contract;
using Common.Utilites;

namespace Common.Facades
{
    public class Facade<TEntity, TFilter, TReadService, TEditService> : IFacade<TEntity, TFilter>
        where TEntity : class
        where TFilter : BaseFilter
        where TReadService : IReadService<TEntity, TFilter>
        where TEditService : IEditService<TEntity>
    {
        private readonly Lazy<TReadService> read;
        private readonly Lazy<TEditService> edit;
        private readonly Lazy<ITransactionService> transaction;

        public Facade(Lazy<TReadService> read, Lazy<TEditService> edit, Lazy<ITransactionService> transaction)
        {
            this.read = read;
            this.edit = edit;
            this.transaction = transaction;
        }

        public async Task<TEntity> Get(params object[] key)
        {
            return await read.Value.GetAsync(key);
        }

        public async Task<ResultDto<TEntity>> Get(TFilter filter)
        {
            return await read.Value.GetAsync(filter);
        }

        public async Task<bool> Exist(TFilter filter)
        {
            return await read.Value.ExistAsync(filter);
        }

        public async Task<long> Count(TFilter filter)
        {
            return await read.Value.CountAsync(filter);
        }

        public async Task Add(TEntity entity)
        {
            transaction.Value.Begin();
            try
            {
                await edit.Value.AddAsync(entity);
                await edit.Value.CommitAsync();
                transaction.Value.Commit();
            }
            catch (Exception)
            {
                transaction.Value.Rollback();
                throw;
            }
            ObjectUtilites.CleanPropertyByType(entity, typeof(byte[]));
        }

        public async Task Update(TEntity currEntity, TEntity prevEntity)
        {
            transaction.Value.Begin();
            try
            {
                await edit.Value.UpdateAsync(currEntity, prevEntity);
                await edit.Value.CommitAsync();
                transaction.Value.Commit();
            }
            catch (Exception)
            {
                transaction.Value.Rollback();
                ObjectUtilites.CleanPropertyByType(prevEntity, typeof(byte[]));
                throw;
            }
            ObjectUtilites.CleanPropertyByType(currEntity, typeof(byte[]));
            ObjectUtilites.CleanPropertyByType(prevEntity, typeof(byte[]));
        }

        public async Task Remove(TEntity entity)
        {
            transaction.Value.Begin();
            try
            {
                await edit.Value.RemoveAsync(entity);
                await edit.Value.CommitAsync();
                transaction.Value.Commit();
            }
            catch (Exception)
            {
                transaction.Value.Rollback();
                throw;
            }
            ObjectUtilites.CleanPropertyByType(entity, typeof(byte[]));
        }
    }
}