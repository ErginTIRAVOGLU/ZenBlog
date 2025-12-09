export interface BlogModel {
  id: string;
  title: string;
  coverImage: string;
  blogImage: string;
  description: string;
  categoryId: string;
  userId: string;
}


export const initialBlog: BlogModel = {
  id: '',
  title: '',
  coverImage: '',
  blogImage: '',
  description: '',
  categoryId: '',
  userId: ''
}
