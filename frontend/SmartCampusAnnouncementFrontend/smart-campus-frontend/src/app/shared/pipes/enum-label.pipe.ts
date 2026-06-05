import { Pipe, PipeTransform } from '@angular/core';
import {
  UserType, AnnouncementType, TargetAudience, NotificationType,
  UserTypeLabel, AnnouncementTypeLabel, TargetAudienceLabel, NotificationTypeLabel
} from '../../core/models/enums';

type LabelMap = 'userType' | 'announcementType' | 'targetAudience' | 'notificationType';

@Pipe({ name: 'enumLabel', standalone: true, pure: true })
export class EnumLabelPipe implements PipeTransform {
  transform(value: number, map: LabelMap): string {
    switch (map) {
      case 'userType':         return UserTypeLabel[value as UserType]                 ?? String(value);
      case 'announcementType': return AnnouncementTypeLabel[value as AnnouncementType] ?? String(value);
      case 'targetAudience':   return TargetAudienceLabel[value as TargetAudience]     ?? String(value);
      case 'notificationType': return NotificationTypeLabel[value as NotificationType] ?? String(value);
    }
  }
}
