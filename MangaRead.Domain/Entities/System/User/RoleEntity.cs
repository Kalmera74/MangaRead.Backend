using MangaRead.Domain.Common.Validators;

namespace MangaRead.Domain.Entities.System.User;

public sealed class RoleEntity : Entity
{

    public string Name { get; private set; }

    private IList<UserEntity> _users = new List<UserEntity>();

    public IList<UserEntity> Users => _users.AsReadOnly();



#pragma warning disable CS8618

    private RoleEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static RoleEntity Create(string name)
    {
        ValidateName(name);

        var roleId = Guid.NewGuid();
        var role = new RoleEntity(roleId)
        {
            Name = name
        };
        return role;
    }

    public void SetName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public void AddUser(UserEntity user)
    {
        ValidateUser(user);

        if (!_users.Contains(user))
        {
            _users.Add(user);
            user.AddRole(this);
            SetUpdatedAt();
        }
    }

    public void RemoveUser(UserEntity user)
    {
        ValidateUser(user);

        if (_users.Contains(user))
        {
            _users.Remove(user);
            user.RemoveRole(this);
            SetUpdatedAt();
        }
    }

    private static void ValidateUser(UserEntity roleUser)
    {
        Guard.AgainstNull(roleUser, nameof(roleUser));
    }

    private static void ValidateName(string roleName)
    {
        Guard.AgainstNullOrEmpty(roleName, nameof(roleName));
    }
}
