using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.SpendingDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Validators.FundValidators;
using SpendingsManagementWebAPI.Validators.SpendingVlidators;

namespace SpendingsManagementWebAPI.Services
{
    public class SpendingService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly AddSpendingValidator _addSpendingsValidator;
        private readonly EditSpendingValiator _editSpendingsValidator;
        private readonly UserContextService _userContextService;
        private readonly IMapper _mapper;
        public SpendingService(SpendingDbContext dbContext,
            AddSpendingValidator addSpendingsValidato,
            EditSpendingValiator editSpendingsValidator,
            UserContextService userContextService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _addSpendingsValidator = addSpendingsValidato;
            _editSpendingsValidator = editSpendingsValidator;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetSpedingDto>> GetAll()
        {
            var spendigsForThisMonth = await _dbContext.Spendings
                .Where(s => s.UserId == (int)_userContextService.GetUserId
                && s.CreationDate.Month == DateTime.Now.Month
                && s.CreationDate.Year == DateTime.Now.Year)
                .Include(s => s.Group)
                .ToListAsync();

            var spendings = _mapper.Map<List<GetSpedingDto>>(spendigsForThisMonth);
            return spendings;
        }

        public async Task Add(AddSpendingDto dto)
        {
            var result = _addSpendingsValidator.Validate(dto);
            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new WrongDataException(error?.ErrorMessage ?? "Nie udało się zapisać wydatku.");
            }

            var newSpending = _mapper.Map<Spending>(dto);
            newSpending.UserId = (int)_userContextService.GetUserId;

            _dbContext.Spendings.Add(newSpending);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var spendingToDelte = await _dbContext.Spendings
                .FirstOrDefaultAsync(s => s.Id == id);


            if (spendingToDelte is not null)
            {
                _dbContext.Spendings.Remove(spendingToDelte);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego funduszu.");
            }
        }

        public async Task Edit(EditSpendingDto dto)
        {
            var spendingToEdit = await _dbContext.Spendings
                .FirstOrDefaultAsync(s => s.Id == dto.Id);

            if (spendingToEdit is not null)
            {
                var result = _editSpendingsValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                }
                ;

                _mapper.Map(dto, spendingToEdit);

                _dbContext.Spendings.Update(spendingToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego wydatku.");
            }
        }
    }
}
