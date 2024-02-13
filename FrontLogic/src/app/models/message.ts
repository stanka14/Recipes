export interface Message {
    id: string;
    text: string,
    receiverId: string,
    receiverName: string,
    senderId: string,
    senderName: string,
    sendAt: Date
}