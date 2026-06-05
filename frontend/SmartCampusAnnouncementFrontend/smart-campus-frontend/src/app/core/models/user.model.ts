import { UserType, NotificationType } from './enums';

export interface UserResponse {
  id: number;
  fullName: string;
  email: string;
  phoneNumber: string | null;
  userType: UserType;
  isActive: boolean;
  notificationTypes: NotificationType[];
  createdAt: string;
}

export interface CreateUserRequest {
  fullName: string;
  email: string;
  phoneNumber: string | null;
  userType: UserType;
  notificationTypes: NotificationType[];
}
