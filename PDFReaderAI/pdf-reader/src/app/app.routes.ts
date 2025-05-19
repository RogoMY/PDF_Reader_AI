import { Routes } from '@angular/router';
import { ChatListComponent } from './components/chat/chat-list/chat-list.component';
import { GetChatComponent } from './components/chat/get-chat/get-chat.component';
export const routes: Routes = [
    {path: '', redirectTo: 'chat-list', pathMatch: 'full'},
    {path: 'chat-list', component: ChatListComponent},
    {path: 'chat/:id', component: GetChatComponent}, // Aici ar trebui să fie componenta corespunzătoare pentru detalii chat
];
