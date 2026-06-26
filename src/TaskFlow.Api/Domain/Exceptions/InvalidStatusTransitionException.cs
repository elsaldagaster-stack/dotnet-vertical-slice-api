using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Domain.Exceptions;

public class InvalidStatusTransitionException(IssueStatus from, IssueStatus to)
    : DomainException($"Cannot transition issue from '{from}' to '{to}'.");
