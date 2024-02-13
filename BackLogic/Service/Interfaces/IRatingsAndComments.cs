using Dtos.Dtos;
using Service.Events;

namespace Service.Interfaces
{
    public interface IRatingsAndComments
    {
        Task<IEnumerable<CommentDto>> GetAllRecipeCommentsAsync(string loggedInUser, string recipeId);
        Task<IEnumerable<RatingDto>> GetAllRecipeRatingsAsync(string loggedInUser, string recipeId);
        Task<IEnumerable<CommentDto>> GetAllUserCommentsAsync(string userId);
        Task<IEnumerable<RatingDto>> GetAllUserRatingsAsync(string userId);
        Task AddNewComment(CommentDto commentDto);
        Task AddNewRating(RatingDto ratingDto);
        Task DeleteCommentAsync(string id);
        Task DeleteRatingAsync(string id);
        event CommentsAndRatingsEventHandler? CommentAdded;
        event CommentsAndRatingsEventHandler? RatingAdded;
    }
}
