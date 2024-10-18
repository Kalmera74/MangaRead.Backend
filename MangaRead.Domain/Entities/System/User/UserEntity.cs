using MangaRead.Domain.Common.Validators;
using MangaRead.Domain.Entities.Readables.Manga;
using MangaRead.Domain.Entities.Readables.WebNovel;

namespace MangaRead.Domain.Entities.System.User;

public sealed class UserEntity : Entity
{
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }


    public List<WebNovelEntity> BookMarkedWebNovels { private set; get; } = new List<WebNovelEntity>();
    public List<MangaEntity> BookMarkedMangas { private set; get; } = new List<MangaEntity>();
    public List<RoleEntity> Roles { get; private set; } = new List<RoleEntity>();

#pragma warning disable CS8618

    private UserEntity(Guid id) : base(id)
    {
    }
#pragma warning restore CS8618

    public static UserEntity Create(string userName, string email, string password, RoleEntity role)
    {
        ValidateUserName(userName);
        ValidateEmail(email);
        ValidatePassword(password);
        ValidateRole(role);

        var userId = Guid.NewGuid();
        var user = new UserEntity(userId)
        {
            UserName = userName,
            Email = email,
            Password = password
        };

        return user;
    }

    public void SetUserName(string userName)
    {
        ValidateUserName(userName);
        UserName = userName;
        SetUpdatedAt();
    }
    public void SetEmail(string email)
    {
        ValidateEmail(email);
        Email = email;
        SetUpdatedAt();
    }
    public void SetPassword(string password)
    {
        ValidatePassword(password);
        Password = password;
        SetUpdatedAt();
    }

    public void AddRole(RoleEntity role)
    {
        ValidateRole(role);
        if (!Roles.Contains(role))
        {
            Roles.Add(role);
            role.AddUser(this);
            SetUpdatedAt();
        }
    }
    public void RemoveRole(RoleEntity role)
    {
        ValidateRole(role);
        if (Roles.Contains(role))
        {
            Roles.Remove(role);
            role.RemoveUser(this);
            SetUpdatedAt();
        }
    }



    private static void ValidateRole(RoleEntity userRole)
    {
        Guard.AgainstNull(userRole, nameof(userRole));
    }

    private static void ValidatePassword(string userPassword)
    {
        Guard.AgainstNullOrEmpty(userPassword, nameof(userPassword));
    }

    private static void ValidateEmail(string userEmail)
    {
        Guard.AgainstNullOrEmpty(userEmail, nameof(userEmail));
    }

    private static void ValidateUserName(string userName)
    {
        Guard.AgainstNullOrEmpty(userName, nameof(userName));
    }


}
