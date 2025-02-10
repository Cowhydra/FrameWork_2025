using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_FrameWork_Server.Object
{
    internal class S_OBJECT
    {
        protected int[] HP { get; set; }
        protected int[] MP { get; set; }
        protected float PosX { get; set; }
        protected float PosY { get; set; }
        protected float PosZ { get; set; }
        protected virtual D_F_Enum.E_OBJECT_TYPE E_OBJECT_TYPE { get; set; }

        protected bool IsDead => HP[0] <= 0;

        public float[] GetCord => [PosX, PosY, PosZ];


        public S_OBJECT(int hp,int mp,float startX,float startY,float startZ)
        {
            HP = [hp, hp];
            MP = [mp, mp];
            PosX = startX;
            PosY = startY;
            PosZ = startZ;
        }
    }
}
