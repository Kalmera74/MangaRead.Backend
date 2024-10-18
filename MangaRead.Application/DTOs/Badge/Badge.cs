namespace MangaRead.Application.DTOs.Badge;
public enum BadgeType
{
    New,
    Hot
}


public record BadgeDTO(
    string Name
);