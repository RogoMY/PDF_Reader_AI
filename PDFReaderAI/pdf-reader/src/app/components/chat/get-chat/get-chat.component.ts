import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { filter, map, switchMap, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Chat } from '../../../models/chat.model';
import { ChatService } from '../../../services/chat.service';
import { ChatListComponent } from '../chat-list/chat-list.component';
@Component({
  selector: 'app-get-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, ChatListComponent],
  templateUrl: './get-chat.component.html',
  styleUrls: ['./get-chat.component.css']
})
export class GetChatComponent implements OnInit {
  chat?: Chat;
  newPrompt = '';
  private destroy$ = new Subject<void>();
  modelOptions: string[] = ['neural-chat', 'llama3.2'];
  selectedModel = this.modelOptions[0];
  constructor(private route: ActivatedRoute,
              private chatService: ChatService) {}

  ngOnInit(): void {
    this.route.paramMap.pipe(
      map(p => p.get('id')),
      filter((id): id is string => !!id),      // ignoră null
      switchMap(id => this.chatService.getChat(id)),
      takeUntil(this.destroy$)
    ).subscribe(chat => {
      this.chat = chat;
      this.newPrompt = '';
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

sendPrompt(): void {
    if (!this.chat || !this.newPrompt.trim()) return;
    const chatId = this.chat.id!;
    this.chatService.interactWithChat(chatId, {
      aiModel: this.selectedModel,  // folosim modelul selectat din dropdown
      prompt: this.newPrompt
    }).pipe(
      switchMap(() => this.chatService.getChat(chatId))
    ).subscribe(chat => {
      this.chat = chat;
      this.newPrompt = '';
    });
  }
attachFile(event: Event): void {
  const input = event.target as HTMLInputElement;
  if (!this.chat || !input.files?.length) return;

  const file = input.files[0];
  const allowed = ['application/pdf','image/jpeg','text/plain'];
  if (!allowed.includes(file.type)) {
    alert('Alege doar PDF, JPEG sau TXT.');
    return;
  }

  const reader = new FileReader();
  reader.onload = () => {
    // reader.result = data:<mime>;base64,<base64-data>
    const dataUrl = reader.result as string;
    const base64 = dataUrl.split(',')[1];

    const updated: Chat = {
      ...this.chat!,
      fileName: file.name,
      fileContent: base64,    // ← trimitem stringul Base64
      fileMimeType: file.type
    };

    this.chatService.updateChat(updated.id!, updated).pipe(
      switchMap(() => this.chatService.getChat(updated.id!))
    ).subscribe({
      next: refreshed => this.chat = refreshed,
      error: err => {
        console.error('Eroare la atașarea fișierului:', err);
        alert('Nu am putut atașa fișierul. Vezi consola.');
      }
    });
  };
  reader.readAsDataURL(file);
}


}
