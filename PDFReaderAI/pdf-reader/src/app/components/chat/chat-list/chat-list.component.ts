import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ChatService } from '../../../services/chat.service';
import { Chat } from '../../../models/chat.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat-list',
  standalone: true, // Adaugă această linie
  imports: [RouterModule, CommonModule],
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.css'] // corect: styleUrls (nu styleUrl)
})
export class ChatListComponent implements OnInit {
  chats: Chat[] = [];
  constructor(private chatService: ChatService, private router: Router) { }
  ngOnInit(): void {
    this.chatService.getChats().subscribe((data: Chat[]) => {
      this.chats = data;
    });
  }
goToChat(id: string | undefined) {
  this.router.navigate(['/chat', id]);
}
}