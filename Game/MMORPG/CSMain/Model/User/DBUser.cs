
using DB;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * User模型
 */

namespace DB
{
    public partial class _DBUser
    {
        public virtual int Guid { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public _DBUser()
        {

        }
    }
}


