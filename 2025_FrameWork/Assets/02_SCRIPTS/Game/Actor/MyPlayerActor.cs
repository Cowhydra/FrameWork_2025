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

        // 캐릭터가 바라보는 방향 설정 
        //transform.forward = new Vector3(movePos.x, movePos.y, 0);

#if ENABLE_INPUT_LOG
        Debug.Log($"move pos x:{movePos.x} , y: {movePos.y}");
        Debug.Log($"forward : {transform.forward}");
#endif

        Vector3 forwardDirection = new Vector3(movePos.x, 0, movePos.y);

        // 이동 방향에 따라 모델이 바라보는 방향 설정
        if (movePos != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection); // 이동 방향으로의 회전 계산
            transform.rotation = targetRotation;
        }
    }
}
