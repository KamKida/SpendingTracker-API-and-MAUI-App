using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.PlanedSpendingDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Validators.FundValidators;
using SpendingsManagementWebAPI.Validators.GroupValidators;
using SpendingsManagementWebAPI.Validators.PlanedSpendingValidators;

namespace SpendingsManagementWebAPI.Services
{
    public class PlanedSpendingsService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly UserContextService _userContextService;
        private readonly AddPlanedSpendingValidator _addPlanedSpendingValidator;
       private readonly EditPlanedSpendingValidator _editPlanedSpendingValidator;
        private readonly IMapper _mapper;

        public PlanedSpendingsService(SpendingDbContext dbContext,
            UserContextService userContextService,
            AddPlanedSpendingValidator addPlanedSpendingValidator,
            EditPlanedSpendingValidator editPlanedSpendingValidator,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _addPlanedSpendingValidator = addPlanedSpendingValidator;
            _editPlanedSpendingValidator = editPlanedSpendingValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPlanedSpeningDto>> GetAll()
        {
            var planedSpendings = await _dbContext.PlanedSpendings
                .Where(f => f.UserId == (int)_userContextService.GetUserId)
                .Include(s => s.Group)
                .ToListAsync();

            var planedSpendingsDto = _mapper.Map<List<GetPlanedSpeningDto>>(planedSpendings);

            return planedSpendingsDto;
        }

        public async Task Add(AddPlanedSpendingDto dto)
        {
            var result = _addPlanedSpendingValidator.Validate(dto);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new WrongDataException(error?.ErrorMessage ?? "Nie udało się zapisać planowanego wydatku.");
            }

            var newPlanedSpending = _mapper.Map<PlannedSpending>(dto);
            newPlanedSpending.UserId = (int)_userContextService.GetUserId;

            _dbContext.PlanedSpendings.Add(newPlanedSpending);
            await _dbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var planedSopendingToDelete = await _dbContext.PlanedSpendings
                .FirstOrDefaultAsync(f => f.Id == id);

            if (planedSopendingToDelete is not null)
            {
                _dbContext.PlanedSpendings.Remove(planedSopendingToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego planowanego wydatku.");
            }
        }

        public async Task Edit(EditPlanedSpendingDto dto)
        {
            var planedSpendingToEdit = await _dbContext.PlanedSpendings
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (planedSpendingToEdit is not null)
            {
                var result = _editPlanedSpendingValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                }
                ;

                _mapper.Map(dto, planedSpendingToEdit);

                _dbContext.PlanedSpendings.Update(planedSpendingToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranego planowanego wydatku.");
            }
        }
    }
}
