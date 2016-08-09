using ExpenditureControl.Data.Domain;
using NHibernate;

namespace ExpenditureControl.Data.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(ISession session) : base(session) { }
    }
}
