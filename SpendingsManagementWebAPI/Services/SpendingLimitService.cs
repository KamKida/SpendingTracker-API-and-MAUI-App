using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Dtos.SpendingLimitDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Validators.FundValidators;
using SpendingsManagementWebAPI.Validators.SpendingLimitValiddators;
using SpendingsManagementWebAPI.Validators.SpendingVlidators;

namespace SpendingsManagementWebAPI.Services
{
    public class SpendingLimitService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly AddSpendingLimitValidator _addSpendingsLimitValidator;
        private readonly EditSpendingLimitValidator _editSpendingLimitValidator;
        private readonly UserContextService _userContextService;
        private readonly IMapper _mapper;
        public SpendingLimitService(SpendingDbContext dbContext,
            AddSpendingLimitValidator addSpendingsLimitValidator,
            EditSpendingLimitValidator editSpendingLimitValidator,
            UserContextService userContextService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _addSpendingsLimitValidator = addSpendingsLimitValidator;
            _editSpendingLimitValidator = editSpendingLimitValidator;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetSpendingLimitDto>> GetAll()
        {
            var limitDb = await _dbContext.SpendingLimits
                .Where(s => s.UserId == _userContextService.GetUserId)
                .ToListAsync();

            var limits = _mapper.Map<List<GetSpendingLimitDto>>(limitDb);

            return limits;
        }

        public async Task Add(AddSpendingLimitDto dto)
        {
            var result = _addSpendingsLimitValidator.Validate(dto);
            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new WrongDataException(error?.ErrorMessage ?? "Nie udało się zapisać limitu.");
            }

            var newSpending = _mapper.Map<SpendingLimit>(dto);
            newSpending.UserId = (int)_userContextService.GetUserId;

            _dbContext.SpendingLimits.Add(newSpending);
            await _dbContext.SaveChangesAsync();
        } 

        public async Task Delete(int id)
        {
            var spendingLimitToDelete = await _dbContext.SpendingLimits
                .FirstOrDefaultAsync(l => l.Id == id);

            if (spendingLimitToDelete is not null)
            {
                _dbContext.SpendingLimits.Remove(spendingLimitToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego funduszu.");
            }
        }

        public async Task Edit(EditSpendingLimitDto dto)
        {
            var spendingLimitToEidt = await _dbContext.SpendingLimits
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (spendingLimitToEidt is not null)
            {
                var result = _editSpendingLimitValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                }
                ;

                _mapper.Map(dto, spendingLimitToEidt);

                _dbContext.SpendingLimits.Update(spendingLimitToEidt);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego limitu wydatków.");
            }
        }
    }
}
