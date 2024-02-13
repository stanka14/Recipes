using Dtos.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Service.ErrorHandling;
using Service.Interfaces;

[Route("api/chat")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto message)
    {
        var errorResponse = ValidateModelState();

        if (!errorResponse.Success)
        {
            return BadRequest(errorResponse.Message);
        }

        try
        {
            await _chatService.SendMessageAsync(message);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("receive/{userId}")]
    public async Task<IActionResult> ReceiveMessages(string userId)
    {
        var errorResponse = ValidateModelState();

        if (!errorResponse.Success)
        {
            return BadRequest(errorResponse.Message);
        }

        try
        {
            var messages = await _chatService.GetRecivedMessagesAsync(userId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("messages-between/{loggedInUser}/{withUserId}")]
    public async Task<IActionResult> GetMessagesBetweenUsers(string loggedInUser, string withUserId)
    {
        var errorResponse = ValidateModelState();

        if (!errorResponse.Success)
        {
            return BadRequest(errorResponse.Message);
        }

        try
        {
            var messages = await _chatService.GetMessagesBetweenUsersAsync(loggedInUser, withUserId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("send/{userId}")]
    public async Task<IActionResult> SentMessages(string userId)
    {
        var errorResponse = ValidateModelState();

        if (!errorResponse.Success)
        {
            return BadRequest(errorResponse.Message);
        }

        try
        {
            var messages = await _chatService.GetSentMessagesAsync(userId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private ErrorResponseDto ValidateModelState()
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (!errors.Any())
            return new ErrorResponseDto() { Success = true };

        var errorMessage = string.Join(" ", errors);
        return new ErrorResponseDto { Message = errorMessage, Success = false };
    }
}
