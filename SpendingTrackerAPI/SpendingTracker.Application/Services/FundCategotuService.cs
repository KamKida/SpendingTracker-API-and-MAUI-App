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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			//_userContextService.GetUserId()
			var query = _context.FundCategories.Where(f => f.UserId == Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42"));

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

			newFundCategory.UserId = Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42");

			//newFundcategory.UserId = (Guid)_userContextService.GetUserId();
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
		{//_userContextService.GetUserId()
			FundCategory fundToEdit = await _context.FundCategories
			.Where(f => f.UserId == Guid.Parse("92AE20E5-BAE7-4EB5-42FB-08DE3EFD3C42")
				&& f.Id == fundCategoryRequest.Id)
				.FirstOrDefaultAsync();

			if (fundToEdit == null)
			{
				throw new BadRequestException("Edycja kategori funduszu nie powiodło się. Podany fundusz nie istnieje.");
			}

			fundToEdit.Name = fundCategoryRequest.Name;
			fundToEdit.ShouldBe = fundCategoryRequest.ShouldBe;

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
				query = query.Where(f => f.CreationDate.Date >= request.DateFrom);
			}

			if (request.DateTo != null)
			{
				query = query.Where(f => f.CreationDate.Date <= request.DateTo);
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
