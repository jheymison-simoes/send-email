using SendEmail.Business.Interfaces.Repositories;
using SendEmail.Business.Models;

namespace SendEmail.Data.Repositories;

public class LogEmailRepository : BaseRepository<LogEmail>, ILogEmailRepository
{
    public LogEmailRepository(SqlContext db) : base(db)
    {
    }
}