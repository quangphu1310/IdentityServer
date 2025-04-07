using IdentityServer.Models;
using IdentityServer.Models.DTOs;
using IdentityServer.Repositories.IRepositories;
using IdentityServer.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepo;

        public AuthService(IUserRepository userRepo, ITokenService tokenService, ITokenRepository tokenRepo)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _tokenRepo = tokenRepo;
        }
        public APIResponse Register(RegisterRequest request)
        {
            var response = new APIResponse();

            var existing = _userRepo.GetByUsername(request.Username);
            if (existing != null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add("Username already exists");
                return response;
            }

            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User
            {
                Username = request.Username,
            };
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            _userRepo.Add(newUser);

            response.StatusCode = HttpStatusCode.Created;
            response.Result = "User created successfully";
            return response;
        }
        public APIResponse Login(LoginRequest request)
        {
            var response = new APIResponse();

            var user = _userRepo.GetByUsername(request.Username);
            if (user == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add("Invalid credentials");
                return response;
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add("Invalid credentials");
                return response;
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _tokenRepo.SaveRefreshToken(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(3),
                IsRevoked = false
            });

            response.StatusCode = HttpStatusCode.OK;
            response.Result = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return response;
        }

        public APIResponse RefreshToken(string refreshToken)
        {
            var response = new APIResponse();

            var token = _tokenRepo.GetByToken(refreshToken);
            if (token == null || token.IsRevoked || token.ExpiryDate < DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Errors.Add("Invalid or expired refresh token");
                return response;
            }

            var accessToken = _tokenService.GenerateAccessToken(token.User);
            response.StatusCode = HttpStatusCode.OK;
            response.Result = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return response;
        }

        public APIResponse Logout(string refreshToken)
        {
            var response = new APIResponse();

            var token = _tokenRepo.GetByToken(refreshToken);
            if (token == null || token.IsRevoked)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Errors.Add("Token is invalid or already revoked");
                return response;
            }

            _tokenRepo.RevokeToken(refreshToken);

            response.StatusCode = HttpStatusCode.OK;
            response.Result = "Logged out successfully";
            return response;
        }
    }
}
