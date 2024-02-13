using AutoMapper;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Service.Events;
using Service.Interfaces;

namespace Service.Services
{
    public class RatingsAndCommentsService : IRatingsAndComments
    {
        private readonly ICommentsAndRatingsRepository _repository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public event CommentsAndRatingsEventHandler? CommentAdded;
        public event CommentsAndRatingsEventHandler? RatingAdded;

        public RatingsAndCommentsService(IRecipeRepository recipeRepository, ICommentsAndRatingsRepository repository, IMapper mapper, INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _recipeRepository = recipeRepository;
        }

        public async Task AddNewComment(CommentDto commentDto)
        {
            try
            {
                var comment = new RecipeComment() { RecipeId = commentDto.RecipeId, UserId = commentDto.UserId, Content = commentDto.Content, DateCommented = DateTime.Now };
                var sender = await _userRepository.GetUserByIdAsync(commentDto.UserId);
                var recipe = await _recipeRepository.GetRecipeByIdAsync(commentDto.RecipeId);

                await _repository.AddCommentAsync(comment);

                var notification = new Notification()
                {
                    IsRead = false,
                    SentAt = DateTime.Now,
                    SenderId = commentDto.UserId,
                    ReceiverId = recipe?.UserId,
                    RecipeId = commentDto.RecipeId,
                    NotificationType = NotificationType.Comment,
                    Message = $"You have a new comment from {sender.UserName}.",
                    RelatedObjectId = recipe.ID
                };

                await _notificationRepository.SendNotificationAsync(notification);

                CommentAdded?.Invoke(this, new CommentsAndRatingsEventArgs(comment));
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding a new comment.", ex);
            }
        }

        public async Task AddNewRating(RatingDto ratingDto)
        {
            try
            {
                var rating = new RecipeRating() { Rating = ratingDto.Rating, RecipeId = ratingDto.RecipeId, UserId = ratingDto.UserId, DateRated = DateTime.Now };
                var sender = await _userRepository.GetUserByIdAsync(ratingDto.UserId);
                var recipe = await _recipeRepository.GetRecipeByIdAsync(ratingDto.RecipeId);

                if (sender == null || recipe == null)
                    return;

                await _repository.AddRatingAsync(rating);

                var notification = new Notification()
                {
                    IsRead = false,
                    SentAt = DateTime.Now,
                    SenderId = ratingDto.UserId,
                    ReceiverId = recipe.UserId,
                    RecipeId = ratingDto.RecipeId,
                    NotificationType = NotificationType.Rate,
                    Message = $"You have a new rating from {sender.UserName}.",
                    RelatedObjectId = recipe.ID
                };

                await _notificationRepository.SendNotificationAsync(notification);

                RatingAdded?.Invoke(this, new CommentsAndRatingsEventArgs(rating));
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding a new rating.", ex);
            }
        }

        public async Task DeleteCommentAsync(string id)
        {
            try
            {
                await _repository.DeleteCommentAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting the comment.", ex);
            }
        }

        public async Task DeleteRatingAsync(string id)
        {
            try
            {
                await _repository.DeleteRatingAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting the rating.", ex);
            }
        }

        public async Task<IEnumerable<CommentDto>> GetAllRecipeCommentsAsync(string loggedInUser, string recipeId)
        {
            try
            {
                var comments = await _repository.GetCommentsByRecipeIdAsync(recipeId);
                var commentDtos = MapRecipeCommentsToRecipeDtos(comments);

                await UpdateNotifications(loggedInUser, comments);

                return commentDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all recipe comments.", ex);
            }
        }

        private IEnumerable<CommentDto> MapRecipeCommentsToRecipeDtos(IEnumerable<RecipeComment> comments)
        {
            var commentDtos = _mapper.Map<IEnumerable<RecipeComment>, IEnumerable<CommentDto>>(comments);
            foreach (var commentDto in commentDtos)
            {
                var correspondingComment = comments.FirstOrDefault(c => c.Id == commentDto.Id);

                if (correspondingComment != null)
                {
                    commentDto.UserName = correspondingComment.User.UserName;
                }
            }
            return commentDtos;
        }

        public async Task<IEnumerable<RatingDto>> GetAllRecipeRatingsAsync(string loggedInUser, string recipeId)
        {
            try
            {
                var ratings = await _repository.GetRatingsByRecipeIdAsync(recipeId);

                await UpdateNotifications(loggedInUser, ratings);

                return _mapper.Map<IEnumerable<RecipeRating>, IEnumerable<RatingDto>>(ratings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all recipe ratings.", ex);
            }
        }

        public async Task<IEnumerable<CommentDto>> GetAllUserCommentsAsync(string userId)
        {
            try
            {
                var comments = await _repository.GetCommentsByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<RecipeComment>, IEnumerable<CommentDto>>(comments);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user comments.", ex);
            }
        }

        public async Task<IEnumerable<RatingDto>> GetAllUserRatingsAsync(string userId)
        {
            try
            {
                var ratings = await _repository.GetRatingsByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<RecipeRating>, IEnumerable<RatingDto>>(ratings);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user ratings.", ex);
            }
        }

        private async Task UpdateNotifications<T>(string loggedInUser, IEnumerable<T> relatedObjects)
        {
            var notifications = await _notificationRepository.GetRecivedNotification(loggedInUser);

            foreach (var notification in notifications)
            {
                if (relatedObjects.Any(obj => obj.GetType().GetProperty("Id").GetValue(obj)?.ToString() == notification.RelatedObjectId))
                {
                    await _notificationRepository.MakeAsRead(notification.ID);
                }
            }
        }
    }
}
