import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CategoryModel } from '../models/category.model';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  readonly http = inject(HttpClient);
  readonly baseUrl = 'https://localhost:7179/api/categories';

  getCategories() {
    return this.http.get<Result<CategoryModel[]>>(this.baseUrl);
  }

  getCategoryById(id: string) {
    return this.http.get<Result<CategoryModel>>(`${this.baseUrl}/${id}`);
  }

  createCategory(categoryData: CategoryModel) {
    return this.http.post<Result<CategoryModel>>(this.baseUrl, categoryData);
  }

  updateCategory(id: string, categoryData: CategoryModel) {
    return this.http.put(`${this.baseUrl}/${id}`, categoryData);
  }

  deleteCategory(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

}
