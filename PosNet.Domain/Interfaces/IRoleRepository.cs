using PosNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNet.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> AllRoles();
        Task<Role?> GetRoleByName(string name);

        Task<Role?> GetById(int id);
    }
}
