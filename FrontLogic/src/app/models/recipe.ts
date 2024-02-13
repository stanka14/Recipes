import { Ingredient } from "./ingredient";

export interface Recipe {
    id: string;
    name: string;
    description: string;
    imageUrl: string;
    userId: string,
    favourite: boolean,
    createdAt: Date,
    category: RecipeCategoryEnum; 
    ingredients: Array<Ingredient>;
  }

  export enum RecipeCategoryEnum {
    MainCourse,
    Dessert,
    Snack,
    Salad
}