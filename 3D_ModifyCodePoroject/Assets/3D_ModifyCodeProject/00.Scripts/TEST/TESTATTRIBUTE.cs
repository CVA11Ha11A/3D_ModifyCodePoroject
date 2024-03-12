using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TESTATTRIBUTE : Attribute
{
    public TESTATTRIBUTE(Type _TargetClass)
    {
        //Debug.Log("뭘 받아왔지?", _TargetClass.Name);
    }

}       // ClassEnd
