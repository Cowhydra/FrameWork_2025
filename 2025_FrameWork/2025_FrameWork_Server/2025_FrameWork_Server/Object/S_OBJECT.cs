using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_FrameWork_Server.Object
{
    public class S_OBJECT
    {
        protected int[] HP { get; set; }
        protected int[] MP { get; set; }
        protected int PosX { get; set; }
        protected int PosY { get; set; }
        protected int PosZ { get; set; }
        protected virtual D_F_Enum.E_OBJECT_TYPE E_OBJECT_TYPE { get; set; }

        public bool IsDead => HP[0] <= 0;

        public float[] GetCord => [PosX, PosY, PosZ];


        public S_OBJECT(int hp,int mp, int startX, int startY, int startZ)
        {
            HP = [hp, hp];
            MP = [mp, mp];
            PosX = startX;
            PosY = startY;
            PosZ = startZ;
        }


        public void HIITED(int damage)
        {
            HP[0] -= damage;

            if (HP[0] <= 0)
            {
                HP[0] = 0;
            }
        }


        public float GetDistance(S_OBJECT obj)
        {
            return MathF.Pow((obj.GetCord[0] - PosX), 2)
                + MathF.Pow((obj.GetCord[0] - PosX), 2) + MathF.Pow((obj.GetCord[0] - PosX), 2);
        }
    }
}
