using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMeshPos : MonoBehaviour
{
    public Vector3 bottomLeftCorner;
    public Vector3 bottomRightCorner;
    public Vector3 topLeftCorner;
    public Vector3 topRightCorner;
    public Vector3 center;

    public void InItPos(Vector3 _bottomLeftCorner,Vector3 _bottomRightCorner,
        Vector3 _topLeftCorner,Vector3 _topRightCorner)
    {
        this.bottomLeftCorner = _bottomLeftCorner;
        this.bottomRightCorner = _bottomRightCorner;
        this.topLeftCorner = _topLeftCorner;
        this.topRightCorner = _topRightCorner;
        this.center = new Vector3((_bottomLeftCorner.x + _bottomRightCorner.x) * CalculationValue.ZEROPOINTFIVE,
            _bottomLeftCorner.y, (_bottomLeftCorner.z + _topLeftCorner.z) * CalculationValue.ZEROPOINTFIVE);
    }       // InItPos()

}       // ClassEnd
