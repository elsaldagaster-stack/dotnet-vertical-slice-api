using FluentValidation;

namespace TaskFlow.Api.Features.Issues.UpdateIssue;

public sealed class UpdateIssueValidator : AbstractValidator<UpdateIssueCommand>
{
    public UpdateIssueValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}
