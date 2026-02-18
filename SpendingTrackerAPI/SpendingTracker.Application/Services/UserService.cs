using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
			var validator = new UserEditRequestValidator(checkPassword: false);

			var result = await validator.ValidateAsync(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new RegisterExeption(error?.ErrorMessage ?? "Edycja konta nie powiódł się.");
			}

			User userToEdit = await _context.Users
								.Where(u => u.Id == _userContextService.GetUserId())
								.FirstOrDefaultAsync();


			if (userToEdit == null)
			{
				throw new BadRequestException("Edycja uzytkownika nie powiodła się. Podany użytkownik nie istnieje.");
			}

			userToEdit.FirstName = request.FirstName;
			userToEdit.LastName = request.LastName;
			userToEdit.Email = request.Email;
			if (request.Password != null)
			{
				var newPassword = _passwordHasher.HashPassword(userToEdit, request.Password);

				userToEdit.Password = newPassword;
			}


			await _context.SaveChangesAsync();

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

		public async Task<UserResponse> GetUserThisMonthInfo()
		{
			DateTime today = DateTime.UtcNow;
			UserResponse response = await _context.Users
								.Where(u => u.Id == _userContextService.GetUserId())
								.Select(u => new UserResponse
								{
									FirstName = u.FirstName,
									LastName = u.LastName,
									ThisMonthFundSum = u.Funds
									.Where(f => f.CreationDate.Month == today.Month
											&& f.CreationDate.Year == today.Year)
									.Sum(f => f.Amount),
																ThisMonthSpendingsSum = u.Spendings
									.Where(s => s.CreationDate.Month == today.Month
											&& s.CreationDate.Year == today.Year)
									.Sum(s => s.Amount),
																SpendingReponses = u.Spendings
									.Where(s => s.CreationDate.Month == today.Month && s.CreationDate.Year == today.Year)
									.OrderByDescending(s => s.CreationDate)
									.Take(5)
									.Select(s => new SpendingReponse
									{
										Id = s.Id,
										Amount = s.Amount,
										CreationDate = s.CreationDate
									}).ToList(),
																SpendingCategoryResponses = u.SpendingCategories
								.Where(s =>
									s.CreationDate.Year == today.Year &&
									s.MonthlyLimit.HasValue
								)
								.Select(s => new SpendingCategoryResponse
								{
									Id = s.Id,
									Name = s.Name,
									MonthlyLimit = s.MonthlyLimit,
									WeeklyLimit = s.WeeklyLimit,
									CreationDate = s.CreationDate,
									MonthlyLimitDiffrence =
										s.MonthlyLimit.Value -
										s.Spendings
											.Where(sp => sp.CreationDate.Month == today.Month &&
														 sp.CreationDate.Year == today.Year)
											.Sum(sp => (decimal?)sp.Amount) ?? 0
								})
								.Where(s => s.MonthlyLimitDiffrence <= 100)
								.OrderByDescending(s => s.MonthlyLimitDiffrence)
								.ToList()
								}).FirstOrDefaultAsync();

			if (response == null)
			{
				throw new GetDataExeption("Coś poszło nie tak podczas pobierania danych konta.");
			}

			return response;
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
								}).FirstOrDefaultAsync();

			if (response == null)
			{
				throw new GetDataExeption("Coś poszło nie tak podczas pobierania danych konta.");
			}

			return response;
		}

		public async Task DeleteUser()
		{
			await using var transaction = await _context.Database.BeginTransactionAsync();

			Guid userId = (Guid)_userContextService.GetUserId();

			try
			{
				var fundsToDelete = await _context.Funds
					.Where(f => f.UserId == userId)
					.ToListAsync();

				_context.Funds.RemoveRange(fundsToDelete);

				var fundsCategoriesToDelete = await _context.FundCategories
					.Where(fc => fc.UserId == userId)
					.ToListAsync();

				_context.FundCategories.RemoveRange(fundsCategoriesToDelete);

				var spendingsToDelete = await _context.Spendings
				.Where(s => s.UserId == userId)
				.ToListAsync();

				_context.Spendings.RemoveRange(spendingsToDelete);

				var spendingsCattegoriesToDelete = await _context.SpendingCategories
					.Where(sc => sc.UserId == userId)
					.ToListAsync();

				_context.SpendingCategories.RemoveRange(spendingsCattegoriesToDelete);

				var userToDelete = await _context.Users
					.Where(u => u.Id == userId)
					.FirstOrDefaultAsync();

				_context.Users.Remove(userToDelete);

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}

}
