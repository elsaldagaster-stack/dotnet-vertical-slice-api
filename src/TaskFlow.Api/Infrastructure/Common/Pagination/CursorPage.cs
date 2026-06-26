using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Infrastructure.Common.Pagination;

public record CursorPage<T>(
    IReadOnlyList<T> Items,
    string? NextCursor,
    bool HasMore,
    int Count);

public static class CursorPageExtensions
{
    public static async Task<CursorPage<TDto>> ToCursorPageAsync<TEntity, TDto>(
        this IQueryable<TEntity> query,
        string? cursor,
        int limit,
        Func<TEntity, Guid> idSelector,
        Func<TEntity, TDto> mapper,
        CancellationToken ct)
        where TEntity : class
    {
        if (limit is < 1 or > 100) limit = 20;

        if (cursor is not null)
        {
            try
            {
                var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
                if (Guid.TryParse(decoded, out var cursorId))
                {
                    query = query.Where(e => EF.Property<Guid>(e, "Id").CompareTo(cursorId) > 0);
                }
            }
            catch { /* ignore invalid cursor */ }
        }

        var items = await query
            .OrderBy(e => EF.Property<Guid>(e, "Id"))
            .Take(limit + 1)
            .ToListAsync(ct);

        var hasMore = items.Count > limit;
        if (hasMore) items.RemoveAt(items.Count - 1);

        var nextCursor = hasMore
            ? Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(idSelector(items[^1]).ToString()))
            : null;

        return new CursorPage<TDto>(items.Select(mapper).ToList(), nextCursor, hasMore, items.Count);
    }
}
