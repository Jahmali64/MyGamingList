using Microsoft.EntityFrameworkCore;
using MyGamingList.Application.Services.VideoGames.Dtos;
using MyGamingList.Domain.Entities;
using MyGamingList.Infrastructure.DbContexts;

namespace MyGamingList.Application.Services.VideoGames;

public interface IVideoGameService {
    Task<List<VideoGameDto>> GetAllAsync();
    Task<VideoGameDto?> GetByIdAsync(int videoGameId);
    Task<VideoGameDto> AddAsync(CreateVideoGameDto dto);
    Task<int> UpdateAsync(int videoGameId, UpdateVideoGameDto dto);
    Task<int> DeleteAsync(int videoGameId);
}

public sealed class VideoGameService : IVideoGameService {
    private readonly IDbContextFactory<MyGamingListDbContext> _myGamingListDbContextFactory;
    private readonly CancellationToken _cancellationToken;
    
    public VideoGameService(IDbContextFactory<MyGamingListDbContext> myGamingListDbContextFactory, CancellationToken cancellationToken) {
        _myGamingListDbContextFactory = myGamingListDbContextFactory;
        _cancellationToken = cancellationToken;
    }

    public async Task<List<VideoGameDto>> GetAllAsync() {
        await using MyGamingListDbContext db = await _myGamingListDbContextFactory.CreateDbContextAsync(_cancellationToken);
        
        return await db.VideoGames.Where(vg => vg.Visible == 1 && vg.Trash == 0)
            .Select(vg => new VideoGameDto {
                VideoGameId = vg.VideoGameId,
                Name = vg.Name ?? string.Empty,
                ReleaseDate = vg.ReleaseDate,
            }).ToListAsync(_cancellationToken);
    }
    
    public async Task<VideoGameDto?> GetByIdAsync(int videoGameId) {
        await using MyGamingListDbContext db = await _myGamingListDbContextFactory.CreateDbContextAsync(_cancellationToken);
        
        return await db.VideoGames.Where(vg => vg.VideoGameId == videoGameId && vg.Visible == 1 && vg.Trash == 0)
            .Select(vg => new VideoGameDto {
                VideoGameId = vg.VideoGameId,
                Name = vg.Name ?? string.Empty,
                ReleaseDate = vg.ReleaseDate,
            }).FirstOrDefaultAsync(_cancellationToken);
    }
    
    public async Task<VideoGameDto> AddAsync(CreateVideoGameDto dto) {
        await using MyGamingListDbContext db = await _myGamingListDbContextFactory.CreateDbContextAsync(_cancellationToken);

        VideoGame videoGame = new() {
            Name = dto.Name,
            ReleaseDate = dto.ReleaseDate,
        };
        await db.VideoGames.AddAsync(videoGame, _cancellationToken);
        await db.SaveChangesAsync(_cancellationToken);

        return new VideoGameDto {
            VideoGameId = videoGame.VideoGameId,
            Name = videoGame.Name,
            ReleaseDate = videoGame.ReleaseDate,
        };
    }
    
    public async Task<int> UpdateAsync(int videoGameId, UpdateVideoGameDto dto) {
        await using MyGamingListDbContext db = await _myGamingListDbContextFactory.CreateDbContextAsync(_cancellationToken);
        DateTime? releaseDate = dto.ReleaseDate?.ToUniversalTime();
        
        return await db.VideoGames.Where(vg => vg.VideoGameId == videoGameId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(vg => vg.Name, dto.Name)
                .SetProperty(vg => vg.ReleaseDate, releaseDate), _cancellationToken);
    }
    
    public async Task<int> DeleteAsync(int videoGameId) {
        await using MyGamingListDbContext db = await _myGamingListDbContextFactory.CreateDbContextAsync(_cancellationToken);
        
        return await db.VideoGames.Where(vg => vg.VideoGameId == videoGameId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(vg => vg.Trash, 1)
                .SetProperty(vg => vg.DeletedAt, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")), _cancellationToken);
    }
}
