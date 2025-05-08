import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chat } from '../models/chat.model';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private apiURL = 'http://localhost:5075/api/Chats';
  constructor(private http: HttpClient) {}

  public getChats() : Observable<Chat[]> {
    return this.http.get<Chat[]>(this.apiURL).pipe(catchError(this.handleError));
  }
  public getChat(id: string) : Observable<Chat> {
    return this.http.get<Chat>(`${this.apiURL}/${id}`);
  }
  public createChat(chat: Chat) : Observable<Chat> {
    return this.http.post<Chat>(this.apiURL, chat);
  }
  public changeChatModel(id : string, chat:Chat): Observable<Chat> {
    return this.http.put<Chat>(`${this.apiURL}/${id}`, chat);
  }
  public deleteChat(id: string) : Observable<Chat> {
    return this.http.delete<Chat>(`${this.apiURL}/${id}`);
  }
  public uploadFIle(chatId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post(`${this.apiURL}/upload/${chatId}`, formData);
  }
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
