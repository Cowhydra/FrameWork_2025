using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard<BlackboardKeyType>
{
    private Dictionary<BlackboardKeyType, object> _genericValues = new Dictionary<BlackboardKeyType, object>();


    public void SetGeneric<T>(BlackboardKeyType key, T value)
    {
        _genericValues[key] = value;
    }


    public T GetGeneric<T>(BlackboardKeyType key)
    {
        object value;
        if (_genericValues.TryGetValue(key, out value))
        {
            return (T)value;
        }

        Debug.Log("해당 키에 대한 값이 존재하지 않습니다.");
        return default(T);
    }

}

public abstract class BlackboardKeyBase
{

}


//같은 애들 속성끼리 정보를 공유할때 사용하자.
// 다른 속성도 같이 정보를 공유할 필요가 있으면 Shared를 사용하자.
public class BlackboardManager : SingletonObj<BlackboardManager>
{

    // 각 MonoBehaviour는 고유한 블랙보드를 가질 수 있습니다.
    private Dictionary<MonoBehaviour, object> _individualBlackboards = new Dictionary<MonoBehaviour, object>();

    // 여러 개체들이 동일한 블랙보드를 공유할 때 사용됩니다.
    private Dictionary<int, object> _sharedBlackboards = new Dictionary<int, object>();


    public Blackboard<T> GetIndividualBlackboard<T>(MonoBehaviour requestor) where T : BlackboardKeyBase, new()
    {
        if (!_individualBlackboards.ContainsKey(requestor))
        {
            _individualBlackboards[requestor] = new Blackboard<T>();
        }

        return _individualBlackboards[requestor] as Blackboard<T>;
    }

    public Blackboard<T> GetSharedBlackboard<T>(int uniqueID) where T : BlackboardKeyBase, new()
    {
        if (!_sharedBlackboards.ContainsKey(uniqueID))
        {
            _sharedBlackboards[uniqueID] = new Blackboard<T>();
        }

        return _sharedBlackboards[uniqueID] as Blackboard<T>;
    }

}