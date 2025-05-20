using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    /* 매쉬의 빛반사를 위해 추가해야하는 함수
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
     */


    [Header("BSPRoom")]
    // 던전 크기 설정
    [Tooltip("던전 전체의 가로길이")]
    public int dungeonWidth;      // 던전의 넓이
    [Tooltip("던전 전체의 세로길이")]
    public int dungeonHeight;     // 던전의 높이
    // 방 크기 및 기준 설정
    public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    [Tooltip("던전 생성 최대 반복 횟수 (방 몇개 생성할것인지)")]
    public int maxIterations;       // 던전 생성 최대 반복 횟수
    [Tooltip("복도의 넓이")]
    public int corridorWidth;       // 복도 넓이

    // 방 모양 수정자 설정    
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;  // 방의 하단 모서리 수정자    
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;     // 방의 상단 모서리 수정자    
    [Range(0, 2)]
    public int roomOffset;                // 방 오프셋    
    // 벽이 생성될때에 어디에 생성할지 지정해줄 좌표        // Y축이 벽의 영향을 받음 -> ? 2024.01.23

    [Header("RoopSetting")]
    [Tooltip("지붕을 생성할 것인지")]
    public bool isRoopCreate = true;    
    [Tooltip("지붕의 포지션 (default : 0f,10f,0f)")]
    public Vector3 roopYpos = new Vector3(0f, 10f, 0f);   // default

    [Header("TagSetting")]
    [Tooltip("던전의 바닥태그를 어떤것으로 할지 (default : DungeonFloor)")]
    public string dungeonFloorTag = default;        // 생성된 던전의 바닥테그를 어떤것으로 할지

    [Header("CustomRoom")]
    public string Num = "다시 제작해야함";

    [Header("GrapicsSetting")]
    // 벽 오브젝트 설정
    public GameObject wallVertical;
    public GameObject wallHorizontal;
    // 천장,바닥 마테리얼 설정
    public Material floorMaterial;  // 던전 바닥의 재질
    public Material roopMaterial;   // 천장(지붕)의 재질



    [Header("InspectionSetting")]
    [Tooltip("던전 곂침 검사하는 객체 제거시간 (defualt : 3초)")]
    public float inspectionDestroyTime = 3f;
    [Tooltip("바닥 콜라이더의 Y포지션 (defualt : -0.5f)")]
    public float floorBoxColliderYPos = -CalculationValue.ZEROPOINTFIVE;   // 바닥 콜라이더 y포지션
    [Tooltip("바닥 콜라이더의 Y사이즈 (defualt : 1f)")]
    public float floorSize = 1f;     // 바닥 콜라이더 X,Z 크기

    #region BSP 변수
    // 커스텀 방 이후 제작된 bsp방을 관리할 List
    private List<Transform> bspRoom = default;
    // 가능한 문 및 벽 위치 목록
    private List<Vector3> possibleDoorVerticalPosition = default;
    private List<Vector3> possibleDoorHorizontalPosition = default;
    private List<Vector3> possibleWallHorizontalPosition = default;
    private List<Vector3> possibleWallVerticalPosition = default;
    private List<GameObject> bspMeshList = default;
    private List<Node> listOfRoom = default;
    private int createNum;        // 매쉬가 몇번생성되었는지 알려줄 정수

    private GameObject wallParent = default;
    private GameObject floorParent = default;
    private GameObject corridorParent = default;
    private GameObject roopParent = default;
    #endregion BSP 변수


    void Start()
    {
        DungeonValueInIt();
        // 던전 생성 시작
        CreateDungeon();
    }

    // 던전 생성 함수
    public void CreateDungeon()
    {
        // 실행할떄마다 List를 new하지말고 Clear해주고 변수의 기본값으로 변환시켜주는 함수 필요
        // 아래 리스트 new할당하는것 Awake || Start 타이밍으로 변경        
        StopAllCoroutines();
        ResetValue();

        // 기존 자식 객체 삭제
        DestroyAllChildren();

        // 던전 생성을 위한 제너레이터 초기화
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonHeight);

        // 방 목록 계산
        listOfRoom = generator.CalculateRooms(maxIterations, roomWidthMin, roomLengthMin,
            roomBottomCornerModifier, roomTopCornerModifier, roomOffset, corridorWidth);

        // 벽 부모 오브젝트 생성
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        this.wallParent = wallParent;

        // 바닥의 부모 오브젝트 생성
        GameObject floorParent = new GameObject("FloorMeshParent");
        floorParent.transform.parent = transform;
        this.floorParent = floorParent;

        // 지붕의 부모 오브젝트 생성
        GameObject roopParent = new GameObject("RoopMeshParent");
        roopParent.transform.parent = transform;
        this.roopParent = roopParent;

        // 복도의 부모 오브젝트 생성
        GameObject corridorParent = new GameObject("CorridorMeshParent");
        corridorParent.transform.parent = transform;
        this.corridorParent = corridorParent;


        // 각 방에 대한 메시 생성
        for (int i = CalculationValue.ZERO; i < listOfRoom.Count; i++)
        {
            CreateMesh(listOfRoom[i].BottomLeftAreaCorner,
                listOfRoom[i].TopRightAreaCorner, listOfRoom[i].isFloor, floorParent, corridorParent);
        }

        if (DungeonInspectionChecker.Instance.isOverlap == true)
        {
            CreateDungeon();
            return;
        }
        else { /*PASS*/ }


        StartCoroutine(BuildDelay());       

    }   // CreateDungeon()


    /// <summary>
    /// 바닥 이후 검사 통과시 구조물 생성해주는 함수
    /// </summary>
    private void CreateDungeonBuildTime()
    {
        // CunstomRoomFloorCreaste -> (FixPosition)


        // 벽 생성
        CreateWalls(wallParent);

        if (isRoopCreate == true)
        {
            // 각 방마다 지붕 생성
            for (int i = 0; i < listOfRoom.Count; i++)
            {
                CreateRoof(listOfRoom[i].BottomLeftAreaCorner,
                        listOfRoom[i].TopRightAreaCorner, roopParent);
            }
        }
        else { /*PASS*/ }


        // 각 복도에 문을 생성해주는 함수
        for (int i = 0; i < corridorParent.transform.childCount; i++)
        {
            CreateCorridorDoor(corridorParent.transform.GetChild(i));
        }


        if (DungeonInspectionChecker.Instance.isOverlap == true)
        {
            CreateDungeon();
            return;
        }
        else { /*PASS*/ }

    }       // CreateDungeonBuildTime()

    /// <summary>
    /// 변수를 초기 상태로 만들어주는 함수
    /// </summary>
    private void ResetValue()
    {
        DungeonInspectionChecker.Instance.isOverlap = false;
        bspRoom.Clear();
        bspMeshList.Clear();
        listOfRoom.Clear();
        possibleDoorVerticalPosition.Clear();
        possibleDoorHorizontalPosition.Clear();
        possibleWallHorizontalPosition.Clear();
        possibleWallVerticalPosition.Clear();

    }       // ResetValue()

    /// <summary>
    /// 던전의 변수의 값을 스프레드 시트에 있는 값으로 대입하는 함수
    /// </summary>
    private void DungeonValueInIt()
    {
        bspRoom = new List<Transform>();
        bspMeshList = new List<GameObject>();
        listOfRoom = new List<Node>();
        possibleDoorVerticalPosition = new List<Vector3>();
        possibleDoorHorizontalPosition = new List<Vector3>();
        possibleWallHorizontalPosition = new List<Vector3>();
        possibleWallVerticalPosition = new List<Vector3>();

        DungeonInspectionChecker.Instance.inspectionDestroyTime = inspectionDestroyTime;    // DungeonInspectionChecker에게 검사기 사라지는 시간 할당
        dungeonFloorTag = "DungeonFloor";

    }       // DungeonValueInIt()

    
    /// <summary>
    /// 복도 Mesh에 문을 설치해주는 컴포넌트 추가해주는 함수
    /// </summary>
    /// <param name="_corridor">복도의 parent가 가지고 있는 자식Trasform</param>
    /// <param name="_isBspRoom">해당 방이 bsp방인지 (default = true)</param>
    private void CreateCorridorDoor(Transform _corridor, bool _isBspRoom = true)
    {
        // TODO : 문 제작 하고 싶으면 구현

    }       // CreateCorridorDoor()


    /// <summary>
    ///각 방마다 지붕 생성
    /// </summary>
    private void CreateRoof(Vector2 bottomLeftCorner, Vector2 topRightCorner,
        GameObject roopParent)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, roopYpos.y, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, roopYpos.y, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, roopYpos.y, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, roopYpos.y, topRightCorner.y);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        // UV 매핑을 위한 배열 생성
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = CalculationValue.ZERO; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        // 삼각형을 정의하는 배열 생성
        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        // 메시 생성 및 설정
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        mesh.RecalculateNormals(); // JH : 쉐이더 빛 생성을 위한 노멀 생성
        mesh.RecalculateTangents();


        GameObject dungeonFloor = new GameObject("Mesh" + createNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        roopYpos.x = CalculationValue.ZERO;
        roopYpos.z = CalculationValue.ZERO;



        #region 메시의 콜라이더 Center,Size 수정

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * CalculationValue.ZEROPOINTFIVE,
            roopYpos.y, (topLeftV.z + bottomLeftV.z) * CalculationValue.ZEROPOINTFIVE);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = CalculationValue.ZEROPOINTFIVE;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < CalculationValue.ZERO) { colSizeX = -colSizeX; }
        if (colSizeZ < CalculationValue.ZERO) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size 수정

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = roopMaterial;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기


        dungeonFloor.transform.parent = roopParent.transform;
        dungeonFloor.transform.position = roopYpos;



    }   // CreateRoof()


    /// <summary>
    /// 수평 벽 및 수직 벽 생성 함수
    /// </summary>    
    private void CreateWalls(GameObject wallParent)
    {
        // 수평 벽 생성
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }

        // 수직 벽 생성
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
        //Debug.Log("수직 수평 벽 생성 끝");

    }       // CreateWalls()

    /// <summary>
    /// 벽 오브젝트 생성 함수
    /// </summary>    
    private void CreateWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        #region 벽생성만
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        #endregion 벽생성만


    }       // CreateWall()

    /// <summary>
    /// 방 크기에 따른 메시 생성 함수
    /// </summary>    
    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, CalculationValue.ZEROPOINTZERO, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, CalculationValue.ZEROPOINTZERO, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, CalculationValue.ZEROPOINTZERO, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, CalculationValue.ZEROPOINTZERO, topRightCorner.y);



        #region 바닥 생성 부분

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        // UV 매핑을 위한 배열 생성
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = CalculationValue.ZERO; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        // 삼각형을 정의하는 배열 생성
        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        // 메시 생성 및 설정
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // JH : 쉐이더 빛 생성을 위한 노멀 생성
        mesh.RecalculateTangents();

        GameObject dungeonFloor = new GameObject("Mesh" + createNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));


        bspMeshList.Add(dungeonFloor);


        DungeonTagAddAndSet(dungeonFloor, dungeonFloorTag);

        createNum++;

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * CalculationValue.ZEROPOINTFIVE,
            floorBoxColliderYPos, (topLeftV.z + bottomLeftV.z) * CalculationValue.ZEROPOINTFIVE);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < CalculationValue.ZERO) { colSizeX = -colSizeX; }
        if (colSizeZ < CalculationValue.ZERO) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;

        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = floorMaterial;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        // Obj에게 자신 꼭지점 좌표를 담을수 있는 컴포넌트 추가
        dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);


        if (isFloor == true)
        {
            dungeonFloor.transform.parent = floorParent.transform;
        }
        else
        {
            dungeonFloor.transform.parent = corridorParnet.transform;
        }

        #endregion 바닥 생성 부분

        // 벽 및 문 위치 추가

        // 가로 하단
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row + CalculationValue.ZEROPOINTFIVE, CalculationValue.ZERO, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        // 가로 상단
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row + CalculationValue.ZEROPOINTFIVE, CalculationValue.ZERO, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        // 세로 좌측
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, CalculationValue.ZERO, col + CalculationValue.ZEROPOINTFIVE);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        // 세로 우측
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, CalculationValue.ZERO, col + CalculationValue.ZEROPOINTFIVE);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }

        CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);

    }       // CreateMesh()

    /// <summary>
    /// 던전이 서로 곂치는지 확인해줄 Obj생성해주는 함수(커스텀방도 이용가능)
    /// </summary>    
    private void CreateDungeonInspection(Vector3 nodeCenter, Vector3 bottomLeftV, Vector3 bottomRightV,
        Vector3 topLeftV, GameObject dungeonFloor)
    {
        GameObject dungeonInspectionClone = new GameObject("DungeonInspection",
            typeof(DungeonInspection), typeof(BoxCollider));
        dungeonInspectionClone.transform.parent = dungeonFloor.transform;

        BoxCollider floorCol = dungeonInspectionClone.GetComponent<BoxCollider>();
        nodeCenter.y = floorBoxColliderYPos;
        floorCol.center = nodeCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < CalculationValue.ZERO) { colSizeX = -colSizeX; }
        if (colSizeZ < CalculationValue.ZERO) { colSizeZ = -colSizeZ; }
        colSizeX = colSizeX - CalculationValue.TWOPOINTZERO;
        colSizeZ = colSizeZ - CalculationValue.TWOPOINTZERO;
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;
    }       // CreateDungeonInspection()

    private void CreateFloor(Vector3 bottomLeft, Vector3 topRight, GameObject floorParent)
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(bottomLeft.x, CalculationValue.ZERO, topRight.z),
            new Vector3(topRight.x, CalculationValue.ZERO, topRight.z),
            new Vector3(bottomLeft.x, CalculationValue.ZERO, bottomLeft.z),
            new Vector3(topRight.x, CalculationValue.ZERO, bottomLeft.z)
        };
        GameObject dungeonFloor = new GameObject();

        dungeonFloor.transform.position = new Vector3((bottomLeft.x + topRight.x) * CalculationValue.ZEROPOINTFIVE,
            CalculationValue.ZERO, (bottomLeft.z + topRight.z) * CalculationValue.ZEROPOINTFIVE);

        dungeonFloor.transform.localScale = new Vector3(topRight.x - bottomLeft.x, CalculationValue.ONE, topRight.z - bottomLeft.z);

        dungeonFloor.transform.parent = floorParent.transform;
    } 


    // 벽 위치 목록에 벽 또는 문 위치 추가
    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3> wallList, List<Vector3> doorList)
    {
        //Vector3Int point = Vector3Int.CeilToInt(wallPosition);    // JH : INT 모두 Vector3 로 변경
        if (wallList.Contains(wallPosition))
        {
            doorList.Add(wallPosition);
            wallList.Remove(wallPosition);
        }
        else
        {
            wallList.Add(wallPosition);
        }
    }       // AddWallPositionToList()


    /// <summary>
    /// 모든 자식 오브젝트 삭제 함수
    /// </summary>
    private void DestroyAllChildren()
    {
        while (transform.childCount != CalculationValue.ZERO)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }       // DestroyAllChildren()

    #region CustomRoomCreate

    // 다시 제작해야함


    #endregion CustomRoomCreate


    private void CheckRecreate()
    {

    }

    IEnumerator BuildDelay()
    {
        for (int i = 0; i < CalculationValue.TEN; i++)
        {
            yield return null;
        }        

        if (DungeonInspectionChecker.Instance.isOverlap == false)
        {
            CreateDungeonBuildTime();
        }
        else
        {
            CreateDungeon();
        }

        //StartCoroutine(FloorColSizeFix()); -> 필요한가? 

    }

    IEnumerator FloorColSizeFix()
    {
        yield return new WaitForSeconds(CalculationValue.ONEPOINTZERO);

        BoxCollider boxCollider;
        Vector3 reSize;
        for (int i = 0; i < bspMeshList.Count; i++)
        {
            boxCollider = bspMeshList[i].GetComponent<BoxCollider>();
            reSize = boxCollider.size;
            reSize = new Vector3(reSize.x + CalculationValue.ONEPOINTZERO, reSize.y, reSize.z + CalculationValue.ONEPOINTZERO);
            boxCollider.size = reSize;
        }

        //Debug.Log($"던전 바닥 콜라이더 사이즈 조정 완료");
    }       // FloorColSizeFix()    

    /// <summary>
    /// 매개변수로 받은 태그가 존재하면 해당 태그로 변경하고 없다면 태그 추가후 설정
    /// </summary>
    /// <param name="_setObj">태그를 변경할 오브젝트</param>
    /// <param name="_wantTag">원하는 태그</param>
    private void DungeonTagAddAndSet(GameObject _setObj, string _wantTag = default)
    {   // 개별적 bool값 체크해주면 foreach를 덜돌겠지만 범용성이 확줄어들어서 일단 foreach채용
        if (_wantTag == default || _wantTag == null) { return; }
        else { /*PASS*/ }        
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        foreach (string tag in tags)
        {
            if (tag == _wantTag)
            {
                _setObj.transform.tag = _wantTag;
                return;
            }
            else { /*PASS*/ }
        }
        UnityEditorInternal.InternalEditorUtility.AddTag(_wantTag);
        _setObj.transform.tag = _wantTag;

    }       // DungeonTagAddAndSet()


}   // ClassEnd


#region LEGACY
/*
 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonHeight;     // 던전의 넓이와 높이
    public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    public int maxIterations;       // 최대 기준
    public int corridorWidth;       // 코리 넓이
    public Material material;
    [Range(0.0f,0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornermidifier;
    [Range(0,2)]
    public int roomOffset;

    public GameObject wallVertical, wallHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;



    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();

        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonHeight);
        var listOfRooms = generator.CalculateRooms(maxIterations, roomWidthMin, roomLengthMin,
            roomBottomCornerModifier,roomTopCornermidifier,roomOffset, corridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>(); 
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();


        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach(var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition,wallHorizontal);
        }
        foreach(var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" +bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for(int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for(int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if(wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }       // AddWallPositionToList()


    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }       // DestroyAllChildren()

}   //  ClassEnd
*/
#endregion LEGACY
