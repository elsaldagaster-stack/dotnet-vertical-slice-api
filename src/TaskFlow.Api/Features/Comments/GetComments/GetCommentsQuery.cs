using MediatR;
using TaskFlow.Api.Infrastructure.Common.Pagination;

namespace TaskFlow.Api.Features.Comments.GetComments;

public record GetCommentsQuery(Guid IssueId, string? Cursor, int Limit = 20)
    : IRequest<CursorPage<CommentDto>>;

public record CommentDto(Guid Id, string Content, Guid AuthorId, DateTimeOffset CreatedAt);
