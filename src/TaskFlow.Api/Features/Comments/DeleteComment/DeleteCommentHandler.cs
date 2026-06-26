using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Comments.DeleteComment;

public sealed class DeleteCommentHandler(AppDbContext db) : IRequestHandler<DeleteCommentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Comment '{request.Id}' not found.");

        db.Comments.Remove(comment);
        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
