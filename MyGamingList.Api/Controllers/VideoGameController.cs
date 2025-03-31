using Microsoft.AspNetCore.Mvc;
using MyGamingList.Application.Services.VideoGames;
using MyGamingList.Application.Services.VideoGames.Dtos;

namespace MyGamingList.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class VideoGameController : ControllerBase {
    private readonly IVideoGameService _videoGameService;
    private readonly ILogger<VideoGameController> _logger;

    public VideoGameController(IVideoGameService videoGameService, ILogger<VideoGameController> logger) {
        _videoGameService = videoGameService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<VideoGameDto>>> GetAllAsync() {
        _logger.LogInformation("Invoking {method} in {controller}", nameof(GetAllAsync), nameof(VideoGameController));

        try {
            List<VideoGameDto> videoGames = await _videoGameService.GetAllAsync();
            return Ok(videoGames);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invoke {method} in {controller}", nameof(GetAllAsync), nameof(VideoGameController));
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VideoGameDto?>> GetByIdAsync(int id) {
        _logger.LogInformation("Invoking {method} in {controller}", nameof(GetByIdAsync), nameof(VideoGameController));

        if (id < 1) {
            return BadRequest("Id cannot be less than 1.");
        }

        try {
            VideoGameDto? videoGame = await _videoGameService.GetByIdAsync(id);
            if (videoGame is not null) return Ok(videoGame);
            
            _logger.LogInformation("Video game not found");
            return NotFound("Video game not found");
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invoke {method} in {controller}", nameof(GetByIdAsync), nameof(VideoGameController));
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<VideoGameDto>> AddAsync([FromBody] CreateVideoGameDto createVideoGame) {
        _logger.LogInformation("Invoking {method} in {controller}", nameof(AddAsync), nameof(VideoGameController));

        try {
            VideoGameDto videoGame = await _videoGameService.AddAsync(createVideoGame);
            return Ok(videoGame);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invoke {method} in {controller}", nameof(AddAsync), nameof(VideoGameController));
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateVideoGameDto updateVideoGame) {
        _logger.LogInformation("Invoking {method} in {controller}", nameof(UpdateAsync), nameof(VideoGameController));

        if (id < 1) {
            return BadRequest("Id cannot be less than 1.");
        }

        try {
            int rowsAffected = await _videoGameService.UpdateAsync(id, updateVideoGame);
            if (rowsAffected > 0) return NoContent();
            
            _logger.LogInformation("Video game not found");
            return NotFound("Video game not found");
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invoke {method} in {controller}", nameof(UpdateAsync), nameof(VideoGameController));
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id) {
        _logger.LogInformation("Invoking {method} in {controller}", nameof(DeleteAsync), nameof(VideoGameController));

        if (id < 1) {
            return BadRequest("Id cannot be less than 1.");
        }

        try {
            int rowsAffected = await _videoGameService.DeleteAsync(id);
            if (rowsAffected > 0) return NoContent();
            
            _logger.LogInformation("Video game not found");
            return NotFound("Video game not found");
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to invoke {method} in {controller}", nameof(DeleteAsync), nameof(VideoGameController));
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
