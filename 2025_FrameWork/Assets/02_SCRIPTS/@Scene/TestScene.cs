using UnityEngine;
using System.Collections;

public class TestScene : GameScene
{

    private void Start()
    {


    }

    private IEnumerator LoginCO()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            NetWorkObject.Instance.T_LOGIN(NetWorkObject.N_E_LOGIN_TYPE.GOOGLE);
            yield return new WaitForSeconds(2.0f);
            NetWorkObject.Instance.T_LOGIN(NetWorkObject.N_E_LOGIN_TYPE.APPLE);
            yield return new WaitForSeconds(2.0f);
            NetWorkObject.Instance.T_LOGIN(NetWorkObject.N_E_LOGIN_TYPE.EDITOR);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
