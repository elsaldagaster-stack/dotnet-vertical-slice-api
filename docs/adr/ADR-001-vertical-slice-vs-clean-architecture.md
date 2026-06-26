# ADR-001: Vertical Slice Architecture over Clean Architecture

**Date:** 2026-06-25  
**Status:** Accepted

## Context

For this task management API, we needed to choose between Clean Architecture (layers: Domain, Application, Infrastructure, Presentation) and Vertical Slice Architecture (features as self-contained slices).

## Decision

We chose **Vertical Slice Architecture**.

## Rationale

| Aspect | Clean Architecture | Vertical Slice |
|--------|--------------------|----------------|
| Feature cohesion | Split across 4 layers | All in one folder |
| Adding a feature | Touch 4+ files in 4 places | Touch files in 1 folder |
| Shared abstractions | Repositories, interfaces everywhere | Shared only what's truly shared (EF Core, domain entities) |
| Test locality | Unit tests per layer | Integration tests per feature |
| Over-engineering risk | High (repositories for simple queries) | Low (YAGNI by design) |

## Consequences

**Positive:**
- Adding a feature means creating files in one folder, not across 4 projects
- Integration tests per slice are more valuable than mocked unit tests per layer
- Code that changes together lives together

**Negative:**
- Some duplication across slices (accepted — duplication is cheaper than wrong abstraction)
- Requires discipline to avoid creating shared utilities that couple slices

## Reference

- [Jimmy Bogard — Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)
- [MediatR creator's take on CQRS vs layers](https://lostechies.com/jimmybogard/2015/05/05/cqrs-with-mediatr-and-automapper/)
