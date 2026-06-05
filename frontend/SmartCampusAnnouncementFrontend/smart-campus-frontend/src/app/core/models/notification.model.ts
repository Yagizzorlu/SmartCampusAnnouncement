import { UserType, AnnouncementType, NotificationType, NotificationStatus } from './enums';

export interface NotificationLogResponse {
  id: number;
  appUserId: number;
  recipientFullName: string;
  recipientUserType: UserType;
  announcementId: number;
  announcementTitle: string;
  announcementType: AnnouncementType;
  notificationType: NotificationType;
  status: NotificationStatus;
  message: string;
  errorMessage: string | null;
  sentAt: string;
}

export interface NotificationLogSummaryResponse {
  totalLogs: number;
  totalSent: number;
  totalFailed: number;
  byChannel: Record<string, number>;
  byAnnouncementType: Record<string, number>;
}
