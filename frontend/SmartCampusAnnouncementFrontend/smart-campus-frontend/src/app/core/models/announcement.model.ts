import { AnnouncementType, TargetAudience, AnnouncementStatus, AnnouncementPriority } from './enums';

export interface AnnouncementResponse {
  id: number;
  title: string;
  content: string;
  announcementType: AnnouncementType;
  targetAudience: TargetAudience;
  status: AnnouncementStatus;
  priority: AnnouncementPriority;
  publishedAt: string | null;
  expiresAt: string | null;
  createdBy: string;
  createdAt: string;
}

export interface CreateAnnouncementRequest {
  title: string;
  content: string;
  announcementType: AnnouncementType;
  targetAudience: TargetAudience;
  priority: AnnouncementPriority | null;
  expiresAt: string | null;
  createdBy: string | null;
}

export interface PublishAnnouncementResponse {
  announcementId: number;
  totalRecipients: number;
  sentCount: number;
  failedCount: number;
  publishedAt: string;
}
