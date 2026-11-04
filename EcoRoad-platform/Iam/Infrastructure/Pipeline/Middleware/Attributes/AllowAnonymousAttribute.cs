namespace EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>
/// Marks an action that can be called without a session, even when its controller
/// is decorated with <see cref="AuthorizeAttribute" />.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AllowAnonymousAttribute : Attribute
{
}
