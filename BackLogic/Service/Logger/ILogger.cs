using Service.Events;

namespace Service.Logger
{
    public interface ILogger
    {
        Task OnRecipeAdded(object sender, RecipeEventArgs eventArgs);
        Task OnRecipeUpdated(object sender, RecipeEventArgs eventArgs);
        Task OnRecipeDeleted(object sender, RecipeEventArgs eventArgs);
        Task OnCommentAdded(object sender, CommentsAndRatingsEventArgs eventArgs);
        Task OnRateAdded(object sender, CommentsAndRatingsEventArgs eventArgs);
    }
}
