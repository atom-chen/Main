using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class _DBRole
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual uint Level { get; set; }
        public virtual bool Sex { get; set; }
        public virtual int UserID { get; set; }
        public virtual string HeadIcon { get; set; }
        public virtual int Exp { get; set; }
        public virtual int Coin { get; set; }
        public virtual int YuanBao { get; set; }
        public virtual int Energy { get; set; }
        public virtual int Toughen { get; set; }
        public virtual string LastDownLine { get; set; }
        
        //装备
        public virtual int Helm { get; set; }
        public virtual int nCloth { get; set; }
        public virtual int Weapon { get; set; }
        public virtual int Shoes { get; set; }
        public virtual int NeckLace { get; set; }
        public virtual int BraceLet { get; set; }
        public virtual int Ring { get; set; }
        public virtual int Wing { get; set; }

        public _DBRole()
        {

        }

        public _DBRole(Role role)
        {
            try
            {
                ID = role.id;
                Name = role.name;
                Level = role.level;
                Sex = role.sex;
                UserID = role.userID;
                HeadIcon = role.headIcon;
                Exp = role.exp;
                Coin = role.coin;
                YuanBao = role.yuanBao;
                Energy = role.energy;
                Toughen = role.toughen;

                //构建所穿装备信息
                Equip equip = role.equipInfo.GetEquip(0);
                this.Helm = equip.guid;
                equip = role.equipInfo.GetEquip(1);
                this.nCloth = equip.guid;
                equip = role.equipInfo.GetEquip(2);
                this.Weapon = equip.guid;
                equip = role.equipInfo.GetEquip(3);
                this.Shoes = equip.guid;

                equip = role.equipInfo.GetEquip(4);
                this.NeckLace = equip.guid;
                equip = role.equipInfo.GetEquip(5);
                this.BraceLet = equip.guid;
                equip = role.equipInfo.GetEquip(6);
                this.Ring = equip.guid;
                equip = role.equipInfo.GetEquip(7);
                this.Wing = equip.guid;
            }
            catch(Exception ex)
            {
                LogManager.Error("构造DBROLE错误!"+ ex.Message);
            }
            
                
        }

        //是否是DB模型的已装备
        public bool IsEquip(int guid)
        {
            if(guid == Helm || guid == nCloth || guid == Weapon || guid == Shoes || guid == NeckLace || guid == BraceLet || guid == BraceLet 
                || guid == Ring || guid == Wing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class _DBRoleMap : ClassMap<_DBRole>
    {
        public _DBRoleMap()
        {
            LazyLoad();
            Id(x => x.ID).Column("Id");
            Map(x => x.Name).Column("name");
            Map(x => x.Level).Column("Level");
            Map(x => x.Sex).Column("Sex");
            Map(x => x.UserID).Column("UserID");
            Map(x => x.HeadIcon).Column("HeadIcon");
            Map(x => x.Exp).Column("Exp");
            Map(x => x.Coin).Column("coin");
            Map(x => x.YuanBao).Column("YuanBao");
            Map(x => x.Energy).Column("Energy");
            Map(x => x.Toughen).Column("Toughen");
            Map(x => x.LastDownLine).Column("LastDownLine");

            Map(x => x.Helm).Column("Helm");
            Map(x => x.nCloth).Column("Cloth");
            Map(x => x.Weapon).Column("Weapon");
            Map(x => x.Shoes).Column("Shoes");
            Map(x => x.NeckLace).Column("NeckLace");
            Map(x => x.BraceLet).Column("BraceLet");
            Map(x => x.Ring).Column("Ring");
            Map(x => x.Wing).Column("Wing");
            Table("role");
        }
    }
}