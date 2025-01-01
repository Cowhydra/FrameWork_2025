
public class Flags
{
    private uint _Flags = 0;

    public bool IsClear => (_Flags == 0);


    public void Clear()
    {
        _Flags = 0;
    }

    public void Set(int index, bool val)
    {
        if (index < 0 || index >= 32)
        { 
            return;
        }

        if (val)
        { 
            _Flags |= (uint)(1 << index);
        }
        else
        { 
            _Flags &= ~(uint)(1 << index);
        }
    }

    public bool IsSet(int index)
    {
        if (index < 0 || index >= 32)
        { 
            return false;
        }

        return ((_Flags & (1 << index)) != 0);
    }
}
