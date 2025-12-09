import { Component, inject, OnInit, signal } from '@angular/core';
import { BlogService } from '../../../services/blog-service';

declare var alertify: any;

@Component({
  selector: 'app-blog',
  standalone: false,
  templateUrl: './blog.html',
  styleUrl: './blog.css',
})
export class Blog implements OnInit {
  readonly blogs = signal<Array<any>>([]);
  readonly blogService=inject(BlogService);

  ngOnInit(): void {
    this.getBlogs();
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

  editBlog(blog: any) {
    // Deep copy yaparak liste ile bağımlılığı kesiyoruz
    const blogCopy = { ...blog };
  }

  deleteBlog(id: string) {
    alertify.confirm("Delete Blog", "Are you sure you want to delete this blog?",
      () => {
        this.blogService.deleteBlog(id).subscribe({
          next: () => {
            alertify.success("Blog deleted successfully.");
            this.getBlogs(); // Refresh the blog list
          },
          error: (err) => {
            console.error('Error deleting blog:', err.message);
            alertify.error('An error occurred while deleting the blog.');
          }
        });
      });
  }
}
