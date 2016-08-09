using FluentNHibernate.Mapping;
using ExpenditureControl.Data.Domain;

namespace ExpenditureControl.Data.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Area");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("id");
            Map(x => x.Login).Column("login").Not.Nullable().Length(45);
            Map(x => x.Password).Column("pass").Not.Nullable().Length(45);
            Map(x => x.Name).Column("name").Not.Nullable().Length(45);

        }
    }
}
