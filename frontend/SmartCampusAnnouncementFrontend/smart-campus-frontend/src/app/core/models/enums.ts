export enum UserType {
  Student = 1,
  Teacher = 2
}

export enum AnnouncementType {
  Exam     = 1,
  Event    = 2,
  FoodMenu = 3,
  Library  = 4
}

export enum TargetAudience {
  Students = 1,
  Teachers = 2,
  All      = 3
}

export enum NotificationType {
  Email = 1,
  Sms   = 2,
  Push  = 3
}

export enum AnnouncementStatus {
  Draft     = 1,
  Published = 2,
  Archived  = 3
}

export enum AnnouncementPriority {
  Low    = 1,
  Normal = 2,
  High   = 3,
  Urgent = 4
}

export enum NotificationStatus {
  Sent   = 1,
  Failed = 2
}

export const UserTypeLabel: Record<UserType, string> = {
  [UserType.Student]: 'Öğrenci',
  [UserType.Teacher]: 'Öğretmen'
};

export const AnnouncementTypeLabel: Record<AnnouncementType, string> = {
  [AnnouncementType.Exam]:     'Sınav',
  [AnnouncementType.Event]:    'Etkinlik',
  [AnnouncementType.FoodMenu]: 'Yemekhane',
  [AnnouncementType.Library]:  'Kütüphane'
};

export const TargetAudienceLabel: Record<TargetAudience, string> = {
  [TargetAudience.Students]: 'Öğrenciler',
  [TargetAudience.Teachers]: 'Öğretmenler',
  [TargetAudience.All]:      'Herkes'
};

export const NotificationTypeLabel: Record<NotificationType, string> = {
  [NotificationType.Email]: 'E-posta',
  [NotificationType.Sms]:   'SMS',
  [NotificationType.Push]:  'Mobil Bildirim'
};

export const AnnouncementStatusLabel: Record<AnnouncementStatus, string> = {
  [AnnouncementStatus.Draft]:     'Taslak',
  [AnnouncementStatus.Published]: 'Yayında',
  [AnnouncementStatus.Archived]:  'Arşivlendi'
};

export const AnnouncementPriorityLabel: Record<AnnouncementPriority, string> = {
  [AnnouncementPriority.Low]:    'Düşük',
  [AnnouncementPriority.Normal]: 'Normal',
  [AnnouncementPriority.High]:   'Yüksek',
  [AnnouncementPriority.Urgent]: 'Acil'
};

export const NotificationStatusLabel: Record<NotificationStatus, string> = {
  [NotificationStatus.Sent]:   'Gönderildi',
  [NotificationStatus.Failed]: 'Başarısız'
};
