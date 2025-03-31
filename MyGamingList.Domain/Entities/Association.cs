namespace MyGamingList.Domain.Entities;

public partial class Association
{
    public int AssociationId { get; set; }

    public string? Name { get; set; }

    public string? Website { get; set; }

    public int Active { get; set; }

    public int Trash { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? DeletedAt { get; set; }
}
