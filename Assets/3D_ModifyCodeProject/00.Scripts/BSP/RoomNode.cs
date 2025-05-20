using UnityEngine;

public class RoomNode : Node
{
    // RoomNode 클래스의 생성자
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, Node parentNode, int index) : base(parentNode)
    {
        // 바닥 왼쪽 모서리와 바닥 오른쪽 모서리, 부모 노드 및 트리 레이어 인덱스를 초기화합니다.
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, TopRightAreaCorner.y);
        this.TreeLayerIndex = index;
    }

    // 방의 너비를 계산하는 속성
    public int Width { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }

    // 방의 길이를 계산하는 속성
    public int Length { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }
}       // ClassEnd


#region LEGACY
/*using UnityEngine;

public class RoomNode : Node
{
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, Node parentNode, int index) : base(parentNode)
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, TopRightAreaCorner.y);
        this.TreeLayerIndex = index;
    }       // ctor()

    public int Width { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }

    public int Length { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }

}       // ClassEnd*/
#endregion LEGACY
