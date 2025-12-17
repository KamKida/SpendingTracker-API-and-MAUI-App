using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Dtos.FundsDtos;
using SpendingsManagementWebAPI.Dtos.GroupDtos;
using SpendingsManagementWebAPI.Entities;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Validators.FundValidators;
using SpendingsManagementWebAPI.Validators.GroupValidators;

namespace SpendingsManagementWebAPI.Services
{
    public class GroupService
    {
        private readonly SpendingDbContext _dbContext;
        private readonly UserContextService _userContextService;
        private readonly AddGroupValidator _addGroupValidator;
        private readonly EditGroupValidator _editGroupValidator;
        private readonly IMapper _mapper;

        public GroupService(SpendingDbContext dbContext,
            UserContextService userContextService,
            AddGroupValidator addGroupValidator,
            EditGroupValidator editGroupValidator,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _addGroupValidator = addGroupValidator;
            _editGroupValidator = editGroupValidator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGroupDto>> GetAll()
        {
            var groups = await _dbContext.SpendingGroups
                .Where(g => g.UserId == _userContextService.GetUserId)
                .Include(g => g.GroupLimit)
                .ToListAsync();

            var groupDtos = _mapper.Map<List<GetGroupDto>>(groups);

            return groupDtos;
        }

        public async Task Add(AddGroupDto dto)
        {
            var result = _addGroupValidator.Validate(dto);

            if (!result.IsValid)
            {
                var error = result.Errors.FirstOrDefault();
                throw new WrongDataException(error?.ErrorMessage ?? "Nie udało się zapisać grupy.");
            }

            var newGroup = _mapper.Map<Group>(dto);

            if (dto.AddEditGroupLimitDto is not null)
            {
                var newLimit = _mapper.Map<GroupLimit>(dto.AddEditGroupLimitDto);
                newGroup.GroupLimit = newLimit;
            }


            newGroup.UserId = (int)_userContextService.GetUserId;

            _dbContext.SpendingGroups.Add(newGroup);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var groupToDelete = await _dbContext.SpendingGroups
                .FirstOrDefaultAsync(f => f.Id == id);

            if (groupToDelete is not null)
            {
                var limitToDelete = _dbContext.GroupLimits
                    .FirstOrDefault(l => l.GroupId == groupToDelete.Id);

                if (limitToDelete is not null)
                {
                    _dbContext.GroupLimits.Remove(limitToDelete);
                }

                _dbContext.SpendingGroups.Remove(groupToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranej grupy.");
            }
        }

        public async Task Edit(EditGroupDto dto)
        {
            var groupToEdit = await _dbContext.SpendingGroups
                .Include(g => g.GroupLimit)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (groupToEdit is not null)
            {
                var result = _editGroupValidator.Validate(dto);

                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault();
                    throw new RegisterExeption(error?.ErrorMessage ?? "Edycja nie powiodał się.");
                }
                ;

                if (dto.AddEditGroupLimitDto is not null)
                {
                    if (groupToEdit.GroupLimit is null)
                    {
                        var newLimit = _mapper.Map<GroupLimit>(dto.AddEditGroupLimitDto);
                        groupToEdit.GroupLimit = newLimit;
                    }
                    else
                    {

                        _mapper.Map(dto.AddEditGroupLimitDto, groupToEdit.GroupLimit);

                    }
                }
                ;
                _mapper.Map(dto, groupToEdit);

                _dbContext.SpendingGroups.Update(groupToEdit);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Nie udało się znaleźć wybranej grupy.");
            }
        }
    }
}
 
           
