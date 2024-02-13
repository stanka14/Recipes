using Newtonsoft.Json;
using Service.Events;

namespace Service.Logger
{
    public class Logger : ILogger
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipiesLog.txt");

        public async Task OnRecipeAdded(object sender, RecipeEventArgs eventArgs)
        {
            var addedRecipe = eventArgs.Recipe;
            await LogMessage($"Recipe added: {addedRecipe.Name}");
        }

        public async Task OnRecipeUpdated(object sender, RecipeEventArgs eventArgs)
        {
            var updatedRecipe = eventArgs.Recipe;
            await LogMessage($"Recipe updated: {JsonConvert.SerializeObject(updatedRecipe)}");
        }

        public async Task OnRecipeDeleted(object sender, RecipeEventArgs eventArgs)
        {
            var deletedRecipe = eventArgs.Recipe;
            await LogMessage($"Recipe deleted: {JsonConvert.SerializeObject(deletedRecipe)}");
        }

        public async Task OnCommentAdded(object sender, CommentsAndRatingsEventArgs eventArgs)
        {
            var comment = eventArgs.Comment.Content;
            await LogMessage($"Comment added: {JsonConvert.SerializeObject(comment)}");
        }

        public async Task OnRateAdded(object sender, CommentsAndRatingsEventArgs eventArgs)
        {
            var rating = eventArgs.Rating;
            await LogMessage($"Rating added: {JsonConvert.SerializeObject(rating)}");
        }

        private async Task LogMessage(string message)
        {
            try
            {
                using StreamWriter writer = File.AppendText(path);
                string logEntry = $"{DateTime.Now}: {message}";
                await writer.WriteLineAsync(logEntry);
            }
            catch (Exception ex)
            {
                await LogErrorMessage(ex.Message);
            }
        }

        private async Task LogErrorMessage(string errorMessage)
        {
            using StreamWriter writer = File.AppendText(path);
            string logEntry = $"{DateTime.Now}: {errorMessage}";
            await writer.WriteLineAsync(logEntry);
        }
    }
}
