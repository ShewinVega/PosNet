using PosNet.UseCases.Dtos.Roles;

namespace PosNet.UseCases.Interfaces
{
    public interface IRoleService
    {
        Task<Result<IEnumerable<RoleDto>>> All();
    }
}
