using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTATTRIBUTEGENERIC<T>
{
    public T Instance = default;

    public TESTATTRIBUTEGENERIC(T _Instance)
    {
        this.Instance = _Instance;
    }
}
