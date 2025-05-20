using UnityEngine;
using System;
using System.Collections.Generic;

public class RoomGenerator
{
    private int maxIterations;      // 생성할 방의 최대 반복 횟수
    private int roomLengthMin;      // 방의 최소 길이
    private int roomWidthMin;       // 방의 최소 너비


    // RoomGenerator 클래스의 생성자
    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    // 주어진 공간 내에서 방을 생성하고 반환하는 함수
    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier,
        float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();

        foreach (var space in roomSpaces)
        {
            // 주어진 공간 내에서 새로운 바닥 왼쪽 모서리 지점을 생성합니다.  -> 좌측하단
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetWeen(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            // 주어진 공간 내에서 새로운 바닥 오른쪽 모서리 지점을 생성합니다. -> 우측상단 
            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetWeen(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);

            // 공간의 모서리 지점을 업데이트합니다.
            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            // 생성된 방을 반환 목록에 추가합니다.
            space.isFloor = true;       // 23.11.07_09 : 52 
            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }       // GenerateRoomsInGivenSpaces()

}       // ClassEnd

#region LEGACY
/*using UnityEngine;
using System;
using System.Collections.Generic;

public class RoomGenerator
{
    private int maxIterations;
    private int roomLengthMin;
    private int roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces,float roomBottomCornerModifier,
        float roomTopCornermidifier, int roomOffset)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach (var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetWeen(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetWeen(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornermidifier, roomOffset);

            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
            listToReturn.Add((RoomNode)space);
        }
        return listToReturn;
    }
}       // ClassEnd*/
#endregion LEGACY