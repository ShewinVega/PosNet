using Mapster;
using PosNet.Domain.Interfaces;
using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Dtos.Pagination;
using PosNet.UseCases.Interfaces;

namespace PosNet.UseCases.Services
{
    public class UserService(
        IUnitOfWork unitOfWork
     ) : IUserService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginationResponseDto<UserDto>> GetUsersPaginated(PaginationDto request)
        {
            var query = _unitOfWork.User.GetAllUsersAsIQueryable();

            var queryDto = query.ProjectToType<UserDto>();

            var result  = await PagedList<UserDto>.CreateAsync(queryDto, request.Page, request.PageSize );

            return new PaginationResponseDto<UserDto>
            {
                Success = true,
                CurrentPage = result.Page,
                PageSize = result.PageSize,
                RowsCount = result.TotalCount,
                HasPreviousPage = result.HasPreviousPage,
                HasNextPage = result.HasNextPage,
                Data = result.Items
            };

        }
    }
}
