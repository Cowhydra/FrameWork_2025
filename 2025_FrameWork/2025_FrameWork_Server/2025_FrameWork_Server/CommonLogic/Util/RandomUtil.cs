using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025_FrameWork_Server.CommonLogic.Util
{
    internal class RandomUtil
    {
        private static Random random = new Random();
        public static int Range(int min, int max)
        {
            return random.Next(min, max + 1); // max 포함
        }

        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}
