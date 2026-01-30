using Ca.Domain.Modules.Auth.Aggregates;
using Ca.Domain.Modules.Auth.Results;
using Ca.Domain.Modules.Message;

namespace Ca.Infrastructure.Modules.Message.Postgres;

public class MessageRepositoryPostgres : IMessageRepository
{
    public async Task<RegisterResult> CreateAsync(AppUser appUser, CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}