using HelloCMS.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloCMS.Core.Repositories
{
    public interface IGenRepo<T> where T : CoreModel
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
