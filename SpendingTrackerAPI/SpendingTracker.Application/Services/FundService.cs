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
	public class FundService : IFundService
	{

		private readonly SpendingTrackerDbContext _context;
		private readonly FundRequestValidator _validator;
		private readonly IUserContextService _userContextService;
		private readonly IMapper _mapper;
		public FundService(
			SpendingTrackerDbContext context,
			FundRequestValidator validator,
			IUserContextService userContextService,
			IMapper mapper
		)
		{
			_context = context;
			_validator = validator;
			_userContextService = userContextService;
			_mapper = mapper;
		}

		public async Task<List<FundResponse>> GetByFilter(FundFilterRequest request)
		{
			var query = _context.Funds.Where(f => f.UserId == _userContextService.GetUserId());

			query = AddFilter(query, request);


			var result = await query
				.Include(f => f.FundCategory)
				.OrderByDescending(f => f.CreationDate)
				.Take(10)
				.Select(f => new FundResponse()
				{
					Id = f.Id,
					Amount = f.Amount,
					CreationDate = f.CreationDate,
					FundCategory = f.FundCategory,
					Description = f.Description
					
				})
				.AsNoTracking()
				.ToListAsync();

			return result;
		}

		public async Task<FundResponse> AddFund(FundRequest request)
		{
			var result = _validator.Validate(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new BadRequestException(error?.ErrorMessage ?? "Dodanie funduszu nie powiodło się.");
			}

			Fund newFund = _mapper.Map<Fund>(request);

			newFund.UserId = (Guid)_userContextService.GetUserId();
			await _context.Funds.AddAsync(newFund);
			await _context.SaveChangesAsync();

			return _mapper.Map<FundResponse>(newFund);

		}

		public async Task DeleteFund(Guid fundId)
		{
			Fund fundToDelete = await _context.Funds
			.Where(f => f.Id == fundId)
			.FirstOrDefaultAsync();

			if (fundToDelete == null)
			{
				throw new BadRequestException("Usunięcie funduszu nie powiodło się. Podany fundusz nie istnieje.");
			}

			_context.Funds.Remove(fundToDelete);

			await _context.SaveChangesAsync();
		}

		public async Task EditFund(FundRequest fundRequest)
		{
			Fund fundToEdit = await _context.Funds
			.Where(f => f.UserId == _userContextService.GetUserId()
				&& f.Id == fundRequest.Id)
				.FirstOrDefaultAsync();

			if (fundToEdit == null)
			{
				throw new BadRequestException("Edycja funduszu nie powiodło się. Podany fundusz nie istnieje.");
			}

			if(fundRequest.Amount.HasValue)
			{
				fundToEdit.Amount = (decimal)fundRequest.Amount;
			}
			fundToEdit.FundCategoryId = fundRequest.FundCategoryId;
			fundToEdit.Description = fundRequest.Description;

			await _context.SaveChangesAsync();
		}

		private IQueryable<Fund> AddFilter(IQueryable<Fund> query, FundFilterRequest request)
		{
			if (request.AmountFrom != null)
			{
				query = query.Where(f => f.Amount >= request.AmountFrom);
			}

			if (request.AmountTo != null)
			{
				query = query.Where(f => f.Amount <= request.AmountTo);
			}

			if (request.DateFrom != null)
			{
				var dateFrom = request.DateFrom.Value.DateTime;
				query = query.Where(f => f.CreationDate.Date >= dateFrom);
			}

			if (request.DateTo != null)
			{
				var dateTo = request.DateTo.Value.DateTime;
				query = query.Where(f => f.CreationDate.Date <= dateTo);
			}

			if (request.LastDate != null)
			{
				var lastDate = request.LastDate.Value.DateTime;
				query = query.Where(f => f.CreationDate < lastDate);
			}

			if (request.FundCategoryId != null)
			{
				query = query.Where(f => f.FundCategoryId == request.FundCategoryId);
			}

			return query;
		}

	}
}
