using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Auth;
using TimeTracking.API.Mapping;
using TimeTracking.Application.Services;
using TimeTracking.Contracts.Requests;

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
        var result = await _bookmarkService.BookmarkTimeEntryAsync(id, userId!.Value, request.Bookmark, token);
        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpDelete(ApiEndpoints.TimeEntries.DeleteBookmark)]
    public async Task<IActionResult> DeleteBookmark([FromRoute] Guid id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var result = await _bookmarkService.DeleteBookmarkAsync(id, userId!.Value, token);
        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpGet(ApiEndpoints.Bookmarks.GetUserBookmarks)]
    public async Task<IActionResult> GetUserBookmarks(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var bookmarks = await _bookmarkService.GetBookmarksForUserAsync(userId!.Value, token);
        var bookmarksResponse = bookmarks.MapToResponse();
        return Ok(bookmarksResponse);
    }
}