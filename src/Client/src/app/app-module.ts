import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Home } from './pages/home/home';
import { Category } from './pages/admin/category/category';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { Dashboard } from './pages/admin/dashboard/dashboard';
import { FormsModule } from '@angular/forms';
import { SweetalertService } from './services/sweetalert-service';

@NgModule({
  declarations: [
    App,
    AdminLayout,
    MainLayout,
    Home,
    Category,
    Dashboard
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [App]
})
export class AppModule { }
