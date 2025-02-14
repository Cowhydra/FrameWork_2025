using System;
using System.Collections.Generic;

public delegate void RefCallback<T>(in T arg1);
public delegate void RefCallback<T, U>(T arg1, in U arg2);
public delegate void RefCallback<T, U, V>(T arg1, U arg2, in V arg3);
public delegate void RefCallback<T, U, V, W>(T arg1, U arg2, V arg3, in W arg4);
public delegate void RefCallback<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, in X arg5);

//
// One parameters
//
public static class RefMessenger<T>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, RefCallback<T> handler)
    {
        if (val == MsnLRT.ADD_LISTENER)
        {
            MessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MsnLRT.REMOVE_LISTENER)
        {
            MessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void RemoveAllListener(MsgID msgID)
    {
        MessengerInternal.RemoveAllListener(msgID);
    }

    public static void Broadcast(MsgID msgID, in T arg1)
    {
        Broadcast(msgID, in arg1, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, in T arg1, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                RefCallback<T> callback = delegateList[i] as RefCallback<T>;
                if (callback != null)
                {
                    callback(in arg1);
                }
                else
                {
                    throw new Exception($"Broadcasting message {msgID} but listeners have a different signature than the broadcaster.");
                }
            }
        }
        else
        {
            if (mode == MsnMode.REQUIRE_LISTENER)
            {
                throw new Exception($"Broadcasting message {msgID} but no listener found.");
            }
        }
    }
}


//
// Two parameters
//
public static class RefMessenger<T, U>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, RefCallback<T, U> handler)
    {
        if (val == MsnLRT.ADD_LISTENER)
        {
            MessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MsnLRT.REMOVE_LISTENER)
        {
            MessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, in U arg2)
    {
        Broadcast(msgID, arg1, in arg2, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, in U arg2, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                RefCallback<T, U> callback = delegateList[i] as RefCallback<T, U>;
                if (callback != null)
                {
                    callback(arg1, in arg2);
                }
                else
                {
                    throw new Exception($"Broadcasting message {msgID} but listeners have a different signature than the broadcaster.");
                }
            }
        }
        else
        {
            if (mode == MsnMode.REQUIRE_LISTENER)
            {
                throw new Exception($"Broadcasting message {msgID} but no listener found.");
            }
        }
    }
}


//
// Three parameters
//
public static class RefMessenger<T, U, V>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, RefCallback<T, U, V> handler)
    {
        if (val == MsnLRT.ADD_LISTENER)
        {
            MessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MsnLRT.REMOVE_LISTENER)
        {
            MessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, in V arg3)
    {
        Broadcast(msgID, arg1, arg2, in arg3, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, in V arg3, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                RefCallback<T, U, V> callback = delegateList[i] as RefCallback<T, U, V>;
                if (callback != null)
                {
                    callback(arg1, arg2, in arg3);
                }
                else
                {
                    throw new Exception($"Broadcasting message {msgID} but listeners have a different signature than the broadcaster.");
                }
            }
        }
        else
        {
            if (mode == MsnMode.REQUIRE_LISTENER)
            {
                throw new Exception($"Broadcasting message {msgID} but no listener found.");
            }
        }
    }
}


//
// Four parameters
//
public static class RefMessenger<T, U, V, W>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, RefCallback<T, U, V, W> handler)
    {
        if (val == MsnLRT.ADD_LISTENER)
        {
            MessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MsnLRT.REMOVE_LISTENER)
        {
            MessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, in W arg4)
    {
        Broadcast(msgID, arg1, arg2, arg3, in arg4, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, in W arg4, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                RefCallback<T, U, V, W> callback = delegateList[i] as RefCallback<T, U, V, W>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, in arg4);
                }
                else
                {
                    throw new Exception($"Broadcasting message {msgID} but listeners have a different signature than the broadcaster.");
                }
            }
        }
        else
        {
            if (mode == MsnMode.REQUIRE_LISTENER)
            {
                throw new Exception($"Broadcasting message {msgID} but no listener found.");
            }
        }
    }
}


//
// Five parameters
//
public static class RefMessenger<T, U, V, W, X>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, RefCallback<T, U, V, W, X> handler)
    {
        if (val == MsnLRT.ADD_LISTENER)
        {
            MessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MsnLRT.REMOVE_LISTENER)
        {
            MessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, in X arg5)
    {
        Broadcast(msgID, arg1, arg2, arg3, arg4, in arg5, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, in X arg5, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                RefCallback<T, U, V, W, X> callback = delegateList[i] as RefCallback<T, U, V, W, X>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4, in arg5);
                }
                else
                {
                    throw new Exception($"Broadcasting message {msgID} but listeners have a different signature than the broadcaster.");
                }
            }
        }
        else
        {
            if (mode == MsnMode.REQUIRE_LISTENER)
            {
                throw new Exception($"Broadcasting message {msgID} but no listener found.");
            }
        }
    }
}
