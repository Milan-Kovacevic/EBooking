using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.Domain.DAOs
{
    public interface IGenericDAO<T, ID>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(ID id);
        Task<T> Insert(T entity);
        Task Update(T entity);
        Task<bool> Delete(ID id);
    }
}
