export interface Blog {
  id: string;
  title: string;
  coverImage: string;
  blogImage: string;
  description: string;
  categoryId: string;
  userId: string;
}


export const initialBlog: Blog = {
  id: '',
  title: '',
  coverImage: '',
  blogImage: '',
  description: '',
  categoryId: '',
  userId: ''
}
