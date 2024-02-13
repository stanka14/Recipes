import { Ingredient } from "./ingredient";

export interface ShoppingList {
  id: string;
  userId: string;
  ingredients: Ingredient[];
}
