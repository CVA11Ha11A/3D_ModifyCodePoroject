using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DungeonGenerator
{
    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;

    // 생성자: 던전의 넓이와 길이를 설정
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }   // 생성자

    // 방 및 복도 생성 및 던전 구조 계산
    public List<Node> CalculateRooms(int maxIterations, int roomWidthMin, int roomLengthMin,
        float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        // 이진 공간 분할을 위한 클래스 초기화
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);

        // 모든 공간 노드를 미리 준비
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);

        // 그래프를 탐색하여 가장 하위 단말 노드를 추출하여 방 공간 목록 생성
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        // 방 생성을 위한 클래스 초기화
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);

        // 주어진 공간에 방 생성
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(
            roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        // 복도 생성을 위한 클래스 초기화
        CorridorsGenerator corridorGenerator = new CorridorsGenerator();

        // 복도 생성
        var corridorList = corridorGenerator.CreateCorridor(allSpaceNodes, corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}       // ClassEnd


#region LEGACY
/*
 * using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DungeonGenerator
{
    
    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }   // 생성자

    public List<Node> CalculateRooms(int maxIterations,int roomWidthMin,int roomLengthMin,
        float roomBottomCornerModifier,float roomTopCornermidifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);

        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(
            roomSpaces,roomBottomCornerModifier,roomTopCornermidifier,roomOffset);

        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allSpaceNodes, corridorWidth);
        //var corridorList = corridorGenerator.CreateCorridor(allNodesCollection, corridorWidth);


        return new List<Node>(roomList).Concat(corridorList).ToList();
    }

}       // ClassEnd
 */
#endregion LEGACY
