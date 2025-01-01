
public class BitFlags
{
    private uint _Flags = 0;

    public bool IsClear => (_Flags == 0);


    public void Clear()
    {
        _Flags = 0;
    }

    public void Set(uint flag, bool val)
    {
        if (flag <= 0)
        { 
            return;
        }

        if (val)
        { 
            _Flags |= flag;
        }
        else
        { 
            _Flags &= ~flag;
        }
    }

    // flag°¡ ¼³Á¤µÆ³ª?
    public bool IsSet(uint flag)
    {
        if (flag <= 0)
        { 
            return false;
        }

        return ((_Flags & flag) == flag);
    }
}

