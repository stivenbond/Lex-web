using System.Security.Claims;
using Lex.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Lex.Infrastructure.Identity;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid Id => Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

    public string Email => User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

    public string[] Roles => User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? [];

    public string[] Permissions => User?.FindAll("permissions").Select(c => c.Value).ToArray() ?? [];

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public bool HasPermission(string permission) => Permissions.Contains(permission) || Roles.Contains("Admin");
}
