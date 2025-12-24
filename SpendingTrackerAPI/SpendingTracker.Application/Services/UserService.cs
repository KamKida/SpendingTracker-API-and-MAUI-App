using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpendingTracker.Application.ContextServices;
using SpendingTracker.Application.Interfaces.ContextServices;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;
using SpendingTracker.Domain.Exeptions;
using SpendingTracker.Domain.Models;
using SpendingTracker.Infrastructure.Context;
using SpendingTracker.Infrastructure.Validators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpendingTracker.Application.Services
{
    public class UserService : IUserService
    {
        private readonly SpendingTrackerDbContext _context;
        private readonly UserRequestValidator _userRequestValidator;
        private readonly UserEditRequestValidator _userEditRequestValidator;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            SpendingTrackerDbContext context,
            UserRequestValidator userRequestValidator,
            UserEditRequestValidator userEditRequestValidator,
            AuthenticationSettings authenticationSettings,
            IUserContextService userContextService,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userRequestValidator = userRequestValidator;
            _userEditRequestValidator = userEditRequestValidator;
            _authenticationSettings = authenticationSettings;
            _userContextService = userContextService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateUser(UserRequest request)
        {
            var result = _userRequestValidator.Validate(request);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new RegisterExeption(error?.ErrorMessage ?? "Utworzenie konta nie powiodło się.");
            }

            User user = _mapper.Map<User>(request);
            string hashedPassword = _passwordHasher.HashPassword(user, request.Password);
            user.Password = hashedPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task<string> LoginUser(UserRequest request)
        {
            User user = await _context.Users
                        .Where(u => u.Email == request.Email)
                        .Select(u => new User()
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Password = u.Password,
                        })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new BadRequestException("Nieporawny login lub hasło.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Nieporawny login lub hasło.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? "UnknownUser")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);


            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtAudience,
                claims,
                expires: expires,
                signingCredentials: cred
                );

            var tokenHandler = new JwtSecurityTokenHandler();
;
            return tokenHandler.WriteToken(token);
        }


        public async Task EditUser(UserRequest request)
        {
            /*
            var result = _userRequestValidator.Validate(request);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new RegisterExeption(error?.ErrorMessage ?? "Edycja konta nie powiodła się.");
            }

            var userToDelete = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId());

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            */
        }

        public async Task ResetPassword(UserRequest request)
        {
            var result = _userEditRequestValidator.Validate(request);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new RegisterExeption(error?.ErrorMessage ?? "Reset konta nie powiódł się.");
            }

            var userToReset = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (userToReset == null)
            {
                throw new BadRequestException("Konto nie zostało znalezione. Podaj porawny e-mail.");
            }

            var hashResult = _passwordHasher.VerifyHashedPassword(userToReset, userToReset.Password, request.Password);

            if (hashResult == PasswordVerificationResult.Failed)
            {
                userToReset.Password = _passwordHasher.HashPassword(userToReset, request.Password);
            }

            await _context.SaveChangesAsync();

        }

        public async Task<UserResponse> GetUserBaseData()
        {
            DateTime today = DateTime.UtcNow;

			UserResponse response = await _context.Users
                                .Where(u => u.Id == _userContextService.GetUserId())
                                .Select(u => new UserResponse
                                {
                                    Email = u.Email,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    ThisMonthFund = u.Funds
                                    .Where(f => f.CreationDate.Month == today.Month
                                            && f.CreationDate.Year == today.Year)
                                    .Sum(f => f.Amount)
                                }).FirstOrDefaultAsync();

            if(response == null)
            {
                throw new GetDataExeption("Coś poszło nie tak podczas pobierania danych konta.");
            }

            return response;
        }

    }
}
