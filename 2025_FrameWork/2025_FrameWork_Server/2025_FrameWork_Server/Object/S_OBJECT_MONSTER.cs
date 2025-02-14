using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _2025_FrameWork_Server.Object
{
    internal class S_OBJECT_MONSTER : S_OBJECT
    {
        protected int UniqueID;
        internal bool HasStateChanged { get; private set; }
        
        public DateTime DieTime { get; set; }

        //현재 위치 ---> 추적전 및 죽을떄 갱신
        public float[] CurPosition { get; set; }


        MonsterData? _MonsterData;

        public S_OBJECT_MONSTER(int hp, int mp, int startX, int startY, int startZ, int uniqueID) : base(hp, mp, startX, startY, startZ)
        {
            UniqueID = uniqueID;
            E_OBJECT_TYPE = D_F_Enum.E_OBJECT_TYPE.MONSTER;
            HasStateChanged = false;
            DataManager.MonsterDataDict.TryGetValue(UniqueID, out _MonsterData);
        }


        public void Revive(int posx,int posY,int posZ)
        {
            HP[0] = _MonsterData!.Health;
            PosX = posx;
            PosY = posY;
            PosZ = posZ;
            CurPosition = GetCord;
        }


        public void ResetStateChangedFlag()
        {
            HasStateChanged = false;
        }


        public S_OBJECT_PLAYER? TargetActor { get; set; }

        public bool IsMoving = false;
        public float MoveSpeed => _MonsterData!.MoveSpeed;
        public float ChaseRange => _MonsterData!.ChaseRange;
        public float Attackpower => _MonsterData!.AttackPower;
        public float AttackRange => _MonsterData!.AttackRange;
        public float DetectRange => _MonsterData!.DetectionRange;
        public D_F_Enum.E_ATTACK_TYPE attackType => _MonsterData!.AttackType;



        public void Update()
        {

        }
    }
}
