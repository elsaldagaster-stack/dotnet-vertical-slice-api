using FluentValidation;

namespace TaskFlow.Api.Features.Issues.CreateIssue;

public sealed class CreateIssueValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.ReporterId).NotEmpty();
    }
}
