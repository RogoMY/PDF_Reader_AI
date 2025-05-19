// ...existing code...
import { FormsModule } from '@angular/forms'; // adaugă FormsModule
import { Chat } from '../../../models/chat.model';
import { ChatService } from '../../../services/chat.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-get-chat',
  standalone: true,
  imports: [CommonModule, FormsModule], // adaugă FormsModule aici
  templateUrl: './get-chat.component.html',
  styleUrls: ['./get-chat.component.css']
})
export class GetChatComponent implements OnInit {
  chat?: Chat;
  newPrompt: string = '';

  constructor(
    private chatService: ChatService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const chatId = this.route.snapshot.paramMap.get('id');
    if (chatId) {
      this.chatService.getChat(chatId).subscribe(chat => {
        this.chat = chat;
      });
    }
  }

sendPrompt() {
  if (!this.newPrompt.trim() || !this.chat) return;
  const chatId = this.chat.id!;
  const payload = {
    aiModel: 'neural-chat',
    prompt: this.newPrompt // cheia corectă!
  };
  this.chatService.interactWithChat(chatId, payload).subscribe(() => {
    this.chatService.getChat(chatId).subscribe(chat => {
      this.chat = chat;
      this.newPrompt = '';
    });
  });
}
}
// ...existing code...