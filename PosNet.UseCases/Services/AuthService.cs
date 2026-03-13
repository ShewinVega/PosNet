

using FluentValidation.Results;
using PosNet.Domain.Interfaces;
using PosNet.UseCases.Dtos.Auth;
using PosNet.UseCases.Interfaces;
using PosNet.UseCases.Validators.User;

namespace PosNet.UseCases.Services
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IPasswordEncrypt hasher,
        IHandleBusinessError handleError,
        IPasswordEncrypt passwordEncrypt,
        ITokenAuthService tokenAuthService
        ) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordEncrypt _hasher = hasher;
        private readonly IHandleBusinessError _handleError = handleError;
        private readonly IPasswordEncrypt _passwordEncrypt = passwordEncrypt;
        private readonly ITokenAuthService _tokenService = tokenAuthService;

        public async Task<Result<UserDto>> Register(RegisterDto request)
        {

            // Entry validations
            var validator = new RegisterValidation();
            ValidationResult userValidated = validator.Validate(request);

            if(!userValidated.IsValid)
            {
                _handleError.AddValidationErrors(userValidated);
                return Result<UserDto>.Fail();
            }

            var usernameExists = await _unitOfWork.User.GetUserByName(request.Username);
            if (usernameExists != null)
            {
                _handleError.AddError("USERNAME_ALREADY_EXIST", 400, "Username");
            }

            var emailExists = await _unitOfWork.User.GetUserByEmail(request.Email);
            if (emailExists != null)
            {
                _handleError.AddError("EMAIL_ALREADY_EXIST",400, "Email");
            }

            var roleExist = await _unitOfWork.Role.GetById(request.RoleId);
            if (roleExist == null)
            {
                _handleError.AddError("ROLE_NOT_FOUND",404, "Role");
            }

            if(_handleError.HasErrors())
            {
                return Result<UserDto>.Fail();
            }

            // Convert to Model
            var newUser = request.ToModel();
            newUser.Role = roleExist!; // the role exist for sure

            // Hash Password
            var hashedPassword = _hasher.Hash(request.Password);
            newUser.PasswordHash = hashedPassword;

            await _unitOfWork.User.Register(newUser);
            await _unitOfWork.Save();

            return Result<UserDto>.Ok(UserDto.FromModel(newUser));
        }

        public async Task<Result<TokenDto>> Login(LoginDto request)
        {
            var user = await _unitOfWork.User.GetUserByNameOrEmail(request.Identifier);

            if(user == null)
            {
                _handleError.AddError("User does not exist", 404, "user");
                return Result<TokenDto>.Fail();
            }

            // Verify password
            var passwordVerified = _passwordEncrypt.Verify(request.Password, user.PasswordHash);

            if(!passwordVerified)
            {
                _handleError.AddError("User or password are incorrect", 401, "user");
                return Result<TokenDto>.Fail();
            }

            // Create the token and refresh token for the user
            return await _tokenService.CreateTokenResponse(user);

        }

        public async Task<Result<TokenDto>> RefreshAccessTokenAsync(RefreshTokenDto request)
        {
            // get user by refreshToken
            var user = await _unitOfWork.User.GetUserByRefreshToken(request.RefreshToken);

            if(user == null)
            {
                _handleError.AddError("Refresh token not valid", 401, "user");
                return Result<TokenDto>.Fail();
            }

            // Create the token and refresh token for the user
            return await _tokenService.CreateTokenResponse(user);

        }
    }
}