import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Home } from './pages/home/home';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { Category } from './pages/admin/category/category';
import { Dashboard } from './pages/admin/dashboard/dashboard';
import { Blog } from './pages/admin/blog/blog';

const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: '',
        component: Home,
      },
    ],
  },
  {
    path: 'admin',
    component: AdminLayout,
    children: [
      {
        path: '',
        component: Dashboard,
      },
      {
        path: 'category',
        component: Category,
      },
      {
        path: 'blog',
        component: Blog,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
