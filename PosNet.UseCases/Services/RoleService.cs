using PosNet.Domain.Interfaces;
using PosNet.UseCases.Dtos.Roles;
using PosNet.UseCases.Interfaces;

namespace PosNet.UseCases.Services
{
    public class RoleService(
        IUnitOfWork unitOfWork
    ) : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<RoleDto>>> All()
        {
            var result = await _unitOfWork.Role.AllRoles();

            var roles = result.Select(item => RoleDto.FromModel(item));

            return Result<IEnumerable<RoleDto>>.Ok(roles);
        }
    }
}
