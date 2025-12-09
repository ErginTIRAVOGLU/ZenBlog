import { BlogModel } from "./blog.model";

export interface CategoryModel {
  id: string;
  categoryName: string;
  blogs: BlogModel[];
}

export const initialCategory: CategoryModel = {
  id: '',
  categoryName: '',
  blogs: []
}
