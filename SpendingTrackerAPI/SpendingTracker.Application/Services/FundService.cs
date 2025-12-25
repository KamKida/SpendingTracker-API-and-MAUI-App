using AutoMapper;
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
		public async Task<FundResponse> AddFund(FundRequest request)
		{
			var result = _validator.Validate(request);

			if (!result.IsValid)
			{
				var error = result.Errors.FirstOrDefault();
				throw new RegisterExeption(error?.ErrorMessage ?? "Dodanie funduszu nie powiodło się.");
			}

			Fund newFund = _mapper.Map<Fund>(request);
			newFund.UserId = (Guid)_userContextService.GetUserId();
			await _context.Funds.AddAsync(newFund);
			await _context.SaveChangesAsync();

			return _mapper.Map<FundResponse>(newFund);

		}
	}
}
