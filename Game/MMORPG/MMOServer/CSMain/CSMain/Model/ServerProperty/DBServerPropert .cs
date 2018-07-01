using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DB
{
  public class _DBServerPropert
  {
    public virtual int ID { get; set; }
    public virtual string IP { get; set; }
    public virtual string Name { get; set; }
    public virtual int Count { get; set; }
  }

  public class ServerPropertMap : ClassMap<_DBServerPropert>
  {
    public ServerPropertMap()
    {
      LazyLoad();
      Id(x => x.ID).Column("Id");
      Map(x => x.IP).Column("IP");
      Map(x => x.Name).Column("Name");
      Map(x => x.Count).Column("Count");
      Table("ServerPropert");
    }
  }
}


