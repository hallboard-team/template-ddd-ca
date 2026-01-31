namespace Ca.Domain.Modules.Common.Base;

/// <summary>
/// Marks entities as append-only, meaning they should not use optimistic concurrency tokens like xmin.
/// </summary>
public interface IAppendOnly;
