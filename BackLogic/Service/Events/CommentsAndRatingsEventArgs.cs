using Data.Models;

namespace Service.Events
{
    public class CommentsAndRatingsEventArgs : EventArgs
    {
        public RecipeComment Comment { get; }
        public RecipeRating Rating { get; }

        public CommentsAndRatingsEventArgs(RecipeComment comment)
        {
            Comment = comment;
        }
        public CommentsAndRatingsEventArgs(RecipeRating rating)
        {
            Rating = rating;
        }
    }
}
