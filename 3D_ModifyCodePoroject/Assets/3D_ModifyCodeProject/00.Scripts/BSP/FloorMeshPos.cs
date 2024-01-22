using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMeshPos : MonoBehaviour
{
    public Vector3 bottomLeftCorner;
    public Vector3 bottomRightCorner;
    public Vector3 topLeftCorner;
    public Vector3 topRightCorner;

    public void InItPos(Vector3 _bottomLeftCorner,Vector3 _bottomRightCorner,
        Vector3 _topLeftCorner,Vector3 _topRightCorner)
    {
        this.bottomLeftCorner = _bottomLeftCorner;
        this.bottomRightCorner = _bottomRightCorner;
        this.topLeftCorner = _topLeftCorner;
        this.topRightCorner = _topRightCorner;
    }       // InItPos()

}       // ClassEnd
