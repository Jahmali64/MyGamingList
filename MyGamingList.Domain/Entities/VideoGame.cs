namespace MyGamingList.Domain.Entities;

public partial class VideoGame
{
    public int VideoGameId { get; set; }

    public string? Name { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int Visible { get; set; }

    public int Trash { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? DeletedAt { get; set; }
}
