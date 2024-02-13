import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from '../services/userservice';
import { ChatService } from '../services/chatservice';
import { User } from '../models/user';
import { Message } from '../models/message';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  messages: Message[] = [];
  newMessage: string = '';
  userId: string = '';
  user: User = {
    id: '',
    username: '',
    firstname: '',
    lastname: '',
    email: ''
  };
  recipientId: string = '';
  reciver: User = {
    id: '',
    username: '',
    firstname: '',
    lastname: '',
    email: ''
  };
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm: string = '';
  chat: boolean = false;

  @ViewChild('messageContainer') messageContainer!: ElementRef; 
  
  constructor(
    private userservice: UserService,
    private chatservice: ChatService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {

    this.userservice.loadCurrentUser().subscribe({
      next: (response) => {
        this.userId = response.id;
        this.user = response;
        this.userservice.getAllUsers().subscribe({
          next: (users: User[]) => {
            this.users = users.filter(x => x.id != this.userId);
            this.filteredUsers = this.users;
          },
          error: (error) => {
            this.processError(error);
          }
        });

        this.chatservice.receiveMessage().subscribe((message) => {
          this.messages.push(message);
          setTimeout(() => {
            this.scrollToBottom();
          });
          this.cdr.detectChanges(); 
        });

      },
      error: (error) => {
        this.processError(error);
      },
    });
  }
  
  sendMessage() {
    if (this.newMessage.trim() !== '') {
      const message = {
        text: this.newMessage,
        senderId: this.userId,
        receiverId: this.recipientId,
        id: '',
        senderName: this.user.username,
        receiverName: this.reciver.username,
        sendAt: new Date()
      };

      this.chatservice.sendMessage(message);

      this.newMessage = '';
    }
  }

  private scrollToBottom() {
    try {
      this.messageContainer.nativeElement.scrollTop = this.messageContainer.nativeElement.scrollHeight;
    } catch (error) {
      this.processError(error);
    }
  }

  searchUser() {
    this.filteredUsers = this.users.filter(user =>
      user.username.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  startChat(recipientId: string) {
    this.recipientId = recipientId;
    this.reciver = this.filteredUsers.find(user => user.id === this.recipientId)!;
    this.loadMessages();
  }

  private loadMessages() {
    this.chatservice.getMessagesWithUser(this.userId, this.recipientId).subscribe({
      next: (messages: Message[]) => {
        this.messages = messages;
        this.chat = true;
        setTimeout(() => {
          this.scrollToBottom();
        });
      },
      error: (error) => {
        this.processError(error);
      },
    });
  }
  
  processError(error)
  {
    if(error.error.errors){
      var errors = error.error.errors;
      Object.keys(errors).forEach(key => {
        this.alertError(`Key: ${key}, Value: ${errors[key][0]}`);
      });   
    }
    else
    {
      this.alertError(error.error);
    }   
  }

  alertError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
    });
    console.error(message);
  }
}
