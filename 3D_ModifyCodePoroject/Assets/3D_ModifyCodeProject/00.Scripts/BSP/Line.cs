using UnityEngine;

public class Line
{
    Orientation orientation; // 직선의 방향 (가로 또는 세로)
    Vector2Int coordinates;  // 직선의 좌표

    // Line 클래스의 생성자
    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    // 직선의 방향을 가져오거나 설정합니다.
    public Orientation Orientation { get => orientation; set => orientation = value; }

    // 직선의 좌표를 가져오거나 설정합니다.     == X pos
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

// 직선의 방향을 정의하는 열거형
public enum Orientation
{
    Horizontal = 0, // 가로 방향
    Vertical = 1   // 세로 방향
}

#region LEGACY
/*
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientation,Vector2Int coordinates)    
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

public enum orienTation
{
    Horiszontal = 0,
    Vertical = 1
}
 */
#endregion LEGACY