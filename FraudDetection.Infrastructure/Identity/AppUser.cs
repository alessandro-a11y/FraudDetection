using Microsoft.AspNetCore.Identity;

namespace FraudDetection.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}