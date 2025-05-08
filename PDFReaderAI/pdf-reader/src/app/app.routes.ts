import { Routes } from '@angular/router';
import { ChatListComponent } from './components/chat/chat-list/chat-list.component';
export const routes: Routes = [
    {path: '', redirectTo: 'chat-list', pathMatch: 'full'},
    {path: 'chat-list', component: ChatListComponent},
];
