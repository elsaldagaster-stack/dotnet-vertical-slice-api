using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Domain.Exceptions;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Users.RegisterUser;

public sealed class RegisterUserHandler(AppDbContext db) : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists) throw new DomainException($"Email '{request.Email}' is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
