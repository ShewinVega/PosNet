using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Dtos.Pagination;

namespace PosNet.UseCases.Interfaces
{
    public interface IUserService
    {
        public Task<PaginationResponseDto<UserDto>> GetUsersPaginated( PaginationDto request);
    }
}
