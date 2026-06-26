using MediatR;

namespace TaskFlow.Api.Features.Comments.AddComment;

public record AddCommentCommand(Guid IssueId, string Content, Guid AuthorId) : IRequest<Guid>;
