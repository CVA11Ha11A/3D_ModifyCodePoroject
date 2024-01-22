using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class CorridorsGenerator
{
    public List<Node> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
    {
        // 복도를 생성하는 함수입니다.
        List<Node> corridorList = new List<Node>(); // 코릿도 노드를 담을 리스트를 생성합니다.
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(Node => Node.TreeLayerIndex).ToList());
        // 모든 방 노드를 TreeLayerIndex에 따라 큐에 넣습니다. 큐는 우선 순위 큐로 활용됩니다.

        while (structuresToCheck.Count > 0)
        {
            // 큐가 빌 때까지 반복합니다.
            var node = structuresToCheck.Dequeue(); // 큐에서 노드를 하나 꺼냅니다.            
            if (node.ChildrenNodeList.Count == 0)
            {
                // 현재 노드에 자식 노드가 없다면, 다음 노드를 처리합니다.
                continue;
            }

            // 현재 노드의 자식 노드를 이용하여 코릿도 노드를 생성합니다.
            CorridorNode corridor = new CorridorNode(
                node.ChildrenNodeList[0], node.ChildrenNodeList[1], corridorWidth);

            // 생성된 코릿도 노드를 코릿도 리스트에 추가합니다.
            corridorList.Add(corridor);
        }
        foreach(Node tempcorridorNode in corridorList)
        {
            tempcorridorNode.isCorridor = true;
        }

        return corridorList; // 코릿도 노드로 이루어진 리스트를 반환합니다.

    } // CreateCorridor()
}       // ClassEnd

#region LEGACY
/*
 using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorsGenerator
{
    public List<Node> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(Node => Node.TreeLayerIndex).ToList());
        while(structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            if(node.ChildrenNodeList.Count == 0)
            {
                continue;
            }
            CorridorNode corridor = new CorridorNode(
                node.ChildrenNodeList[0], node.ChildrenNodeList[1], corridorWidth);

            corridorList.Add(corridor);
        }
        return corridorList;
    }       // CreateCorridor()
}       // ClassEnd*/
#endregion LEGACY