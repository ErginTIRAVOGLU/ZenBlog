import { Blog } from "./blog.model";

export interface CategoryModel {
  id: string;
  categoryName: string;
  blogs: Blog[];
}

export const initialCategory: CategoryModel = {
  id: '',
  categoryName: '',
  blogs: []
}
