using MediatR;

namespace TaskFlow.Api.Features.Comments.DeleteComment;

public record DeleteCommentCommand(Guid Id) : IRequest<Unit>;
