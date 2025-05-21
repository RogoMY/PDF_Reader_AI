import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ChatService } from '../../../services/chat.service';
import { Chat } from '../../../models/chat.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'; // dacă folosești destroyRef (Angular ≥16)
   import { v4 as uuidv4 } from 'uuid';          
import { switchMap } from 'rxjs';
@Component({
  selector: 'app-chat-list',
  standalone: true, // Adaugă această linie
  imports: [RouterModule, CommonModule],
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.css'] // corect: styleUrls (nu styleUrl)
})

export class ChatListComponent implements OnInit {
  chats: Chat[] = [];
  chat?: Chat;
newChatTitle = 'New Chat';
  constructor(
    private chatService: ChatService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadChats();
  }

  private loadChats(): void {
    this.chatService.getChats().subscribe(data => this.chats = data);
  }

  createChat(): void {
    const id = uuidv4();
    const nowIso = new Date().toISOString();
    // payload complet conform DTO-ului din backend
    const payload: Chat = {
      id,
      title: this.newChatTitle,
      prompts: [],                  // string array gol
      responses: [],                // string array gol
      timeOfDiscussion: nowIso,     // ISO string parsabil în DateTime
      fileName: null,               // fără fișier inițial
      fileContent: null,            // byte[] nul
      fileMimeType: null            // fără mimetype
    };

    this.chatService.createChat(payload).pipe(
      switchMap(created => this.chatService.getChat(created.id!))
    ).subscribe(chat => {
      this.chat = chat;
      this.router.navigate(['/chat', chat.id]);
    }, err => {
      console.error('Eroare la creare chat:', err);
      alert('A apărut o eroare la creare chat-ului.');
    });
  }
}