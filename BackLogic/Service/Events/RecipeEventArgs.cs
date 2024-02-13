using Dtos.Dtos;

namespace Service.Events
{
    public class RecipeEventArgs : EventArgs
    {
        public RecipeDto Recipe { get; }

        public RecipeEventArgs(RecipeDto recipe)
        {
            Recipe = recipe;
        }
    }
}
