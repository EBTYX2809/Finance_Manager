using AutoMapper;
using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Finance_Manager_Backend.BusinessLogic.Services;

public class SavingsService
{
    private AppDbContext _appDbContext;
    private DbTransactionTemplate _transactionTemplate;
    private ILogger<SavingsService> _logger;
    private UsersService _usersService;
    private readonly IMapper _mapper;
    public SavingsService(AppDbContext appDbContext, DbTransactionTemplate dbTransactionTemplate,
        ILogger<SavingsService> logger, UsersService usersService, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _transactionTemplate = dbTransactionTemplate;
        _logger = logger;
        _usersService = usersService;
        _mapper = mapper;
    }

    public async Task CreateSavingAsync(SavingDTO savingDTO)
    {
        var user = await _usersService.GetUserByIdAsync(savingDTO.UserId);

        var saving = _mapper.Map<Saving>(savingDTO);

        await _appDbContext.Savings.AddAsync(saving);
        await _appDbContext.SaveChangesAsync();

        savingDTO.Id = saving.Id;
    }

    public async Task<Saving> GetSavingByIdAsync(int savingId)
    {
        var saving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingId);

        if (saving == null) throw new EntityNotFoundException<Saving>(savingId);

        return saving;
    }

    public async Task<SavingDTO> GetSavingDTOByIdAsync(int savingId)
    {
        var saving = await _appDbContext.Savings.FirstOrDefaultAsync(s => s.Id == savingId);

        if (saving == null) throw new EntityNotFoundException<Saving>(savingId);

        return _mapper.Map<SavingDTO>(saving);
    }

    public async Task<List<SavingDTO>> GetSavingsAsync(int userId, int previousSavingId, int pageSize)
    {
        var savings = _appDbContext.Savings.Where(s => s.UserId == userId && s.Id > previousSavingId);

        var savingsList = await savings
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<List<SavingDTO>>(savingsList);
    }

    public async Task UpdateSavingAsync(int savingId, decimal topUpAmount)
    {
        _logger.LogInformation("Executing UdateSavingAsync method");
        await _transactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var saving = await GetSavingByIdAsync(savingId);

            if (topUpAmount > saving.Goal) throw new InvalidOperationException("Saving top up can't be geater than goal.");

            var user = await _usersService.GetUserByIdAsync(saving.UserId);

            _appDbContext.Attach(user);

            // Have to test
            if (saving.CurrentAmount + topUpAmount > saving.Goal)
            {
                decimal excess = saving.CurrentAmount + topUpAmount - saving.Goal;
                saving.CurrentAmount += topUpAmount - excess;
                user.Balance -= topUpAmount - excess;
            }
            else
            {
                saving.CurrentAmount += topUpAmount;
                user.Balance -= topUpAmount;
            }
        });
    }

    public async Task DeleteSavingAsync(int savingId)
    {
        _logger.LogInformation("Executing DeleteSavingAsync method.");
        await _transactionTemplate.ExecuteTransactionAsync(async () =>
        {
            var saving = await GetSavingByIdAsync(savingId);

            var user = await _usersService.GetUserByIdAsync(saving.UserId);

            _appDbContext.Attach(user);

            user.Balance += saving.CurrentAmount;

            _appDbContext.Savings.Remove(saving);
        });
    }
}
