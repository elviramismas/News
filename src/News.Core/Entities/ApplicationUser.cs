using Microsoft.AspNetCore.Identity;

namespace News.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Article> Articles { get; set; }
}