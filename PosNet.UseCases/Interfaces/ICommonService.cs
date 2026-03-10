using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNet.UseCases.Interfaces
{
    public interface ICommonService<T, Tinsert, Tupdate>
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        Task<T> Create(Tinsert request);
        Task<bool> Update(Tupdate request);
        Task<bool> Delete(int id);
    }
}
