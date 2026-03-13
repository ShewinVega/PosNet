using PosNet.Domain.Interfaces;
using PosNet.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNet.Infrastructure.Repositories
{
    public class RoleRepository(AppDbContext appDbContext) : IRoleRepository
    {
        private readonly AppDbContext _context =  appDbContext;

        public async Task<IEnumerable<Role>> AllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetById(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
