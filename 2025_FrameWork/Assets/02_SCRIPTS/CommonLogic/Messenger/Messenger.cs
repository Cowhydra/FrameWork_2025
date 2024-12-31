using System;
using System.Collections.Generic;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
public delegate void Callback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);
public delegate void Callback<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, X arg5);

public enum MsnMode : byte
{
    DONT_REQUIRE_LISTENER,
    REQUIRE_LISTENER,
}

public enum MsnLRT : byte
{
    ADD_LISTENER,
    REMOVE_LISTENER
}

internal static class MessengerInternal
{
    public static Dictionary<MsgID, List<Delegate>> EventDic { get; private set; } = new Dictionary<MsgID, List<Delegate>>();
    public static readonly MsnMode DEFAULT_MODE = MsnMode.DONT_REQUIRE_LISTENER;

    public static void AddListener(MsgID msgID, Delegate listener)
    {
        if (listener == null)
        { 
            return;
        }

        if (EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            if (delegateList.Contains(listener))
            { 
                return;
            }

            if (delegateList.Count > 0)
            {
                Delegate d = delegateList[0];

                if (d != null && d.GetType() != listener.GetType())
                {
                    throw new Exception($"Attempting to add listener with inconsistent signature for event type {msgID}." +
                        $"Current listeners have type {d.GetType().Name} and listener being added has type {listener.GetType().Name}");
                }
            }

            delegateList.Add(listener);
        }
        else
        {
            delegateList = new List<Delegate>();
            delegateList.Add(listener);

            EventDic.Add(msgID, delegateList);
        }
    }

    public static void RemoveListener(MsgID msgID, Delegate listener)
    {
        if (listener == null)
        { 
            return;
        }

        if (EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            delegateList.Remove(listener);

            if (delegateList.Count <= 0)
            { 
                EventDic.Remove(msgID);
            }
        }
    }

    public static void RemoveAllListener(MsgID msgID)
    {
        EventDic.Remove(msgID);
    }
}

//
// No parameters
//
public static class Messenger
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback handler)
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

    public static void Broadcast(MsgID msgID)
    {
        Broadcast(msgID, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback callback = delegateList[i] as Callback;
                if (callback != null)
                { 
                    callback();
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
// One parameters
//
public static class Messenger<T>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback<T> handler)
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

    public static void Broadcast(MsgID msgID, T arg1)
    {
        Broadcast(msgID, arg1, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback<T> callback = delegateList[i] as Callback<T>;
                if (callback != null)
                { 
                    callback(arg1);
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
public static class Messenger<T, U>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback<T, U> handler)
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

    public static void Broadcast(MsgID msgID, T arg1, U arg2)
    {
        Broadcast(msgID, arg1, arg2, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback<T, U> callback = delegateList[i] as Callback<T, U>;
                if (callback != null)
                {
                    callback(arg1, arg2);
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
public static class Messenger<T, U, V>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback<T, U, V> handler)
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

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3)
    {
        Broadcast(msgID, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback<T, U, V> callback = delegateList[i] as Callback<T, U, V>;
                if (callback != null)
                { 
                    callback(arg1, arg2, arg3);
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
public static class Messenger<T, U, V, W>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback<T, U, V, W> handler)
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

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4)
    {
        Broadcast(msgID, arg1, arg2, arg3, arg4, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback<T, U, V, W> callback = delegateList[i] as Callback<T, U, V, W>;
                if (callback != null)
                { 
                    callback(arg1, arg2, arg3, arg4);
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
public static class Messenger<T, U, V, W, X>
{
    public static void RegisterListener(MsnLRT val, MsgID msgID, Callback<T, U, V, W, X> handler)
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

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Broadcast(msgID, arg1, arg2, arg3, arg4, arg5, MessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, X arg5, MsnMode mode)
    {
        if (MessengerInternal.EventDic.TryGetValue(msgID, out List<Delegate> delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                Callback<T, U, V, W, X> callback = delegateList[i] as Callback<T, U, V, W, X>;
                if (callback != null)
                { 
                    callback(arg1, arg2, arg3, arg4, arg5);
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
