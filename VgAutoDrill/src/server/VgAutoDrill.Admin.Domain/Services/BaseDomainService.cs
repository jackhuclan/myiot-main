using SqlSugar;
using System.Linq.Expressions;
using VgAutoDrill.Admin.Domain.Interfaces;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Domain.Services
{
    public class BaseDomainService<T> : IBaseDomainService<T> where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public IBaseRepository<T> _baseRepository;

        public async Task<bool> Add(T model)
        {
            return await _baseRepository.Add(model);
        }

        public async Task<bool> BulkInsert(List<T> list)
        {
            return await _baseRepository.BulkInsert(list);
        }

        public async Task<bool> BulkDelete(List<T> list)
        {
            return await _baseRepository.BulkDelete(list);
        }

        public async Task<int> AddReturnId(T model)
        {
            return await _baseRepository.AddReturnId(model);
        }
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await _baseRepository.DeleteByIds(ids);
        }

        public async Task<bool> DeleteById(object id)
        {
            return await _baseRepository.DeleteById(id);
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            if (await _baseRepository.IsExistAsync(where))
            {
                return await _baseRepository.DeleteAsync(where);
            }
            return true;
        }

        public async Task<T> QueryByID(object objId)
        {
            return await _baseRepository.QueryByID(objId);
        }

        public async Task<bool> Update(T model)
        {
            return await _baseRepository.Update(model);
        }

        public async Task<bool> BulkUpdate(List<T> list)
        {
            return await _baseRepository.BulkUpdate(list);
        }

        public async Task<bool> UpdateAsync(T model, string strWhere)
        {
            return await _baseRepository.UpdateAsync(model, strWhere);
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> strWhere)
        {
            return await _baseRepository.UpdateAsync(columns, strWhere);
        }
        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> where)
        {
            return await _baseRepository.IsExistAsync(where);
        }

        public async Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1, int pageSize = 10)
        {
            return await _baseRepository.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }

        public async Task<IPageList<dynamic>> QueryPageAsync(string sql, string whereSql, object parameters, string orderBy, int pageIndex = 1, int pageSize = 10)
        {
            return await _baseRepository.QueryPageAsync(sql, whereSql, parameters, orderBy, pageIndex, pageSize);
        }

        public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression, OrderByType orderByType)
        {
            return await _baseRepository.QueryAsync(predicate, orderByExpression, orderByType);
        }
        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _baseRepository.FindSingleAsync(predicate);
        }
        public async Task<int> GetMaxSort<T>()
        {
            return await _baseRepository.GetMaxSort<T>();
        }
    }
}
