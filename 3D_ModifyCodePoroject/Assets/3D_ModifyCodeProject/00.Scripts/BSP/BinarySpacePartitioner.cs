using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinarySpacePartitioner
{
    private RoomNode rootNode; // 이진 공간 분할을 시작할 루트 노드
    private int dungeonWidth; // 던전의 너비
    private int dungeonLength; // 던전의 길이
    public RoomNode RootNode { get => rootNode; } // 루트 노드에 대한 읽기 전용 속성

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
        // 이진 공간 분할을 초기화하고 루트 노드를 생성
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLengthMin)
    {
        // 이진 공간 분할을 수행하고 노드 컬렉션을 준비하는 함수
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxIterations && graph.Count > 0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= roomWidthMin * 2 || currentNode.Length >= roomLengthMin * 2)
            {
                SplitTheSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph);
                // 현재 노드를 분할하고 자식 노드를 생성하여 큐에 추가
            }
        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        // 공간을 분할하는 함수
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomWidthMin,
            roomLengthMin);
        RoomNode node1, node2;
        if (line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else // (line.Orientation == Orientation.Vertical)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
           new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y),
           currentNode,
           currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
        // 공간을 나누고 생성된 자식 노드를 추가
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        // 새로운 노드를 결과 리스트와 큐에 추가하는 함수
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        // 공간을 나누는 선을 결정하는 함수
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLengthMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;

        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if (widthStatus)
        {
            orientation = Orientation.Vertical; // 0
        }
        else
        {
            orientation = Orientation.Horizontal;  // 1
        }
        return new Line(orientation, GetCoordinatesFororientation
            (orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthMin,
            roomLengthMin));
        // 공간을 나눌 방향과 좌표를 결정하여 반환
    }

    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        // 분할된 방향에 따라 새로운 좌표를 생성하는 함수
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, Random.Range(
                (bottomLeftAreaCorner.y + roomLengthMin),
                (topRightAreaCorner.y - roomLengthMin)));
        }
        else
        {
            coordinates = new Vector2Int(
                Random.Range(
                (bottomLeftAreaCorner.x + roomWidthMin),
                (topRightAreaCorner.x - roomWidthMin))
                , 0);
        }

        return coordinates;
    }
}       // ClassEnd

#region LEGACY
/*
 using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinarySpacePartitioner
{

    private RoomNode rootNode;
    private int dungeonWidth;
    private int dungeonLength;
    public RoomNode RootNode { get => rootNode; }

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxIterations,int roomWidthMin,int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while(iterations < maxIterations && graph.Count > 0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if(currentNode.Width>=roomWidthMin * 2 || currentNode.Length >= roomLengthMin * 2)
            {
                SplitTheSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph);
            }
        }
        return listToReturn;
    }       // PrepareNodesCollection()

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomWidthMin,
            roomLengthMin);
        RoomNode node1, node2;
        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
           new Vector2Int(line.Coordinates.x,currentNode.TopRightAreaCorner.y),
           currentNode,
           currentNode.TreeLayerIndex + 1);

            node2 = new RoomNode(new Vector2Int(line.Coordinates.x,currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);

    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLengthMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;

        if (lengthStatus && widthStatus)        
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if(widthStatus)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }
        return new Line(orientation, GetCoordinatesFororientation
            (orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthMin,
            roomLengthMin));

    }

    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, Random.Range(
                (bottomLeftAreaCorner.y + roomLengthMin),
                (topRightAreaCorner.y - roomLengthMin)));
        }
        else
        {
            coordinates = new Vector2Int(
                Random.Range(
                (bottomLeftAreaCorner.x + roomWidthMin),
                (topRightAreaCorner.x - roomWidthMin))
                ,0);
        }

        return coordinates;
    }
}       // ClassEnd
 */
#endregion LEGACY
