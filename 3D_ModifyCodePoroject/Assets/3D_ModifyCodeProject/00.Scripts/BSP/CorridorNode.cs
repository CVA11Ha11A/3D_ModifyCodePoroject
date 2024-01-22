using UnityEngine;
using System;
using static StructureHelper;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class CorridorNode : Node
{
    private Node structure1; // 복도 노드의 첫 번째 연결 구조
    private Node structure2; // 복도 노드의 두 번째 연결 구조
    private int CorridorWidth; // 복도의 너비
    private int modifierDistanceFromWall = 1; // 벽으로부터의 거리 수정자

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        structure1 = node1;
        structure2 = node2;
        CorridorWidth = corridorWidth;
        GenerateCorridor(); // 복도 생성 함수 호출
    }

    private void GenerateCorridor()
    {
        // structure1과 structure2의 상대적 위치를 확인합니다.
        var relaticePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();

        switch (relaticePositionOfStructure2)
        {
            case RelativePosition.Up:
                // structure1이 위쪽에 위치할 때 복도를 생성합니다.
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                // structure1이 아래쪽에 위치할 때 복도를 생성합니다.
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                // structure1이 오른쪽에 위치할 때 복도를 생성합니다.
                ProcessRoomInRelationRightOrLeft(this.structure1, this.structure2);
                break;
            case RelativePosition.Left:
                // structure1이 왼쪽에 위치할 때 복도를 생성합니다.
                ProcessRoomInRelationRightOrLeft(this.structure2, this.structure1);
                break;
            default:
                break;
        }
    }

    private void ProcessRoomInRelationRightOrLeft(Node structure1, Node structure2)
    {
        // 오른쪽 또는 왼쪽에 있는 방과 관련된 처리 함수입니다.
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        // 왼쪽 방 중 최하위 리프 노드를 찾습니다.
        var sortedLeftStructure = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if (sortedLeftStructure.Count == 1)
        {
            leftStructure = sortedLeftStructure[0];
        }
        else
        {
            // 최하위 리프 노드들 중 X 좌표가 비슷한 노드를 선택합니다.
            int maxX = sortedLeftStructure[0].TopRightAreaCorner.x;
            sortedLeftStructure = sortedLeftStructure.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructure.Count);
            leftStructure = sortedLeftStructure[index];
        }

        // 오른쪽 방 중 유효한 이웃 후보들을 선택합니다.
        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child => GetValidYForNeighourLeftRight(
                leftStructure.TopRightAreaCorner,
                leftStructure.BottomLeftAreaCorner,
                child.TopLeftAreaCorner,
                child.BottomLeftAreaCorner
            ) != -1
        ).OrderBy(child => child.BottomRightAreaCorner.x).ToList();

        if (possibleNeighboursInRightStructureList.Count <= 0)
        {
            // 유효한 오른쪽 방 후보가 없으면 structure2를 사용합니다.
            rightStructure = structure2;
        }
        else
        {
            // 유효한 오른쪽 방 후보 중 첫 번째를 선택합니다.
            rightStructure = possibleNeighboursInRightStructureList[0];
        }

        // 유효한 Y 좌표를 계산합니다.
        int y = GetValidYForNeighourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner,
            rightStructure.BottomLeftAreaCorner);

        while (y == -1 && sortedLeftStructure.Count > 0)
        {
            // 유효한 Y 좌표가 없다면 왼쪽 방 중 다른 후보를 선택하여 재시도합니다.
            sortedLeftStructure = sortedLeftStructure.Where(
                child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortedLeftStructure[0];

            y = GetValidYForNeighourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner,
            rightStructure.BottomLeftAreaCorner);
        }

        // 복도의 왼쪽 아래 모서리와 오른쪽 위 모서리 좌표를 설정합니다.
        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + this.CorridorWidth);
    }

    private int GetValidYForNeighourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        // 왼쪽 또는 오른쪽 이웃의 유효한 Y 좌표를 계산하는 함수입니다.
        // 왼쪽 방 위쪽 모서리와 아래쪽 모서리, 오른쪽 방 위쪽 모서리, 아래쪽 모서리를 비교하여 유효한 Y 좌표를 계산합니다.
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
            ).y;
        }
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
            ).y;
        }
        if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)
            ).y;
        }
        if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
            ).y;
        }
        return -1;
    }

    private void ProcessRoomInRelationUpOrDown(Node structure1, Node structure2)
    {
        // 위쪽 또는 아래쪽 관계에 있는 방과 관련된 처리 함수입니다.
        Node bottomStructure = null;
        List<Node> structureBottomChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node topStructure = null;
        List<Node> structureAboveChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        // 아래 방 중 최하위 리프 노드를 찾습니다.
        var sortedBottomStructure = structureBottomChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

        if (sortedBottomStructure.Count == 1)
        {
            bottomStructure = structureBottomChildren[0];
        }
        else
        {
            // 최하위 리프 노드들 중 Y 좌표가 비슷한 노드를 선택합니다.
            int maxY = sortedBottomStructure[0].TopLeftAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Mathf.Abs(maxY - child.TopLeftAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        // 윗 방에서 유효한 X 좌표를 계산하는 후보들을 선택합니다.
        var possibleNeighboursInTopStructure = structureAboveChildren.Where(
            child => GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                child.BottomLeftAreaCorner,
                child.BottomRightAreaCorner)
            != -1).OrderBy(ChildrenNodeList => ChildrenNodeList.BottomRightAreaCorner.y).ToList();

        if (possibleNeighboursInTopStructure.Count == 0)
        {
            topStructure = structure2;
        }
        else
        {
            topStructure = possibleNeighboursInTopStructure[0];
        }

        // 유효한 X 좌표를 계산합니다.
        int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);

        while (x == -1 && sortedBottomStructure.Count > 1)
        {
            // 유효한 X 좌표가 없다면 다른 아래 방 후보를 선택하여 재시도합니다.
            sortedBottomStructure = sortedBottomStructure.Where(child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];

            x = GetValidXForNeighbourUpDown(
            bottomStructure.TopLeftAreaCorner,
            bottomStructure.TopRightAreaCorner,
            topStructure.BottomLeftAreaCorner,
            topStructure.BottomRightAreaCorner);
        }

        // 복도의 아래쪽 왼쪽 모서리와 위쪽 오른쪽 모서리 좌표를 설정합니다.
        BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x + this.CorridorWidth, topStructure.BottomLeftAreaCorner.y);
    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft, Vector2Int bottomNodeRight, Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        // 위쪽 또는 아래쪽 이웃의 유효한 X 좌표를 계산하는 함수입니다.
        // 아래 방의 왼쪽, 오른쪽 모서리와 윗 방의 왼쪽, 오른쪽 모서리를 비교하여 유효한 X 좌표를 계산합니다.
        if (topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall, 0)
            ).x;
        }
        if (topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall, 0)
            ).x;
        }
        if (bottomNodeLeft.x >= topNodeLeft.x && bottomNodeLeft.x <= topNodeRight.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall, 0)
            ).x;
        }
        if (bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureHelper.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall, 0)
            ).x;
        }
        return -1;
    }       // GetValidXForNeighbourUpDown()

    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        // structure2가 structure1에 대한 상대적인 위치를 판단하는 함수입니다.
        Vector2 middlePointStructure1Temp = (
            (Vector2)structure1.TopRightAreaCorner + structure1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStructure2Temp = (
            (Vector2)structure2.TopRightAreaCorner + structure2.BottomLeftAreaCorner) / 2;

        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return RelativePosition.Right;
        }
        else if (angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }       // CheckPositionStructure2AgainstStructure1()

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
    {
        // structure1과 structure2의 중간 지점을 이용하여 각도를 계산하는 함수입니다.
        return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y,
            middlePointStructure2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }       // CalculateAngle()


}       // ClassEnd

#region LEGACY
/*
 using UnityEngine;
using System;
using UnityEditor.iOS;
using static StructureHelper;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class CorridorNode : Node
{

    private Node structure1;
    private Node structure2;
    private int CorridorWidth;
    private int modifierDistanceFromWall = 1;


    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        structure1 = node1;
        structure2 = node2;
        CorridorWidth = corridorWidth;
        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        var relaticePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        switch (relaticePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.structure1, this.structure2);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.structure2, this.structure1);
                break;
            default:
                break;
        }
    }       // GenerateCorridor()



    private void ProcessRoomInRelationRightOrLeft(Node structure1, Node structure2)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        var sortedLeftStructure = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if (sortedLeftStructure.Count == 1)
        {
            leftStructure = sortedLeftStructure[0];
        }
        else
        {
            int maxX = sortedLeftStructure[0].TopRightAreaCorner.x;
            sortedLeftStructure = sortedLeftStructure.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructure.Count);
            leftStructure = sortedLeftStructure[index];
        }

        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child => GetValidYForNeighourLeftRight(
                leftStructure.TopRightAreaCorner,
                leftStructure.BottomLeftAreaCorner,
                child.TopLeftAreaCorner,
                child.BottomLeftAreaCorner
                ) != -1
            ).OrderBy(child => child.BottomRightAreaCorner.x).ToList();
        if (possibleNeighboursInRightStructureList.Count <= 0)
        {
            rightStructure = structure2;
        }
        else
        {
            rightStructure = possibleNeighboursInRightStructureList[0];
        }
        int y = GetValidYForNeighourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner,
            rightStructure.BottomLeftAreaCorner);
        while (y == -1 && sortedLeftStructure.Count > 0)
        {
            sortedLeftStructure = sortedLeftStructure.Where(
                child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortedLeftStructure[0];

            y = GetValidYForNeighourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner,
            rightStructure.BottomLeftAreaCorner);
        }
        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + this.CorridorWidth);

    }

    private int GetValidYForNeighourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
            return StructureHelper.CalculatemiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
                ).y;
        }
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {
            return StructureHelper.CalculatemiddlePoint(
              rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
              rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
              ).y;
        }
        if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculatemiddlePoint(

           rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
           leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)
           ).y;
        }
        if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureHelper.CalculatemiddlePoint(
           leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
           rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.CorridorWidth)
           ).y;
        }
        return -1;


    }       // GetValidYForNeighourLeftRight()

    private void ProcessRoomInRelationUpOrDown(Node structure1, Node structure2)
    {
        Node bottomStructure = null;
        List<Node> structureBottomChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        Node topStructure = null;
        List<Node> structureAboveChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        var sortedBottomStructure = structureBottomChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

        if (sortedBottomStructure.Count == 1)
        {
            bottomStructure = structureBottomChildren[0];
        }
        else
        {
            int maxY = sortedBottomStructure[0].TopLeftAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Mathf.Abs(maxY - child.TopLeftAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        var possibleNeighboursInTopStruture = structureAboveChildren.Where(
            child => GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                child.BottomLeftAreaCorner,
                child.BottomRightAreaCorner)
            != -1).OrderBy(ChildrenNodeList => ChildrenNodeList.BottomRightAreaCorner.y).ToList();
        if (possibleNeighboursInTopStruture.Count == 0)
        {
            topStructure = structure2;
        }
        else
        {
            topStructure = possibleNeighboursInTopStruture[0];
        }
        int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        while (x == -1 && sortedBottomStructure.Count > 1)
        {
            sortedBottomStructure = sortedBottomStructure.Where(child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];

            x = GetValidXForNeighbourUpDown(
            bottomStructure.TopLeftAreaCorner,
            bottomStructure.TopRightAreaCorner,
            topStructure.BottomLeftAreaCorner,
            topStructure.BottomRightAreaCorner);

        }
        BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x + this.CorridorWidth, topStructure.BottomLeftAreaCorner.y);
    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft, 
        Vector2Int bottomNodeRight, Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        if(topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if(topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall,0),
                topNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall,0)
                ).x;
        }
        if(bottomNodeLeft.x >= (topNodeLeft.x) && bottomNodeLeft.x <= topNodeRight.x)
        {
            return StructureHelper.CalculatemiddlePoint(
               bottomNodeLeft + new Vector2Int(modifierDistanceFromWall,0),
               topNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall,0)
               ).x;
        }
        if(bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureHelper.CalculatemiddlePoint(
               topNodeLeft + new Vector2Int(modifierDistanceFromWall,0),
               bottomNodeRight - new Vector2Int(this.CorridorWidth + modifierDistanceFromWall,0)
               ).x;
        }
        return -1;


    }       // GetValidXForNeighbourUpDown()

    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        Vector2 middlePointStructure1Temp = (
            (Vector2)structure1.TopRightAreaCorner + structure1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStructure2Temp = (
            (Vector2)structure2.TopRightAreaCorner + structure2.BottomLeftAreaCorner) / 2;

        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return RelativePosition.Right;
        }
        else if (angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if (angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
    {
        return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y,
            middlePointStructure2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }
}       // ClassEnd
 */
#endregion LEGACY