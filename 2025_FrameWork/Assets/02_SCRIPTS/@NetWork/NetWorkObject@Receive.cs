using System;
using UnityEngine;

public partial class NetWorkObject : SingletonObj<NetWorkObject>
{
    public void R_LOGIN(N_E_LOGIN_TYPE loginType)
    {
        Debug.Log($"R_LOGIN {loginType}");
    }
}
