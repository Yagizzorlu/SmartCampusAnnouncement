using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Abstractions.Factories;

public interface IAnnouncementFactory
{
    Announcement Create(CreateAnnouncementRequest request);
}
