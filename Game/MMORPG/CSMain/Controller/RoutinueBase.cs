using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ROUTINUE_CODE
{
    USER,
    DB,
    NET,
}
public abstract class RoutinueBase
{
    public abstract void SetUp();
    public  abstract void Tick();

    public abstract ROUTINUE_CODE Code { get; }

    public abstract int UseResources { get; }

}

