using MangaRead.Application.DTOs.Rating;
using MangaRead.Application.DTOs.Readables.Manga;
using MangaRead.Application.DTOs.Readables.WebNovel;
using MangaRead.Application.Repositories.Rating;
using MangaRead.Application.Repositories.Readables.Manga;
using MangaRead.Application.Repositories.Readables.WebNovel;
using MangaRead.Application.Repositories.System.User;
using MangaRead.Application.UnitOfWork;
using MangaRead.Domain.Entities.Rating;
using MangaRead.Infrastructure.Repositories.Readables.WebNovel;
using Microsoft.AspNetCore.Mvc;



namespace MangaRead.API.Controllers.Rating;

[Route("/api/v1/")]
[ApiController]
public class RatingController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRatingRepository _ratingRepository;
    private readonly IMangaRepository _mangaRepository;
    private readonly IUserRepository _userRepository;
    private readonly WebNovelRepository _webNovelRepository;

    public RatingController(IUnitOfWork unitOfWork, IRatingRepository mangaRatingRepository, IMangaRepository mangaRepository, IUserRepository userRepository, IWebNovelRepository webNovelRepository)
    {
        _unitOfWork = unitOfWork;
        _ratingRepository = mangaRatingRepository;
        _mangaRepository = mangaRepository;
        _userRepository = userRepository;
        _webNovelRepository = (WebNovelRepository)webNovelRepository;
    }


    [HttpGet("ratings")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RatingDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RatingDTO>>> GetRatings()
    {
        var ratings = await _ratingRepository.GetAllAsync();
        if (ratings == null || !ratings.Any())
        {
            return NotFound("No Ratings Found");
        }
        return Ok(ratings.ToDTO());
    }

    [HttpGet("ratings/page/{page:int}/size/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RatingDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RatingDTO>>> GetRatingsPaged(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }

        var ratings = await _ratingRepository.GetAllPagedAsync(page, pageSize);
        if (ratings == null || !ratings.Any())
        {
            return NotFound("No Ratings Found");
        }
        return Ok(ratings.ToDTO());
    }



    [HttpGet("ratings/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RatingDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingDTO>> GetRatingById(Guid id)
    {
        var rating = await _ratingRepository.GetByIdAsync(id);
        if (rating == null)
        {
            return NotFound($"Rating with id {id} not found");
        }
        return Ok(rating.ToDTO());
    }





    [HttpGet("users/{userId:guid}/ratings")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RatingDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<RatingDTO>>> GetUserRatings(Guid userId)
    {
        var ratings = await _ratingRepository.GetByUserAsync(userId);
        if (ratings == null || !ratings.Any())
        {
            return NotFound($"No Ratings For User With Id: {userId} Found");
        }
        return Ok(ratings.ToDTO());
    }

    [HttpGet("users/{userId:guid}/ratings/{page:int}/{pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RatingDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<RatingDTO>>> GetUserRatingsPaged(Guid userId, int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Invalid Page or Page Size");
        }
        var ratings = await _ratingRepository.GetByUserPagedAsync(userId, page, pageSize);
        if (ratings == null || !ratings.Any())
        {
            return NotFound($"No Ratings For User With Id: {userId} Found");
        }
        return Ok(ratings.ToDTO());
    }



    [HttpPost("ratings/mangas/{mangaId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RatingDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RatingDTO>> CreateMangaRating(Guid mangaId, [FromBody] RatingCreateDTO rating)
    {

        var manga = await _mangaRepository.GetByIdAsync(mangaId);

        if (manga == null)
        {
            return NotFound($"No Manga With Id: {mangaId} Exists");

        }

        var user = await _userRepository.GetByIdAsync(rating.UserId);
        if (user == null)
        {
            return NotFound($"No User With Id: {rating.UserId} Exists");
        }

        try
        {
            RatingEntity newRating = RatingEntity.Create(user, rating.StartCount);


            manga.AddRating(newRating);
            _mangaRepository.MarkAsModified(manga);

            await _ratingRepository.AddAsync(newRating);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetRatingById), new { id = newRating.Id }, newRating.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("ratings/web-novel/{webNovelId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RatingDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RatingDTO>> CreateWebNovelRating(Guid webNovelId, [FromBody] RatingCreateDTO rating)
    {

        var webNovel = await _webNovelRepository.GetByIdAsync(webNovelId);

        if (webNovel == null)
        {
            return NotFound($"No Manga With Id: {webNovelId} Exists");

        }

        var user = await _userRepository.GetByIdAsync(rating.UserId);
        if (user == null)
        {
            return NotFound($"No User With Id: {rating.UserId} Exists");
        }

        try
        {
            RatingEntity newRating = RatingEntity.Create(user, rating.StartCount);


            webNovel.AddRating(newRating);
            _webNovelRepository.MarkAsModified(webNovel);

            await _ratingRepository.AddAsync(newRating);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetRatingById), new { id = newRating.Id }, newRating.ToDTO());
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return BadRequest(ex.Message);
        }

    }


    [HttpGet("ratings/min/{min:int}/max/{max:int}/mangas")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MangaDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MangaDTO>>> SearchMangaRatings(int min, int max)
    {
        var mangas = await _mangaRepository.GetByRatingsAsync(min, max);

        if (mangas == null || !mangas.Any())
        {
            return NoContent();
        }
        return Ok(mangas.ToDTO());
    }

    [HttpGet("ratings/min/{min:int}/max/{max:int}/web-novels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WebNovelDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WebNovelDTO>>> SearchWebNovelRatings(int min, int max)
    {
        var webNovels = await _webNovelRepository.GetByRatingsAsync(min, max);

        if (webNovels == null || !webNovels.Any())
        {
            return NoContent();
        }

        return Ok(webNovels.ToDTO());
    }


    [HttpPut("ratings/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RatingDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RatingDTO>> UpdateRating(Guid id, [FromBody] RatingUpdateDTO rating)
    {

        RatingEntity? ratingToUpdate = await _ratingRepository.GetByIdAsync(id);

        if (ratingToUpdate == null)
        {
            return NotFound($"No Rating With Id: {id} Found");
        }

        try
        {
            ratingToUpdate.SetStarCount(rating.StartCount);

            _ratingRepository.MarkAsModified(ratingToUpdate);

            await _unitOfWork.CommitAsync();
            return Ok(ratingToUpdate.ToDTO());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("ratings/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRating(Guid id)
    {

        var rating = await _ratingRepository.GetByIdAsync(id);

        if (rating == null)
        {
            return NotFound($"No Rating With Id: {id} Found");
        }

        var manga = await _mangaRepository.GetByRatingAsync(id);

        if (manga != null)
        {
            manga.RemoveRating(rating);
            _mangaRepository.MarkAsModified(manga);
        }
        else
        {
            var webNovel = await _webNovelRepository.GetByRatingAsync(id);
            if (webNovel != null)
            {
                webNovel.RemoveRating(rating);
                _webNovelRepository.MarkAsModified(webNovel);
            }
        }

        await _ratingRepository.DeleteAsync(rating);
        await _unitOfWork.CommitAsync();
        return Accepted();
    }
}