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
	public class FundCategotuService : IFundCategotuService
	{
		private readonly SpendingTrackerDbContext _context;
		private readonly FundCategoryRequestValidator _validator;
		private readonly IUserContextService _userContextService;
		private readonly IMapper _mapper;
		public FundCategotuService(
			SpendingTrackerDbContext context,
			FundCategoryRequestValidator validator,
			IUserContextService userContextService,
			IMapper mapper
		)
		{
			_context = context;
			_validator = validator;
			_userContextService = userContextService;
			_mapper = mapper;
		}

		public async Task<List<FundCategoryResponse>> GetByFilter(FundCategoryFilterRequest request)
		{
			var query = _context.FundCategories.Where(f => f.UserId == _userContextService.GetUserId());

			query = AddFilter(query, request);


			var result = await query
				.OrderByDescending(f => f.CreationDate)
				.Take(10)
				.Select(f => new FundCategoryResponse()
				{
					Id = f.Id,
					Name = f.Name,
					ShouldBe = f.ShouldBe,
					CreationDate = f.CreationDate,
					Description = f.Description
				})
				.AsNoTracking()
				.ToListAsync();

			return result;
		}

		public async Task AddFundCategory(FundCategoryRequest request)
		{
			var result = _validator.Validate(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new BadRequestException(error?.ErrorMessage ?? "Dodanie kategori funduszu nie powiodło się.");
			}

			FundCategory newFundCategory = _mapper.Map<FundCategory>(request);

			newFundCategory.UserId = (Guid)_userContextService.GetUserId();
			await _context.FundCategories.AddAsync(newFundCategory);
			await _context.SaveChangesAsync();

		}

		public async Task DeleteFundCategory(Guid fundId)
		{
			FundCategory fundCategoryToDelete = await _context.FundCategories
			.Where(f => f.Id == fundId)
			.FirstOrDefaultAsync();

			if (fundCategoryToDelete == null)
			{
				throw new BadRequestException("Usunięcie funduszu nie powiodło się. Podany fundusz nie istnieje.");
			}

			_context.FundCategories.Remove(fundCategoryToDelete);

			await _context.SaveChangesAsync();
		}

		public async Task EditFundCategory(FundCategoryRequest fundCategoryRequest)
		{
			FundCategory fundCategoryToEdit = await _context.FundCategories
			.Where(f => f.UserId ==  _userContextService.GetUserId()
				&& f.Id == fundCategoryRequest.Id)
				.FirstOrDefaultAsync();

			if (fundCategoryToEdit == null)
			{
				throw new BadRequestException("Edycja kategori funduszu nie powiodło się. Podany fundusz nie istnieje.");
			}

			fundCategoryToEdit.Name = fundCategoryRequest.Name;
			if(fundCategoryRequest.ShouldBe.HasValue)
			{
				fundCategoryToEdit.ShouldBe = fundCategoryRequest.ShouldBe;
			}
			fundCategoryToEdit.Description = fundCategoryRequest.Description;

			await _context.SaveChangesAsync();
		}

		private IQueryable<FundCategory> AddFilter(IQueryable<FundCategory> query, FundCategoryFilterRequest request)
		{
			if (!String.IsNullOrEmpty(request.Name))
			{
				query = query.Where(f => f.Name.Contains(request.Name));
			}

			if (request.ShouldBeFrom != null)
			{
				query = query.Where(f => f.ShouldBe >= request.ShouldBeFrom);
			}

			if (request.ShouldBeTo != null)
			{
				query = query.Where(f => f.ShouldBe <= request.ShouldBeTo);
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

			return query;
		}
	}
}
