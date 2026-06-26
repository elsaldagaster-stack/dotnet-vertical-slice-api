using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Common.Pagination;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Comments.GetComments;

public sealed class GetCommentsHandler(AppDbContext db)
    : IRequestHandler<GetCommentsQuery, CursorPage<CommentDto>>
{
    public async Task<CursorPage<CommentDto>> Handle(
        GetCommentsQuery request, CancellationToken cancellationToken)
        => await db.Comments
            .AsNoTracking()
            .Where(c => c.IssueId == request.IssueId)
            .ToCursorPageAsync(
                request.Cursor, request.Limit,
                c => c.Id,
                c => new CommentDto(c.Id, c.Content, c.AuthorId, c.CreatedAt),
                cancellationToken);
}
