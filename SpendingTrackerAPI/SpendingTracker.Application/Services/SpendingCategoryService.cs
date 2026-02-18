using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Interfaces.ContextServices;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Requests.FiltersRequests;
using SpendingTracker.Contracts.Dtos.Responses;
using SpendingTracker.Domain.Exeptions;
using SpendingTracker.Domain.Models;
using SpendingTracker.Infrastructure.Context;
using SpendingTracker.Infrastructure.Validators;

namespace SpendingTracker.Application.Services
{
	public class SpendingCategoryService : ISpendingCategoryService
	{
		private readonly SpendingTrackerDbContext _context;
		private readonly SpendingCategoryRequestValidator _validator;
		private readonly IUserContextService _userContextService;
		private readonly IMapper _mapper;

		public SpendingCategoryService(
			SpendingTrackerDbContext context,
			SpendingCategoryRequestValidator validator,
			IUserContextService userContextService,
			IMapper mapper
		)
		{
			_context = context;
			_validator = validator;
			_userContextService = userContextService;
			_mapper = mapper;
		}
		public async Task<List<SpendingCategoryResponse>> GetByFilter(SpendingCategoryFilterRequest request)
		{
			var query = _context.SpendingCategories
				.Where(s => s.UserId == _userContextService.GetUserId());

			query = AddFilter(query, request);

			var result = await query
				.OrderByDescending(s => s.CreationDate)
				.Take(10)
				.Select(s => new SpendingCategoryResponse()
				{
					Id = s.Id,
					Name = s.Name,
					WeeklyLimit = s.WeeklyLimit,
					MonthlyLimit = s.MonthlyLimit,
					CreationDate = s.CreationDate,
					Description = s.Description
				})
				.AsNoTracking()
				.ToListAsync();

			return result;
		}
		public async Task AddSpendingCategory(SpendingCategoryRequest request)
		{
			var result = _validator.Validate(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new BadRequestException(error?.ErrorMessage ?? "Dodanie kategorii wydatków nie powiodło się.");
			}

			SpendingCategory newSpendingCategory = _mapper.Map<SpendingCategory>(request);

			newSpendingCategory.UserId = (Guid)_userContextService.GetUserId();
			await _context.SpendingCategories.AddAsync(newSpendingCategory);
			await _context.SaveChangesAsync();
		}
		public async Task DeleteSpendingCategory(Guid spendingCategoryId)
		{
			SpendingCategory spendingCategoryToDelete = await _context.SpendingCategories
				.Where(s => s.Id == spendingCategoryId)
				.FirstOrDefaultAsync();

			if (spendingCategoryToDelete == null)
			{
				throw new BadRequestException("Usunięcie kategorii wydatków nie powiodło się. Podana kategoria nie istnieje.");
			}

			_context.SpendingCategories.Remove(spendingCategoryToDelete);
			await _context.SaveChangesAsync();
		}
		public async Task EditSpendingCategory(SpendingCategoryRequest spendingCategoryRequest)
		{
			SpendingCategory spendingCategoryToEdit = await _context.SpendingCategories
				.Where(s => s.UserId == _userContextService.GetUserId()
					&& s.Id == spendingCategoryRequest.Id)
				.FirstOrDefaultAsync();

			if (spendingCategoryToEdit == null)
			{
				throw new BadRequestException("Edycja kategorii wydatków nie powiodła się. Podana kategoria nie istnieje.");
			}

			spendingCategoryToEdit.Name = spendingCategoryRequest.Name;

			if (spendingCategoryRequest.WeeklyLimit.HasValue)
			{
				spendingCategoryToEdit.WeeklyLimit = spendingCategoryRequest.WeeklyLimit;
			}

			if (spendingCategoryRequest.MonthlyLimit.HasValue)
			{
				spendingCategoryToEdit.MonthlyLimit = spendingCategoryRequest.MonthlyLimit;
			}

			spendingCategoryToEdit.Description = spendingCategoryRequest.Description;

			await _context.SaveChangesAsync();
		}
		private IQueryable<SpendingCategory> AddFilter(IQueryable<SpendingCategory> query, SpendingCategoryFilterRequest request)
		{
			if (!string.IsNullOrEmpty(request.Name))
			{
				query = query.Where(s => s.Name.Contains(request.Name));
			}

			if (request.WeeklyLimitFrom != null)
			{
				query = query.Where(s => s.WeeklyLimit>= request.WeeklyLimitFrom);
			}

			if (request.WeeklyLimitTo != null)
			{
				query = query.Where(s => s.WeeklyLimit <= request.WeeklyLimitTo);
			}

			if (request.MonthlyLimitFrom != null)
			{
				query = query.Where(s => s.MonthlyLimit >= request.MonthlyLimitFrom);
			}

			if (request.MonthlyLimitTo != null)
			{
				query = query.Where(s => s.MonthlyLimit <= request.MonthlyLimitTo);
			}

			if (request.DateFrom != null)
			{
				var dateFrom = request.DateFrom.Value.DateTime;
				query = query.Where(s => s.CreationDate.Date >= dateFrom);
			}

			if (request.DateTo != null)
			{
				var dateTo = request.DateTo.Value.DateTime;
				query = query.Where(s => s.CreationDate.Date <= dateTo);
			}

			if (request.LastDate != null)
			{
				var lastDate = request.LastDate.Value.DateTime;
				query = query.Where(s => s.CreationDate < lastDate);
			}

			return query;
		}
	}
}
