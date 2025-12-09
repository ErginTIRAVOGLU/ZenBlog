import { initialCategory } from './../../../models/category.model';
import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CategoryService } from '../../../services/category-service';
import { CategoryModel } from '../../../models/category.model';
import { ErrorModel } from '../../../models/error.model';
import { SweetalertService } from '../../../services/sweetalert-service';

declare var alertify: any;

@Component({
  selector: 'app-category',
  standalone: false,
  templateUrl: './category.html',
  styleUrl: './category.css',
})
export class Category implements OnInit {
  readonly categories = signal<CategoryModel[]>([]);
  readonly newCategory = signal<CategoryModel>(initialCategory);
  readonly errors = signal<ErrorModel[]>([]);
  readonly sweetAlert = inject(SweetalertService);

  @ViewChild('createCategoryForm') createCategoryForm?: NgForm;

  ngOnInit(): void {
    this.getCategories();
  }
  readonly categoryService = inject(CategoryService);

  getCategories() {
    this.categoryService.getCategories().subscribe({
      next: (result) => {
        console.log(result);
        if (result.isSuccess && result.data) {
          this.categories.set(result.data);
        }
      },
      error: (err) => {
        console.error('Error fetching categories:', err.message);
        alertify.error('An error occurred while fetching the categories.');
      },
    });
  }

  deleteCategory(id: string) {
    this.sweetAlert
      .confirm('Delete Category', 'Are you sure you want to delete this category?')
      .then((result) => {
        if (result) {
          this.performDeleteCategory(id);
        }
      });
  }

  editCategory(category: CategoryModel) {
    // Deep copy yaparak liste ile bağımlılığı kesiyoruz
    const categoryCopy = { ...category };
    this.newCategory.set(categoryCopy);
  }

  performDeleteCategory(id: string) {
    this.categoryService.deleteCategory(id).subscribe({
      next: () => {
        const updatedCategories = this.categories().filter((c) => c.id !== id);
        this.categories.set(updatedCategories);
        alertify.success('Deleted successfully!').then(() => {
          location.reload();
        });
      },
      error: (err) => {
        console.error('Error deleting category:', err.message);
        alertify.error('An error occurred while deleting the category.');
      },
    });
  }

  createCategory() {
    const category = this.newCategory();

    // Eğer id varsa update, yoksa create
    if (category.id !== null && category.id !== '') {
      this.performUpdateCategory(category);
      return;
    }

    this.performCreateCategory(category);
  }

  performCreateCategory(category: CategoryModel) {
    this.categoryService.createCategory(category).subscribe({
      next: (result) => {
        if (result.isSuccess && result.data) {
          const updatedCategories = [...this.categories(), result.data];
          this.categories.set(updatedCategories);
          this.resetForm();
          this.closeModal();
          alertify.success('Created successfully!');
        }
      },
      error: (err) => {
        alertify.error('An error occurred while saving the category.');
        if (err.error && err.error.errors) {
          this.errors.set(err.error.errors);
        }
      },
    });
  }

  performUpdateCategory(category: CategoryModel) {
    if (!category.id) {
      alertify.error('Category ID is missing.');
      return;
    }

    this.categoryService.updateCategory(category.id, category).subscribe({
      next: () => {
        const index = this.categories().findIndex((c) => c.id === category.id);
        if (index !== -1) {
          const updatedCategories = [...this.categories()];
          updatedCategories[index] = category;
          this.categories.set(updatedCategories);
        }
        this.resetForm();
        this.closeModal();
        alertify.success('Updated successfully!');
      },
      error: (err) => {
        alertify.error('An error occurred while updating the category.');
        if (err.error && err.error.errors) {
          this.errors.set(err.error.errors);
        }
      },
    });
  }

  resetForm() {
    this.newCategory.set(initialCategory);
    this.errors.set([]);
    this.createCategoryForm?.resetForm();
  }

  closeModal() {
    const modalElement = document.getElementById('createModal');
    if (modalElement) {
      const modal = (window as any).bootstrap.Modal.getInstance(modalElement);
      modal?.hide();
    }
  }
}
