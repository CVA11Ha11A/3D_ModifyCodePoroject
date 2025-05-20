#region LEGACY
/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> childrenNodeList; // 자식 노드 목록을 저장할 리스트

    public List<Node> ChildrenNodeList { get => childrenNodeList; } // 자식 노드 리스트를 외부에서 읽을 수 있도록 속성 제공

    public bool Visted { get; set; } // 방문 여부를 나타내는 부울 값

    public Vector2Int BottomLeftAreaCorner { get; set; } // 왼쪽 아래 영역 모퉁이 좌표
    public Vector2Int BottomRightAreaCorner { get; set; } // 오른쪽 아래 영역 모퉁이 좌표
    public Vector2Int TopRightAreaCorner { get; set; } // 오른쪽 위 영역 모퉁이 좌표
    public Vector2Int TopLeftAreaCorner { get; set; } // 왼쪽 위 영역 모퉁이 좌표

    public Node Parent { get; set; } // 부모 노드
    public int TreeLayerIndex { get; set; } // 노드가 트리 내에서 몇 번째 레이어에 속하는지 나타내는 값

    public Node(Node parentNode)
    {
        childrenNodeList = new List<Node>(); // 자식 노드 리스트 초기화
        this.Parent = parentNode; // 부모 노드 설정
        if (parentNode != null)
        {
            parentNode.AddChild(this); // 부모 노드에 현재 노드를 자식으로 추가
        }
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node); // 자식 노드를 리스트에 추가
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node); // 자식 노드를 리스트에서 제거
    }
}
 */
#endregion LEGACY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{

    private List<Node> childrenNodeList; // 자식 노드 목록을 저장할 리스트

    public List<Node> ChildrenNodeList { get => childrenNodeList; } // 자식 노드 리스트를 외부에서 읽을 수 있도록 속성 제공

    public bool Visted { get; set; } // 방문 여부를 나타내는 부울 값

    public bool isFloor { get; set; } // 해당 노드가 바닥으로 사용될지 확인할 bool값
    public bool isCorridor { get; set; }    // 해당 노드가 복도로 사용될지 확인할 bool값

    public Vector2Int BottomLeftAreaCorner { get; set; } // 왼쪽 아래 영역 모퉁이 좌표
    public Vector2Int BottomRightAreaCorner { get; set; } // 오른쪽 아래 영역 모퉁이 좌표
    public Vector2Int TopRightAreaCorner { get; set; } // 오른쪽 위 영역 모퉁이 좌표
    public Vector2Int TopLeftAreaCorner { get; set; } // 왼쪽 위 영역 모퉁이 좌표

    public Node Parent { get; set; } // 부모 노드
    public int TreeLayerIndex { get; set; } // 노드가 트리 내에서 몇 번째 레이어에 속하는지 나타내는 값

     public Node(Node parentNode)
        {
        childrenNodeList = new List<Node>(); // 자식 노드 리스트 초기화
        this.Parent = parentNode; // 부모 노드 설정
        if (parentNode != null)
         {
             parentNode.AddChild(this); // 부모 노드에 현재 노드를 자식으로 추가
         }
      }

    public void AddChild(Node node)
     {
        childrenNodeList.Add(node); // 자식 노드를 리스트에 추가
     }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node); // 자식 노드를 리스트에서 제거
    }
}
