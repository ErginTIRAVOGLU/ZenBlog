import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { BlogService } from '../../../services/blog-service';
import { BlogModel, initialBlog } from '../../../models/blog.model';
import { ErrorModel } from '../../../models/error.model';
import { SweetalertService } from '../../../services/sweetalert-service';
import { CategoryService } from '../../../services/category-service';
import { CategoryModel } from '../../../models/category.model';
import { NgForm } from '@angular/forms';

declare var alertify: any;

@Component({
  selector: 'app-blog',
  standalone: false,
  templateUrl: './blog.html',
  styleUrl: './blog.css',
})
export class Blog implements OnInit {
  readonly blogs = signal<BlogModel[]>([]);
  readonly newBlog = signal<BlogModel>(initialBlog);
  readonly errors = signal<ErrorModel[]>([]);
  readonly categories = signal<CategoryModel[]>([]);
  readonly blogService = inject(BlogService);
  readonly categoryService = inject(CategoryService);
  readonly sweetAlert = inject(SweetalertService);

  @ViewChild('createBlogForm') createBlogForm?: NgForm;

  ngOnInit(): void {
    this.getBlogs();
    this.getCategories();
  }

  getCategories() {
    this.categoryService.getCategories().subscribe({
      next: (result) => {
        if (result.isSuccess && result.data) {
          this.categories.set(result.data);
        }
      },
      error: (err) => {
        console.error('Error fetching categories:', err.message);
        alertify.error('An error occurred while fetching the categories.');
      }
    });
  }

  getBlogs() {
    this.blogService.getBlogs().subscribe({
      next: (result) => {
        if (result.isSuccess && result.data) {

          this.blogs.set(result.data);
        }
      },
      error: (err) => {
        console.error('Error fetching blogs:', err.message);
        alertify.error('An error occurred while fetching the blogs.');
      }
    });
  }

  editBlog(blog: BlogModel) {
    const blogCopy = { ...blog };
    this.newBlog.set(blogCopy);
  }

  addNewBlog() {
    this.resetForm();
  }

  deleteBlog(id: string) {
    this.sweetAlert
      .confirm('Delete Blog', 'Are you sure you want to delete this blog?')
      .then((result) => {
        if (result) {
          this.performDeleteBlog(id);
        }
      });
  }

  performDeleteBlog(id: string) {
    this.blogService.deleteBlog(id).subscribe({
      next: () => {
        const updatedBlogs = this.blogs().filter((b) => b.id !== id);
        this.blogs.set(updatedBlogs);
        alertify.success('Deleted successfully!').then(() => {
          location.reload();
        });
      },
      error: (err) => {
        console.error('Error deleting blog:', err.message);
        alertify.error('An error occurred while deleting the blog.');
      }
    });
  }

  createBlog() {
    const blog = this.newBlog();

    if (blog.id !== null && blog.id !== '') {
      this.performUpdateBlog(blog);
      return;
    }

    this.performCreateBlog(blog);
  }

  performCreateBlog(blog: BlogModel) {
    this.blogService.createBlog(blog).subscribe({
      next: (result) => {
        if (result.isSuccess && result.data) {
          this.getBlogs();
          this.resetForm();
          this.closeModal();
          alertify.success('Created successfully!');
        }
      },
      error: (err) => {
        alertify.error('An error occurred while saving the blog.');
        if (err.error && err.error.errors) {
          this.errors.set(err.error.errors);
        }
      }
    });
  }

  performUpdateBlog(blog: BlogModel) {
    if (!blog.id) {
      alertify.error('Blog ID is missing.');
      return;
    }

    this.blogService.updateBlog(blog.id, blog).subscribe({
      next: () => {
        this.getBlogs();
        this.resetForm();
        this.closeModal();
        alertify.success('Updated successfully!');
      },
      error: (err) => {
        alertify.error('An error occurred while updating the blog.');
        if (err.error && err.error.errors) {
          this.errors.set(err.error.errors);
        }
      }
    });
  }

  resetForm() {
    this.newBlog.set(initialBlog);
    this.errors.set([]);
    this.createBlogForm?.resetForm();
  }

  closeModal() {
    const modalElement = document.getElementById('createModal');
    if (modalElement) {
      const modal = (window as any).bootstrap.Modal.getInstance(modalElement);
      modal?.hide();
    }
  }

  logFormStatus() {
    if (this.createBlogForm) {
      console.log('Form Status:', {
        valid: this.createBlogForm.valid,
        invalid: this.createBlogForm.invalid,
        pristine: this.createBlogForm.pristine,
        dirty: this.createBlogForm.dirty,
        submitted: this.createBlogForm.submitted,
        controls: Object.keys(this.createBlogForm.controls).map(key => ({
          name: key,
          value: this.createBlogForm?.controls[key].value,
          valid: this.createBlogForm?.controls[key].valid,
          invalid: this.createBlogForm?.controls[key].invalid,
          errors: this.createBlogForm?.controls[key].errors
        }))
      });
    }
  }
}
