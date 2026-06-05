import { UserResponse } from './user.model';
import { AnnouncementResponse, PublishAnnouncementResponse } from './announcement.model';
import { NotificationLogResponse } from './notification.model';

export interface DemoPatterns {
  factory: string;
  observer: string;
  singleton: string;
}

export interface DemoScenarioResponse {
  scenario: string;
  patterns: DemoPatterns;
  usersCreated: UserResponse[];
  announcementCreated: AnnouncementResponse;
  publishResult: PublishAnnouncementResponse;
  notificationLogs: NotificationLogResponse[];
}
