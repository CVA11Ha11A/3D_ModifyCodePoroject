using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    // 주어진 부모 노드 아래에서 가장 하위 리프 노드를 추출하는 메서드
    public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();

        // 부모 노드가 리프 노드인 경우, 해당 노드를 반환합니다.
        if (parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>() { parentNode };
        }

        // 모든 자식 노드를 큐에 추가합니다.
        foreach (var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }

        // 큐를 통해 모든 리프 노드를 추출합니다.
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();

            if (currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                // 현재 노드의 자식 노드를 큐에 추가합니다.
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }       // TraverseGraphToExtractLowestLeafes()

    // 주어진 경계 포인트 범위 내에서 하단 왼쪽 모퉁이를 생성하는 메서드
    public static Vector2Int GenerateBottomLeftCornerBetWeen(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        // 랜덤한 좌표를 생성하여 반환합니다.
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (minY - minY) * pointModifier)));
    }       // GenerateBottomLeftCornerBetWeen()

    // 주어진 경계 포인트 범위 내에서 상단 오른쪽 모퉁이를 생성하는 메서드
    public static Vector2Int GenerateTopRightCornerBetWeen(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        // 랜덤한 좌표를 생성하여 반환합니다.
        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY));
    }       // GenerateTopRightCornerBetWeen()

    // 두 벡터의 중간 지점을 계산하는 메서드
    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;

        // 중간 지점의 정수 좌표를 반환합니다.
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }

    // 상대적 위치를 정의하는 열거형
    public enum RelativePosition
    {
        Up,
        Down,
        Left,
        Right
    }
}       // ClassEnd


#region LEGACY
/*using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeafes(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        if(parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach(var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
        while(nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if(currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }       // TraverseGraphToExtractLowestLeafes()

    public static Vector2Int GenerateBottomLeftCornerBetWeen(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (minY - minY) * pointModifier)));
    }       // GenerateBottomLeftCOrnerBetWeen()

    public static Vector2Int GenerateTopRightCornerBetWeen(
    Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        return new Vector2Int(
            Random.Range((int)(minX + (maxX-minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY-minY) * pointModifier), maxY)
            );
    }       // GenerateBottomLeftCOrnerBetWeen()

    public static Vector2Int CalculatemiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVEctor = sum / 2;
        return new Vector2Int((int)tempVEctor.x,(int)tempVEctor.y);
    }

    public enum RelativePosition
    {
        Up,
        Down,
        Left,
        Right
    }

}       // ClassEnd*/
#endregion LEGACY