using UnityEngine;

public partial class GameLogic : MonoBehaviour
{
    private StageData _StageData;

    public StageData CurStage => _StageData;



    public bool SetUpStage(int stage)
    {
        if(AssetServer.StageDataDict.Value.TryGetValue(stage, out StageData _SetStageData) == false)
        {
            return false;
        }
        else
        {
            _StageData = _SetStageData;
        }

        return true;
    }


    public bool SetUpNextStage()
    {
        return SetUpStage(_StageData.StageIndex + 1);
    }
}
