using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_FrameWork_Server.Object
{
    internal class S_OBJECT_MONSTER: S_OBJECT
    {
        protected int UniqueID;
        internal bool HasStateChanged { get; private set; }
        public DateTime DieTime { get; set; }


        public S_OBJECT_MONSTER(int hp, int mp, float startX, float startY, float startZ, int uniqueID) : base(hp, mp, startX, startY, startZ)
        {
            UniqueID = uniqueID;
            E_OBJECT_TYPE = D_F_Enum.E_OBJECT_TYPE.MONSTER;
            HasStateChanged = false;
        }


        public void ResetStateChangedFlag()
        {
            HasStateChanged = false;
        }
    }
}
