export interface Notification {
    id: string;
    message: string;
    sendAt: Date;
    isRead: boolean,
    notificationType: string,
    receiverId: string,
    senderId: string,
    receiverName: string,
    senderName: string,
    recipeId: string
    relatedObjectId: string
  }

