using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpendingsManagementWebAPI.Dtos.AuthenticationDtos;
using SpendingsManagementWebAPI.Dtos.UserDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Validators.UserValidators;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingsManagementWebAPI.Services
{
    public class AccountService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RegisterUserValidator _registerValidator;
        private readonly EditUserValidator _editUserValidator;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly UserContextService _userContextService;

        public AccountService(SpendingDbContext dbContext,
            IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings,
            RegisterUserValidator registerValidator,
            EditUserValidator editUserValidator,
            UserContextService userContextService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _registerValidator = registerValidator;
            _editUserValidator = editUserValidator;
            _authenticationSettings = authenticationSettings;
            _userContextService = userContextService;

        }

        public async Task RegisterUser(RegisterUserDto dto)
        {
            var validationResult = _registerValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var error = validationResult.Errors.FirstOrDefault();
                throw new RegisterExeption(error?.ErrorMessage ?? "Rejestracja nie powidła się.");
            }

            var newUser = new User()
            {
                Email = dto.Email,
                DateOfCreation = DateTime.Now,
                UserName = dto.UserName,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.Password = hashedPassword;

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password.");
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? "UnknownUser")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);


            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public async Task Delete()
        {
            var userToDelete = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);

            if (userToDelete is not null)
            {
                _dbContext.Users.Remove(userToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się usunąć użtkownika. Nie został znaleziony.");
            }
        }

        public async Task Edit(EditUserDto dto)
        {
            var userToEdit = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);

            if (userToEdit is not null)
            {
                var result = _editUserValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                };
                
                var hashedPassword = _passwordHasher.HashPassword(userToEdit, dto.NewPassword);

                userToEdit.Email = dto.NewEmail;
                userToEdit.Password = hashedPassword;

                _dbContext.Update(userToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się usunąć użtkownika. Nie został znaleziony.");
            }
        }
    }
}
