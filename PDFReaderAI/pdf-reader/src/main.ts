import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { importProvidersFrom } from '@angular/core';
bootstrapApplication(AppComponent,{
  providers:[
    provideRouter(routes),
    importProvidersFrom(
      BrowserAnimationsModule,
      ReactiveFormsModule),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimationsAsync()
  ]
}) .catch((err) => console.error(err));
