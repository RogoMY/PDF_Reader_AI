// filepath: d:\piton\PDF_Reader_AI\PDFReaderAI\pdf-reader\src\app\app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { ChatService } from './services/chat.service';
import { ChatModule } from './components/chat.module';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes';
import { CommonModule } from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
@NgModule({
  declarations: [

  ],
  imports: [
    BrowserModule,
    ChatModule,
    RouterModule.forRoot(routes),
    CommonModule,
    HttpClientModule
  ],
  providers: [ ChatService,provideHttpClient(withInterceptorsFromDi())],
  bootstrap:[]
})
export class AppModule { }