namespace MyGamingList.Domain.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public int Active { get; set; }

    public int Trash { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? DeletedAt { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
