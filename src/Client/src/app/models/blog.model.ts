import { CategoryModel, initialCategory } from "./category.model";

export interface BlogModel {
  id: string;
  title: string;
  coverImage: string;
  blogImage: string;
  description: string;
  categoryId: string;
  categoryName: string;
  userId: string;
  userName?: string;
  createdAt: Date;
  updatedAt?: Date;
}


export const initialBlog: BlogModel = {
  id: '',
  title: '',
  coverImage: '',
  blogImage: '',
  description: '',
  categoryId: '',
  categoryName: '',
  userId: '',
  userName: '',
  createdAt: new Date(),
  updatedAt: undefined,
}
