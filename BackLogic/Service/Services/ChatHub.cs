using Dtos.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using System.Security.Claims;

namespace SignalRWebpack.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;

        public ChatHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task SendMessage(MessageDto messageDto)
        {
            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId != messageDto.SenderId && currentUserId != messageDto.ReceiverId)
            {
                await Clients.Caller.SendAsync("errorMessage", "You are not authorized");
            }

            var _userService = _serviceProvider.GetRequiredService<IUserService>();

            var senderExists = await _userService.GetUserByIdAsync(messageDto.SenderId);
            var receiverExists = await _userService.GetUserByIdAsync(messageDto.ReceiverId);

            if (senderExists == null && receiverExists == null)
            {
                await Clients.Caller.SendAsync("errorMessage", "User not found.");
            }

            var _chatService = _serviceProvider.GetRequiredService<IChatService>();

            await _chatService.SendMessageAsync(messageDto);

            await Groups.AddToGroupAsync(Context.ConnectionId, currentUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, messageDto.ReceiverId);

            await Clients.User(messageDto.ReceiverId).SendAsync("messageReceived", messageDto);
            await Clients.User(messageDto.SenderId).SendAsync("messageReceived", messageDto);
        }

        public override async Task OnConnectedAsync()
        {
            var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, currentUserId);
            }

            await base.OnConnectedAsync();
        }
    }
}
