// filepath: d:\piton\PDF_Reader_AI\PDFReaderAI\pdf-reader\src\app\app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ChatService } from './services/chat.service';
import { ChatModule } from './components/chat.module';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes';

@NgModule({
  declarations: [

  ],
  imports: [
    BrowserModule,
    AppComponent,
    ChatModule,
    RouterModule.forRoot(routes)
  ],
  providers: [ ChatService,provideHttpClient()],
  bootstrap:[]
})
export class AppModule { }