using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Profilers;
using SpendingsManagementWebAPI.Validators.FundValidators;

namespace SpendingsManagementWebAPI.Services
{
    public class FundService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly AddFundsValidator _addFaundsValidator;
        private readonly UserContextService _userContextService;
        private readonly EditFundsValidator _editFundsValidator;
        private readonly IMapper _mapper;

        public FundService(SpendingDbContext dbContext, 
            AddFundsValidator addFaundsValidator, 
            UserContextService userContextService,
            EditFundsValidator editFundsValidator,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _addFaundsValidator = addFaundsValidator;
            _userContextService = userContextService;
            _editFundsValidator = editFundsValidator;
            _mapper = mapper;
        }


        public async Task<IEnumerable<GetFundDto>> GetAll()
        {
            var fundsForThisMonth = await _dbContext.AddedFunds
                .Where(f => f.UserId == (int)_userContextService.GetUserId
                && f.DateOfCreation.Month == DateTime.Now.Month
                && f.DateOfCreation.Year == DateTime.Now.Year)
                .ToListAsync();

            var fundsDto = _mapper.Map<List<GetFundDto>>(fundsForThisMonth);

            return fundsDto;
        }

        public async Task Add(AddFundDto dto)
        {
            var result = _addFaundsValidator.Validate(dto);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new WrongDataException(error?.ErrorMessage ?? "Nie udało się zapisać funduszu.");
            }

            var newFund = _mapper.Map<AddedFund>(dto);
            newFund.UserId = (int)_userContextService.GetUserId;

            _dbContext.AddedFunds.Add(newFund);
            await _dbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var fundToDelte = await _dbContext.AddedFunds
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fundToDelte is not null)
            {
                _dbContext.AddedFunds.Remove(fundToDelte);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego funduszu.");
            }
        }

        public async Task Edit(EditFundDto dto)
        {
            var fundToEdit = await _dbContext.AddedFunds
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (fundToEdit is not null)
            {
                var result = _editFundsValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                };

                _mapper.Map(dto, fundToEdit);
                
                _dbContext.AddedFunds.Update(fundToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego funduszu.");
            }
        }

    }
}
