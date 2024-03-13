export interface Message {
  id: string
  senderId: number
  senderUserName: string
  senderPhotoUrl: string
  recipientId: number
  recipientUserName: string
  recipientPhotoUrl: string
  content: string
  dateRead?: Date
  messageSent: Date
}
