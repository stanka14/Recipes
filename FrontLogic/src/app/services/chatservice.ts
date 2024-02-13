import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Message } from '../models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatApiUrl = 'http://localhost:63855/api/chat'; // Promijenite na svoj URL
  private hubConnection!: HubConnection;
  private headers: HttpHeaders = new HttpHeaders({})

  constructor(private http: HttpClient) {
    this.initializeSignalR();
  }

  private initializeSignalR() {
    const options: signalR.IHttpConnectionOptions = { 
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
      accessTokenFactory: () => {
        const jwtToken = localStorage.getItem('token');
        return jwtToken || '';
      }
    };

    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:63855/hub', options) 
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection.onclose(async () => {
      await this.startConnection();
    });

    this.startConnection();
  }

  private async startConnection() {
    try {
      await this.hubConnection.start();
      console.log('SignalR connected.');
    } catch (error) {
      console.error('Error starting SignalR:', error);
    }
  }

  sendMessage(message: Message) {
    this.hubConnection.invoke('SendMessage', message);
  }

  receiveMessage(): Observable<Message> {
    return new Observable<Message>((observer) => {
      this.hubConnection.on('messageReceived', (message: any) => {
        observer.next(message);
      });
    });
  }

  getRecivedMessages(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.chatApiUrl}/messages?userId=${userId}`);
  }

  getMessagesWithUser(loggedInUser: string, withUserId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.chatApiUrl}/messages-between/${loggedInUser}/${withUserId}`);
  }

  getSentMessages(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.chatApiUrl}/messages?userId=${userId}`);
  }
}
