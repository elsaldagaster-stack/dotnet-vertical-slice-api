using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Comments.AddComment;

public sealed class AddCommentHandler(AppDbContext db) : IRequestHandler<AddCommentCommand, Guid>
{
    public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var issueExists = await db.Issues.AnyAsync(i => i.Id == request.IssueId, cancellationToken);
        if (!issueExists) throw new KeyNotFoundException($"Issue '{request.IssueId}' not found.");

        var authorExists = await db.Users.AnyAsync(u => u.Id == request.AuthorId, cancellationToken);
        if (!authorExists) throw new KeyNotFoundException($"User '{request.AuthorId}' not found.");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            IssueId = request.IssueId,
            AuthorId = request.AuthorId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        db.Comments.Add(comment);
        await db.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}
