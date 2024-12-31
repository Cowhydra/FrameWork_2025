using UnityEngine;

public class Layer : MonoBehaviour
{
    public static readonly int DEFAULT      = LayerMask.NameToLayer("Default");
    public static readonly int UI           = LayerMask.NameToLayer("UI");
    public static readonly int WATER        = LayerMask.NameToLayer("WATER");
    public static readonly int FBX          = LayerMask.NameToLayer("FBX");
    public static readonly int TREE         = LayerMask.NameToLayer("TREE");
    public static readonly int MY_PLAYER    = LayerMask.NameToLayer("MY_PLAYER");
    public static readonly int PLAYER       = LayerMask.NameToLayer("PLAYER");
    public static readonly int MONSTER      = LayerMask.NameToLayer("MONSTER");
}
