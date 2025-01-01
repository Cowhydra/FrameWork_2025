using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public partial class UIScene : MonoBehaviour
{
    private readonly int MAX_NUM_MSG_BOXES = 20;

    private readonly string UIMsgBox_YesNo_Path = @"ui/prefab/msg_box/ui_msg_box_yesno";
    private readonly string UIMsgBox_Ok_Path = @"ui/prefab/msg_box/ui_msg_box_ok";


    public UIMsgBox OpenYesNoMsgBox(string msg, Color? msgColor = null, int userData = 0, System.Action<UIMsgBox, int> onClose = null)
    {
        // HACK: 메시지 박스가 너무 많이 생성된 경우 (뭔가 에러가 났을 것이다.)
        if (MsgBoxParent.childCount > MAX_NUM_MSG_BOXES)
        {
            return null;
        }

        UIMsgBox_YesNo msgBox = AssetServer.InstantiateFromResource<UIMsgBox_YesNo>(UIMsgBox_YesNo_Path, MsgBoxParent);
        if (msgBox == null)
        {
            Debug.LogErrorFormat("UIScene.OpenYesNoMsgBox> Failed to InstantiateFromResource({0})!!", UIMsgBox_YesNo_Path);
            return null;
        }

        msgBox.Open(msg, msgColor, userData, onClose);

        return msgBox;
    }


    public UIMsgBox OpenOKMsgBox(string msg, Color? msgColor = null, int userData = 0, System.Action<UIMsgBox, int> onClose = null)
    {
        // HACK: 메시지 박스가 너무 많이 생성된 경우 (뭔가 에러가 났을 것이다.)
        if (MsgBoxParent.childCount > MAX_NUM_MSG_BOXES)
        {
            return null;
        }

        UIMsgBox_OK msgBox = AssetServer.InstantiateFromResource<UIMsgBox_OK>(UIMsgBox_Ok_Path, MsgBoxParent);
        if (msgBox == null)
        {
            Debug.LogErrorFormat("UIScene.OpenOKMsgBox> Failed to InstantiateFromResource({0})!!", UIMsgBox_Ok_Path);
            return null;
        }

        msgBox.Open(msg, msgColor, userData, onClose);

        return msgBox;
    }
}
