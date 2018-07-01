using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ServerPropert
{
  public ServerPropert(DB._DBServerPropert dbServer)
  {
    this.ID = dbServer.ID;
    IP = dbServer.IP;
    Name = dbServer.Name;
    Count = dbServer.Count;
  }
  public void CopyForm(DB._DBServerPropert dbServer)
  {
    this.ID = dbServer.ID;
    IP = dbServer.IP;
    Name = dbServer.Name;
    Count = dbServer.Count;
  }
}




