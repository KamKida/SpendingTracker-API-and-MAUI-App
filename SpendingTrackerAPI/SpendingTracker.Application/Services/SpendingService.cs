using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Interfaces.ContextServices;
using SpendingTracker.Application.Interfaces.Services;
using SpendingTracker.Contracts.Dtos.Requests;
using SpendingTracker.Contracts.Dtos.Responses;
using SpendingTracker.Domain.Exeptions;
using SpendingTracker.Domain.Models;
using SpendingTracker.Infrastructure.Context;
using SpendingTracker.Infrastructure.Validators;

namespace SpendingTracker.Application.Services
{
	public class SpendingService : ISpendingService
	{

		private readonly SpendingTrackerDbContext _context;
		private readonly SpendingRequestValidator _validator;
		private readonly IUserContextService _userContextService;
		private readonly IMapper _mapper;
		public SpendingService(
			SpendingTrackerDbContext context,
			SpendingRequestValidator validator,
			IUserContextService userContextService,
			IMapper mapper
		)
		{
			_context = context;
			_validator = validator;
			_userContextService = userContextService;
			_mapper = mapper;
		}

		public async Task<List<SpendingReponse>> GetByFilter(SpendingFilterRequest request)
		{
			//_userContextService.GetUserId()
			var query = _context.Spendings.Where(f => f.UserId == Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42"));

			query = AddFilter(query, request);


			var result = await query
				.OrderByDescending(f => f.CreationDate)
				.Take(10)
				.Select(f => new SpendingReponse()
				{
					Id = f.Id,
					Amount = f.Amount,
					CreationDate = f.CreationDate,
					Description = f.Description
				})
				.AsNoTracking()
				.ToListAsync();

			return result;
		}

		public async Task<SpendingReponse> AddSpending(SpendingRequest request)
		{
			var result = _validator.Validate(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new BadRequestException(error?.ErrorMessage ?? "Dodanie fwydatku nie powiodło się.");
			}

			Spending newSpending = _mapper.Map<Spending>(request);

			newSpending.UserId = Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42");

			//newFund.UserId = (Guid)_userContextService.GetUserId();
			await _context.Spendings.AddAsync(newSpending);
			await _context.SaveChangesAsync();

			return _mapper.Map<SpendingReponse>(newSpending);

		}

		public async Task DeleteSpending(Guid spendingId)
		{
			Spending spendingToDelete = await _context.Spendings
			.Where(s => s.Id == spendingId)
			.FirstOrDefaultAsync();

			if (spendingToDelete == null)
			{
				throw new BadRequestException("Usunięcie wydatku nie powiodło się. Podany fundusz nie istnieje.");
			}

			_context.Spendings.Remove(spendingToDelete);

			await _context.SaveChangesAsync();
		}

		public async Task EditSpending(SpendingRequest request)
		{//_userContextService.GetUserId()
			Spending spendingToEdit = await _context.Spendings
			.Where(s => s.UserId == Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42")
				&& s.Id == request.Id)
				.FirstOrDefaultAsync();

			if (spendingToEdit == null)
			{
				throw new BadRequestException("Edycja wydatku nie powiodło się. Podany fundusz nie istnieje.");
			}


			spendingToEdit.Amount = request.Amount;
			spendingToEdit.Description = request.Description;

			await _context.SaveChangesAsync();
		}

		private IQueryable<Spending> AddFilter(IQueryable<Spending> query, SpendingFilterRequest request)
		{
			if (request.AmountFrom != null)
			{
				query = query.Where(s => s.Amount >= request.AmountFrom);
			}

			if (request.AmountTo != null)
			{
				query = query.Where(s => s.Amount <= request.AmountTo);
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

			if (request.SpendingCategoryId != null)
			{
				query = query.Where(s => s.SpendingCategoryId == request.SpendingCategoryId);
			}

			return query;
		}

	}
}
