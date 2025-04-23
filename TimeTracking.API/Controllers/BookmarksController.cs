using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Auth;
using TimeTracking.Application.Services;

namespace TimeTracking.API.Controllers;

[ApiController]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarksController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    [Authorize]
    [HttpPut(ApiEndpoints.TimeEntries.Bookmark)]
    public async Task<IActionResult> BookmarkTimeEntry([FromRoute] Guid id,
        [FromBody] BookmarkTimeEntryRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _bookmarkService.BookmarkTimeEntryAsync(userId!.Value, request.Bookmark, id, token);
        return result ? Ok() : NotFound();
    }
}

public class BookmarkTimeEntryRequest
{
    public required bool Bookmark { get; set; }
}