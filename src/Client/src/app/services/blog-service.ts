import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BlogModel } from '../models/blog.model';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  readonly http = inject(HttpClient);
  readonly baseUrl = 'https://localhost:7179/api/blogs';


    getBlogs() {
      return this.http.get<Result<BlogModel[]>>(this.baseUrl);
    }

    getBlogById(id: string) {
      return this.http.get<Result<BlogModel>>(`${this.baseUrl}/${id}`);
    }

    createBlog(blogData: BlogModel) {
      return this.http.post<Result<BlogModel>>(this.baseUrl, blogData );
    }

    updateBlog(id: string, blogData: BlogModel) {
      return this.http.put(`${this.baseUrl}/${id}`, blogData);
    }

    deleteBlog(id: string) {
      return this.http.delete(`${this.baseUrl}/${id}`);
    }

}
