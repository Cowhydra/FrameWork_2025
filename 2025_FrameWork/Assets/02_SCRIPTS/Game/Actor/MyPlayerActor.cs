using UnityEngine;

public class MyPlayerActor : PlayerActor
{
    private void OnEnable()
    {
        RegisterListers(MsnLRT.ADD_LISTENER);
    }

    private void OnDisable()
    {
        RegisterListers(MsnLRT.REMOVE_LISTENER);
    }


    private void RegisterListers(MsnLRT val)
    {
        Messenger<Vector2>.RegisterListener(val, MsgID.CHAR_MOVING_DIRECTION, Moving);
    }


    public void Moving(Vector2 movePos)
    {
        Vector3 movement = new Vector3(movePos.x, 0, movePos.y) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // ĳ���Ͱ� �ٶ󺸴� ���� ���� 
        //transform.forward = new Vector3(movePos.x, movePos.y, 0);

#if ENABLE_INPUT_LOG
        Debug.Log($"move pos x:{movePos.x} , y: {movePos.y}");
        Debug.Log($"forward : {transform.forward}");
#endif

        Vector3 forwardDirection = new Vector3(movePos.x, 0, movePos.y);

        // �̵� ���⿡ ���� ���� �ٶ󺸴� ���� ����
        if (movePos != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection); // �̵� ���������� ȸ�� ���
            transform.rotation = targetRotation;
        }
    }
}
