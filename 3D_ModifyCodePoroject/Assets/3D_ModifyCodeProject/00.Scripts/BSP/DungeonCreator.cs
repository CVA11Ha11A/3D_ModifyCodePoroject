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
    public int dungeonWidth;      // 던전의 넓이
    public int dungeonHeight;     // 던전의 높이
    // 방 크기 및 기준 설정
    public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    public int maxIterations;       // 던전 생성 최대 반복 횟수
    public int corridorWidth;       // 복도 넓이

    // 그래픽 설정
    public Material floorMaterial;  // 던전 바닥의 재질
    public Material roopMaterial;   // 천장(지붕)의 재질

    // 방 모양 수정자 설정
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;  // 방의 하단 모서리 수정자
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;     // 방의 상단 모서리 수정자
    [Range(0, 2)]
    public int roomOffset;                // 방 오프셋    
    // 벽이 생성될때에 어디에 생성할지 지정해줄 좌표        // Y축이 벽의 영향을 받음
    public Vector3 roopYpos;
    //public Vector3 roopYpos = new Vector3(1, 26, 1); LEGACY

    [Header("CustomRoom")]
    public string Num = "다시 제작해야함";

    [Header("WallObj")]
    // 벽 오브젝트 설정
    public GameObject wallVertical;
    public GameObject wallHorizontal;                    


    

    [Header("FloorObj")]    
    // 가능한 문 및 벽 위치 목록
    private List<Vector3> possibleDoorVerticalPosition;
    private List<Vector3> possibleDoorHorizontalPosition;
    private List<Vector3> possibleWallHorizontalPosition;
    private List<Vector3> possibleWallVerticalPosition;

    public float floorYPos = -CalculationValue.ZEROPOINTFIVE;   // 바닥 콜라이더 y포지션
    public float floorSize = 1f;     // 바닥 콜라이더 X,Z 크기

    // 커스텀 방 이후 제작된 bsp방을 관리할 List
    private List<Transform> bspRoom = default;
  

    private int initNum;        // 매쉬가 몇번생성되었는지 알려줄 정수



    [Header("FloorObj")]
    // 바닥에 깔아둘 ObjPrefabs


    List<GameObject> bspMeshList = new List<GameObject>();

    List<Node> listOfRoom = new List<Node>();

    private GameObject wallParent;
    private GameObject floorParent;
    private GameObject corridorParent;
    private GameObject roopParent;        


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
        DungeonInspectionChecker.Instance.isOverlap = false;
        StopAllCoroutines();
        bspMeshList.Clear();
        bspRoom.Clear();
        listOfRoom.Clear();

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
        possibleDoorVerticalPosition = new List<Vector3>();
        possibleDoorHorizontalPosition = new List<Vector3>();
        possibleWallHorizontalPosition = new List<Vector3>();
        possibleWallVerticalPosition = new List<Vector3>();
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


        // ! 던전 재생성 부분 다시 제작해야함 ! 

        CheckRecreate();
     
        if (DungeonInspectionChecker.Instance.isOverlap == true)
        {            
            CreateDungeon();
            return;
        }
        else { /*PASS*/ }

        
        StartCoroutine(BuildDelay());

        //====================================== 이 아래부터는 바닥 생성 이후 ===================================
        #region 잠시 Retion
        //PlayerStartRoomBuild(bspMeshList[0]);      // LEGACYPAram : floorParent
        //BossRoomBuild(bspMeshList[^1]);
        //NextStageRoomBuild();

        #region 땅바닥 OBj 생성
        ////각 방에 대한 땅바닥Obj 생성
        ////for (int i = 0; i < listOfRooms.Count; i++)
        ////{
        ////    CreateMeshInFloor(listOfRooms[i].BottomLeftAreaCorner,
        ////        listOfRooms[i].TopRightAreaCorner, listOfRooms[i].isFloor, floorParent, corridorParnet);
        ////}
        #endregion 땅바닥 OBj 생성

        //// 벽 생성
        //CreateWalls(wallParent);

        //// 각 방마다 지붕 생성
        //for (int i = 0; i < listOfRooms.Count; i++)
        //{
        //    CreateRoof(listOfRooms[i].BottomLeftAreaCorner,
        //            listOfRooms[i].TopRightAreaCorner, roopParent);
        //}

        //// 커스텀 방
        ////PlayerStartRoomCreate(floorParent);   //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (15 : 48))

        ////BossRoomCreate(floorParent);          //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (16 : 08))

        ////NextStageRoomCreate(); //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (16 : 20))

        //// 각 복도에 문을 생성해주는 함수
        //for (int i = 0; i < corridorParnet.transform.childCount; i++)
        //{
        //    CreateCorridorDoor(corridorParnet.transform.GetChild(i),true,false,false);
        //}

        //// BSP 각 방 셋팅        
        //InItRoomsEvent(floorParent);




        ////DungeonInspectionManager.dungeonManagerInstance.isCreateDungeonEnd = true;
        //Debug.Log("던전 생성 끝");
        #endregion 잠시 Retion

    }   // CreateDungeon()


    /// <summary>
    /// 바닥 이후 검사 통과시 구조물 생성해주는 함수
    /// </summary>
    private void CreateDungeonBuildTime()
    {
        // CunstomRoomFloorCreaste -> (FixPosition)


        // 벽 생성
        CreateWalls(wallParent);

        // 각 방마다 지붕 생성
        for (int i = 0; i < listOfRoom.Count; i++)
        {
            CreateRoof(listOfRoom[i].BottomLeftAreaCorner,
                    listOfRoom[i].TopRightAreaCorner, roopParent);
        }
        

        // 각 복도에 문을 생성해주는 함수
        for (int i = 0; i < corridorParent.transform.childCount; i++)
        {
            CreateCorridorDoor(corridorParent.transform.GetChild(i), true, false, false);
        }
                
        CheckRecreate();

        if (DungeonInspectionChecker.Instance.isOverlap == true)
        {
            CreateDungeon();
            return;
        }
        else { /*PASS*/ }
        
    }       // CreateDungeonBuildTime()


    /// <summary>
    /// 던전의 변수의 값을 스프레드 시트에 있는 값으로 대입하는 함수
    /// </summary>
    private void DungeonValueInIt()
    {
        bspRoom = new List<Transform>();

    }       // DungeonValueInIt()

    /// <summary>
    /// 복도 Mesh에 문을 설치해주는 컴포넌트 추가해주는 함수
    /// </summary>
    /// <param name="transform">복도의 parent가 가지고 있는 자식Trasform</param>
    private void CreateCorridorDoor(Transform _corridor, bool _isBspRoom,
        bool _isBossRoom, bool _isNextRoom)
    {
            // TODO : 다시제작해야함
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


        GameObject dungeonFloor = new GameObject("Mesh" + initNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        roopYpos.x = CalculationValue.ZERO;
        roopYpos.z = CalculationValue.ZERO;



        #region 메시의 콜라이더 Center,Size 수정

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, roopYpos.y, (topLeftV.z + bottomLeftV.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = CalculationValue.ZEROPOINTFIVE;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
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

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };



        #region temp //
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

        GameObject dungeonFloor = new GameObject("Mesh" + initNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));


        bspMeshList.Add(dungeonFloor);  // -> 이부분이 좀 의심됨


        dungeonFloor.gameObject.tag = "Floor";

        initNum++;

        #region 메시에 해당 매쉬 꼭지점들을 알수 있도록 스크립트 넣어주고 해당 좌표 생성자로 기입
        //dungeonFloor.AddComponent<FloorMesh>();
        #endregion 메시에 해당 매쉬 꼭지점들을 알수 있도록 스크립트 넣어주고 해당 좌표 생성자로 기입

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * CalculationValue.ZEROPOINTFIVE,
            floorYPos, (topLeftV.z + bottomLeftV.z) * CalculationValue.ZEROPOINTFIVE);
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

        //dungeonFloor.transform.parent = meshParent.transform;       // TODO : Node에 있는 bool값에 따라서 따라갈 parent를 정해줘야함
        //dungeonFloor.transform.parent = transform; : LEGACY

        #endregion temp //

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
        GameObject dungeonInspectionClone = new GameObject("DungeonInspection", typeof(DungeonInspection));        
        dungeonInspectionClone.transform.parent = dungeonFloor.transform;

        BoxCollider floorCol = dungeonInspectionClone.GetComponent<BoxCollider>();
        nodeCenter.y = floorYPos;
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
            CalculationValue.ZERO,(bottomLeft.z + topRight.z) * CalculationValue.ZEROPOINTFIVE);

        dungeonFloor.transform.localScale = new Vector3(topRight.x - bottomLeft.x, CalculationValue.ONE, topRight.z - bottomLeft.z);

        dungeonFloor.transform.parent = floorParent.transform;
    }


    // 방 크기에 따른 메시 와 바닥 생성
    private void CreateMeshInFloor(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, CalculationValue.ZERO, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, CalculationValue.ZERO, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, CalculationValue.ZERO, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, CalculationValue.ZERO, topRightCorner.y);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        // 프리팹으로 바닥 생성
        int numTilesX = Mathf.FloorToInt((topRightV.x - bottomLeftV.x) / CalculationValue.ONE);
        int numTilesZ = Mathf.FloorToInt((topRightV.z - bottomLeftV.z) / CalculationValue.ONE);

        for (int x = 0; x < numTilesX; x++)
        {
            for (int z = 0; z < numTilesZ; z++)
            {
                Vector3 tileBottomLeft = new Vector3(bottomLeftV.x + x * CalculationValue.ONE,
                    CalculationValue.ZERO, bottomLeftV.z + z * CalculationValue.ONE);

                Vector3 tileTopRight = new Vector3(tileBottomLeft.x + CalculationValue.ONE,
                    CalculationValue.ZERO,tileBottomLeft.z + CalculationValue.ONE);
                CreateFloor(tileBottomLeft, tileTopRight, floorParent);

            }
        }       // CreateMeshInFloor()


        #region temp //
        //// UV 매핑을 위한 배열 생성
        //Vector2[] uvs = new Vector2[vertices.Length];
        //for (int i = 0; i < uvs.Length; i++)
        //{
        //    uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //// 삼각형을 정의하는 배열 생성
        //int[] triangles = new int[]
        //{
        //    0,
        //    1,
        //    2,
        //    2,
        //    1,
        //    3
        //};

        //// 메시 생성 및 설정
        //Mesh mesh = new Mesh();
        //mesh.vertices = vertices;
        //mesh.uv = uvs;
        //mesh.triangles = triangles;


        //GameObject dungeonFloor = new GameObject("Mesh" + initNum + bottomLeftCorner,
        //    typeof(MeshFilter), typeof(MeshRenderer),typeof(BoxCollider));

        //initNum++;

        #region 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        //Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0f, (topLeftV.z + bottomLeftV.z) / 2);
        //BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        //floorCol.center = colCenter;
        //// Size
        //float colSizeX, colSizeY, colSizeZ;
        //colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        //colSizeZ = bottomLeftV.z - topLeftV.z;
        //// 음수값이 나오면 양수로 치환
        //if(colSizeX < 0) {  colSizeX = -colSizeX; }
        //if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        //Vector3 colSize = new Vector3(colSizeX,colSizeY,colSizeZ);
        //floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        //dungeonFloor.transform.position = Vector3.zero;
        //dungeonFloor.transform.localScale = Vector3.one;
        //dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        //dungeonFloor.GetComponent<MeshRenderer>().material = material;
        //if (isFloor == true)
        //{
        //    dungeonFloor.transform.parent = floorParent.transform;
        //}
        //else
        //{
        //    dungeonFloor.transform.parent = corridorParnet.transform;
        //}

        //dungeonFloor.transform.parent = meshParent.transform;       // TODO : Node에 있는 bool값에 따라서 따라갈 parent를 정해줘야함
        //dungeonFloor.transform.parent = transform; : LEGACY

        #endregion temp //

        // 벽 및 문 위치 추가
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
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

        CheckRecreate();

        if (DungeonInspectionChecker.Instance.isOverlap == false)
        {
            CreateDungeonBuildTime();
        }
        else
        {
            CreateDungeon();
        }

        StartCoroutine(FloorColSizeFix());

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

        Debug.Log($"던전 바닥 콜라이더 사이즈 조정 완료");
    }

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
