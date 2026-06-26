using FluentValidation;

namespace TaskFlow.Api.Features.Comments.AddComment;

public sealed class AddCommentValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MinimumLength(1).MaximumLength(2000);
        RuleFor(x => x.AuthorId).NotEmpty();
    }
}
