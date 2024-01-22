using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    enum DungeonTableID
    {
        // BSP Room
        DungeonWidth = 9000,
        DungeonHeight = 9001,
        RoomWidthMin = 9002,
        RoomLengthMin = 9003,
        MaxIterations = 9004,
        CorridorWidth = 9005,
        RoomBottomCornerModifier = 9006,
        RoomTopCornerModifier = 9007,
        RoomOffset = 9008,
        RoopYpos = 9009,
        // CustomRoom

        PcRoomDistance = 9101,
        PcRoomWidth = 9102,
        PcRoomHeight = 9103,
        BossRoomDistance = 9104,
        BossRoomWidth = 9105,
        BossRoomHeight = 9106,
        NextStageRoomDistance = 9107,
        NextStageRoomWidth = 9108,
        NextStageRoomHeight = 9109,
        WallBreakDownPercentage = 9110
    }

    // 확인용 변수
    private int InItNum = 1;

    #region 변수 시트화전 (Public)

    //[Header("BSPRoom")]
    //// 던전 크기 설정
    //public int dungeonWidth;      // 던전의 넓이
    //public int dungeonHeight;     // 던전의 높이
    //// 방 크기 및 기준 설정
    //public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    //public int maxIterations;       // 던전 생성 최대 반복 횟수
    //public int corridorWidth;       // 복도 넓이

    //// 그래픽 설정
    //public Material material;  // 던전 바닥의 재질

    //// 방 모양 수정자 설정
    //[Range(0.0f, 0.3f)]
    //public float roomBottomCornerModifier;  // 방의 하단 모서리 수정자
    //[Range(0.7f, 1.0f)]
    //public float roomTopCornerModifier;     // 방의 상단 모서리 수정자
    //[Range(0, 2)]
    //public int roomOffset;                // 방 오프셋    
    //// 벽이 생성될때에 어디에 생성할지 지정해줄 좌표        // Y축이 벽의 영향을 받음
    //public Vector3 roopYpos;
    ////public Vector3 roopYpos = new Vector3(1, 26, 1); LEGACY

    //[Header("CustomRoom")]
    //public float pcRoomDistance;  // 플레이어방과 첫번째 방의 거리
    //public int pcRoomWidth;       // 플레이어방 넓이
    //public int pcRoomHeight;      // 플레이어방 높이
    //private FloorMeshPos playerRoomCornerPos;       // 플레이어방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    //[Space]
    //public float bossRoomDistance;      // 마지막 방과 보스방의 거리
    //public int bossRoomWidth;           // 보스방 넓이
    //public int bossRoomHeight;          // 보스방 높이
    //private FloorMeshPos bossRoomCornerPos;         // 보스방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    //[Space]
    //public float nextStageRoomDistance; // 보스방과 다음스테이지의 거리
    //public int nextStageRoomWidth;      // 다음스테이지방 넓이
    //public int nextStageRoomHeight;     // 다음스테이지방 높이    
    //private FloorMeshPos nextStageRoomCornerPos;   // 다음스테이지방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    //[Header("WallObj")]
    //// 벽 오브젝트 설정
    //public GameObject wallVertical;
    //public GameObject wallHorizontal;
    //public GameObject wallBreakdown;
    //// 이 오브젝트는 프리펩이며 커스텀 방에서 막힌 벽을 뚫어주는데 사용될거임
    //// 벽이 한개만 있는게 아닌 3단 으로 쌓여있기에 이렇게 사용
    //public GameObject demolisherWall;

    //[Header("Prefabs")]
    //public GameObject dungeonInspection;

    //public GameObject nextStageStone;

    ////public GameObject nextStagePotal; // LEGACY

    //public GameObject entrancePrefab;
    //public GameObject exitObjPrefab;        // !현재 위치값은 매직넘버로 이루어져있음
    //public GameObject bossMonsterSkin;      // 보스몬스터 인형
    //public GameObject bossMonsterCapsule;   // 보스몬스터 본체



    //// 부숴지는벽이 나올 확률 
    //private float wallBreakDownPercentage;

    //[Header("FloorObj")]
    //// 바닥에 깔아둘 ObjPrefabs
    //public GameObject[] floorPrefabs;
    //// 가능한 문 및 벽 위치 목록
    //List<Vector3Int> possibleDoorVerticalPosition;
    //List<Vector3Int> possibleDoorHorizontalPosition;
    //List<Vector3Int> possibleWallHorizontalPosition;
    //List<Vector3Int> possibleWallVerticalPosition;

    //public float floorYPos = -0.5f;   // 바닥 콜라이더 y포지션
    //public float floorSize = 1f;     // 바닥 콜라이더 크기

    //// 커스텀 방 이후 제작된 bsp방을 관리할 List
    //List<Transform> bspRoom = new List<Transform>();


    #endregion 변수 시트화전 (Public)
    [Header("BSPRoom")]
    // 던전 크기 설정
    private int dungeonWidth;      // 던전의 넓이
    private int dungeonHeight;     // 던전의 높이
    // 방 크기 및 기준 설정
    private int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    private int maxIterations;       // 던전 생성 최대 반복 횟수
    private int corridorWidth;       // 복도 넓이

    // 그래픽 설정
    public Material material;  // 던전 바닥의 재질
    public Material roopMaterial;   // 지붕의 마테리얼

    // 방 모양 수정자 설정
    //[Range(0.0f, 0.3f)]
    private float roomBottomCornerModifier;  // 방의 하단 모서리 수정자
    //[Range(0.7f, 1.0f)]
    private float roomTopCornerModifier;     // 방의 상단 모서리 수정자
    //[Range(0, 2)]
    private int roomOffset;                // 방 오프셋    
    // 벽이 생성될때에 어디에 생성할지 지정해줄 좌표        // Y축이 벽의 영향을 받음
    private Vector3 roopYpos;
    //public Vector3 roopYpos = new Vector3(1, 26, 1); LEGACY

    [Header("CustomRoom")]
    private float pcRoomDistance;  // 플레이어방과 첫번째 방의 거리
    private int pcRoomWidth;       // 플레이어방 넓이
    private int pcRoomHeight;      // 플레이어방 높이
    private FloorMeshPos playerRoomCornerPos;       // 플레이어방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    private GameObject pcRoomFloorObj;
    [Space]
    private float bossRoomDistance;      // 마지막 방과 보스방의 거리
    private int bossRoomWidth;           // 보스방 넓이
    private int bossRoomHeight;          // 보스방 높이
    private FloorMeshPos bossRoomCornerPos;         // 보스방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    private GameObject bossRoomFloorObj;
    [Space]
    private float nextStageRoomDistance; // 보스방과 다음스테이지의 거리
    private int nextStageRoomWidth;      // 다음스테이지방 넓이
    private int nextStageRoomHeight;     // 다음스테이지방 높이    
    private FloorMeshPos nextStageRoomCornerPos;   // 다음스테이지방 좌표를 넣어주어서 방의 좌표를 알수 있게할것임
    private GameObject nextStageFloorObj;

    [Header("WallObj")]
    // 벽 오브젝트 설정
    public GameObject wallVertical;
    public GameObject wallHorizontal;
    public GameObject wallBreakdown;
    public Material twoFloorWallMat;
    // 이 오브젝트는 프리펩이며 커스텀 방에서 막힌 벽을 뚫어주는데 사용될거임
    // 벽이 한개만 있는게 아닌 3단 으로 쌓여있기에 이렇게 사용
    public GameObject demolisherWall;

    [Header("Prefabs")]
    public GameObject dungeonInspection;

    public GameObject nextStageStone;

    //public GameObject nextStagePotal; // LEGACY

    public GameObject entrancePrefab;
    public GameObject exitObjPrefab;        // !현재 위치값은 매직넘버로 이루어져있음
    public GameObject[] bossMonsterSkin;      // 보스몬스터 인형
    public GameObject[] bossMonsterCapsule;   // 보스몬스터 본체
    [SerializeField] private int[] _bossMonsterIDs =
    {
        100001,     // [1층]보스 "눈"
        100002,     // [2층]보스 "드라"
        100003,     // [3층]보스 "보헌 블레허"
        100004,     // [4층]보스 "리퍼"
        100005      // [5층]보스 "드뷔시"
    };


    // 부숴지는벽이 나올 확률 
    private float wallBreakDownPercentage;

    [Header("FloorObj")]
    // 바닥에 깔아둘 ObjPrefabs
    public GameObject[] floorPrefabs;
    // 가능한 문 및 벽 위치 목록
    private List<Vector3> possibleDoorVerticalPosition;
    private List<Vector3> possibleDoorHorizontalPosition;
    private List<Vector3> possibleWallHorizontalPosition;
    private List<Vector3> possibleWallVerticalPosition;

    public float floorYPos = -0.5f;   // 바닥 콜라이더 y포지션
    public float floorSize = 1f;     // 바닥 콜라이더 크기

    // 커스텀 방 이후 제작된 bsp방을 관리할 List
    List<Transform> bspRoom = new List<Transform>();

    List<GameObject> bspMeshList = new List<GameObject>();

    List<Node> listOfRoom = new List<Node>();

    private GameObject wallParent;
    private GameObject floorParent;
    private GameObject corridorParent;
    private GameObject roopParent;


    private int playerClearCount;

    private bool isReCreate = false;


    void Start()
    {
        playerClearCount = UserDataManager.Instance.ClearCount;
        DungeonValueInIt();
        // 던전 생성 시작
        CreateDungeon();
    }

    // 던전 생성 함수
    public void CreateDungeon()
    {
        isReCreate = false;
        DungeonInspectionManager.dungeonManagerInstance.FloorCollision = false;
        DungeonInspectionManager.dungeonManagerInstance.isEndCreateFloor = false;
        GameManager.isClearRoomList.Clear();
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
        for (int i = 0; i < listOfRoom.Count; i++)
        {
            CreateMesh(listOfRoom[i].BottomLeftAreaCorner,
                listOfRoom[i].TopRightAreaCorner, listOfRoom[i].isFloor, floorParent, corridorParent);
        }

        // 1.플레이어방 바닥 생성
        PlayerStartRoomCreate(floorParent);

        // 2.보스방 바닥 생성
        BossRoomCreate(floorParent);
        // 3.다음스테이지방 바닥 생성
        NextStageRoomCreate();

        DungeonInspectionManager.dungeonManagerInstance.isEndCreateFloor = true;
        /* 4.곂치는지 체크
         *  곂친다면 이 아래로 가지 않고 DungeonCreate 다시 호출         
         */
        CheckRecreate();

        //GFunc.Log($"바닥생성이후 isReCreate값 :{isReCreate}");
        if (isReCreate == true || floorParent.transform.childCount > 6)
        {
            GFunc.Log($"던전 재생성 호출");
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
        //GFunc.Log("던전 생성 끝");
        #endregion 잠시 Retion

    }   // CreateDungeon()


    /// <summary>
    /// 바닥 이후 검사 통과시 구조물 생성해주는 함수
    /// </summary>
    private void CreateDungeonBuildTime()
    {
        PlayerStartRoomBuild(bspMeshList[0]);      // LEGACYPAram : floorParent
        //GFunc.Log($"List 마지막에 들어온얘가 뭐지? -> {bspMeshList[^1].name}");
        int tempBossChildIdx = this.transform.GetChild(1).childCount;
        BossRoomBuild(this.transform.GetChild(1).GetChild(tempBossChildIdx - 1).gameObject);
        //BossRoomBuild(bossRoomFloorObj);
        NextStageRoomBuild();

        // 벽 생성
        CreateWalls(wallParent);

        // 각 방마다 지붕 생성
        for (int i = 0; i < listOfRoom.Count; i++)
        {
            CreateRoof(listOfRoom[i].BottomLeftAreaCorner,
                    listOfRoom[i].TopRightAreaCorner, roopParent);
        }

        // 커스텀 방
        //PlayerStartRoomCreate(floorParent);   //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (15 : 48))

        //BossRoomCreate(floorParent);          //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (16 : 08))

        //NextStageRoomCreate(); //  -> 원래 대로 사용하려면 내부에서 주석 수정해야함 (2023.12.22 (16 : 20))

        // 각 복도에 문을 생성해주는 함수
        for (int i = 0; i < corridorParent.transform.childCount; i++)
        {
            CreateCorridorDoor(corridorParent.transform.GetChild(i), true, false, false);
        }

        // BSP 각 방 셋팅        
        InItRoomsEvent(floorParent);




        //DungeonInspectionManager.dungeonManagerInstance.isCreateDungeonEnd = true;

        CheckRecreate();

        if (isReCreate == true)
        {
            GFunc.Log($"2차 검수 불통 던전 재생성 호출");
            CreateDungeon();
            return;
        }
        else { /*PASS*/ }

        GFunc.Log("던전 생성 끝");
    }       // CreateDungeonBuildTime()


    /// <summary>
    /// 던전의 변수의 값을 스프레드 시트에 있는 값으로 대입하는 함수
    /// </summary>
    private void DungeonValueInIt()
    {

        dungeonWidth = (int)DataManager.Instance.GetData((int)DungeonTableID.DungeonWidth, "DungeonWidth", typeof(int));
        dungeonHeight = (int)DataManager.Instance.GetData((int)DungeonTableID.DungeonHeight, "DungeonHeight", typeof(int));
        roomWidthMin = (int)DataManager.Instance.GetData((int)DungeonTableID.RoomWidthMin, "RoomWidthMin", typeof(int));
        roomLengthMin = (int)DataManager.Instance.GetData((int)DungeonTableID.RoomLengthMin, "RoomLengthMin", typeof(int));
        maxIterations = (int)DataManager.Instance.GetData((int)DungeonTableID.MaxIterations, "MaxIterations", typeof(int));
        corridorWidth = (int)DataManager.Instance.GetData((int)DungeonTableID.CorridorWidth, "CorridorWidth", typeof(int));
        roomBottomCornerModifier = (float)DataManager.Instance.GetData((int)DungeonTableID.RoomBottomCornerModifier, "RoomBottomCornerModifier", typeof(float));
        roomTopCornerModifier = (float)DataManager.Instance.GetData((int)DungeonTableID.RoomTopCornerModifier, "RoomTopCornerModifier", typeof(float));
        roomOffset = (int)DataManager.Instance.GetData((int)DungeonTableID.RoomOffset, "RoomOffset", typeof(int));
        roopYpos = new Vector3(1f, (float)DataManager.Instance.GetData((int)DungeonTableID.RoopYpos, "RoopYpos", typeof(float)), 1f);

        // CustomRoom
        pcRoomDistance = (float)DataManager.Instance.GetData((int)DungeonTableID.PcRoomDistance, "PcRoomDistance", typeof(float));
        pcRoomWidth = (int)DataManager.Instance.GetData((int)DungeonTableID.PcRoomWidth, "PcRoomWidth", typeof(int));
        pcRoomHeight = (int)DataManager.Instance.GetData((int)DungeonTableID.PcRoomHeight, "PcRoomHeight", typeof(int));

        bossRoomDistance = (float)DataManager.Instance.GetData((int)DungeonTableID.BossRoomDistance, "BossRoomDistance", typeof(float));
        bossRoomWidth = (int)DataManager.Instance.GetData((int)DungeonTableID.BossRoomWidth, "BossRoomWidth", typeof(int));
        bossRoomHeight = (int)DataManager.Instance.GetData((int)DungeonTableID.BossRoomHeight, "BossRoomHeight", typeof(int));

        nextStageRoomDistance = (float)DataManager.Instance.GetData((int)DungeonTableID.NextStageRoomDistance, "NextStageRoomDistance", typeof(float));
        nextStageRoomWidth = (int)DataManager.Instance.GetData((int)DungeonTableID.NextStageRoomWidth, "NextStageRoomWidth", typeof(int));
        nextStageRoomHeight = (int)DataManager.Instance.GetData((int)DungeonTableID.NextStageRoomHeight, "NextStageRoomHeight", typeof(int));
        wallBreakDownPercentage = (float)DataManager.Instance.GetData((int)DungeonTableID.WallBreakDownPercentage, "WallBreakDownPercentage", typeof(float));




    }       // DungeonValueInIt()

    /// <summary>
    /// 복도 Mesh에 문을 설치해주는 컴포넌트 추가해주는 함수
    /// </summary>
    /// <param name="transform">복도의 parent가 가지고 있는 자식Trasform</param>
    private void CreateCorridorDoor(Transform _corridor, bool _isBspRoom,
        bool _isBossRoom, bool _isNextRoom)
    {
        _corridor.gameObject.AddComponent<CorridorColorChange>();
        if (_isBspRoom == true)
        {
            _corridor.gameObject.AddComponent<CorridorDoorCreate>();
        }
        else if (_isBossRoom == true)
        {
            _corridor.gameObject.AddComponent<BossRoomCorridorDoorCreate>();
        }
        else if (_isNextRoom == true)
        {
            _corridor.gameObject.AddComponent<NextStageRoomCorridorDoorCreate>();
        }
    }       // CreateCorridorDoor()


    /// <summary>
    /// 각방에 이벤트 : 전투 or 이벤트 무작위로 넣어줌
    /// </summary>
    /// <param name="floorParent">방의 갯수는 parent의 ChildrenCount로 계산</param>
    private void InItRoomsEvent(GameObject floorParent)
    {         // 나중에 Llst대신 Queue를 사용해서 EnQueue로 넣고 DeQueue로 뺴는 식으로 해서 Count로 Random돌려도 될거같음  
        int roomCount = floorParent.transform.childCount;
        int battleRoomCount = roomCount / 3;
        int eventRoom = battleRoomCount;
        int nullRoom = battleRoomCount;
        //int battleRoomCount = 2;
        //int eventRoom = 2;
        //int nullRoom = 2;
        //GFunc.Log($"RoomCount : {roomCount} NullRoom : {nullRoom}");

        for (int i = 0; i < floorParent.transform.childCount; i++)
        {       // List에 bsp의방 Transform을 Add
            bspRoom.Add(floorParent.transform.GetChild(i));
        }
        // clone으로 만들어서 하나씩 remove하면서 각자 방에 넣어줄 예정
        List<Transform> bspListClone = bspRoom;
        //GFunc.Log($"ListCount : {bspListClone.Count}");
        //GFunc.Log($"event -> {eventRoom} Battle -> {battleRoomCount}");
        while (battleRoomCount != 0 || eventRoom != 0 || nullRoom != 0)
        {
            int randomIdx = UnityEngine.Random.Range(0, bspListClone.Count);
            int randomEvent = UnityEngine.Random.Range(0, 3);       // 0 ~ 2
            if (bspListClone[randomIdx] == null)
            {
                break;
            }

            if (randomEvent == 0 && battleRoomCount != 0)
            {
                bspListClone[randomIdx].AddComponent<BattleRoomObjCreate>();
                bspListClone[randomIdx].AddComponent<BattleRoomRunTimeNav>();
                bspListClone[randomIdx].AddComponent<BattleRoom>();
                battleRoomCount -= 1;
                bspListClone.Remove(bspListClone[randomIdx]);
            }
            else if (randomEvent == 1 && eventRoom != 0)
            {
                bspListClone[randomIdx].AddComponent<EventRoom>();
                bspListClone[randomIdx].AddComponent<EventRoomObjCreate>();
                eventRoom -= 1;
                bspListClone.Remove(bspListClone[randomIdx]);
            }
            else if (randomEvent == 2 && nullRoom != 0)
            {
                bspListClone[randomIdx].AddComponent<NullRoom>();
                bspListClone[randomIdx].AddComponent<NullRoomObjCreate>();
                nullRoom -= 1;
                bspListClone.Remove(bspListClone[randomIdx]);
            }
        }

        if (bspListClone[0] != null)
        {
            CreateDungeon();
        }       

        //GFunc.LogFormat("각방 이벤트 선정 끝");



    }       // InItRoomsEvent()

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
        for (int i = 0; i < uvs.Length; i++)
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


        GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        roopYpos.x = 0;
        roopYpos.z = 0;


        InItNum++;

        #region 메시의 콜라이더 Center,Size 수정

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, roopYpos.y, (topLeftV.z + bottomLeftV.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = 0.5f;
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
        //GFunc.Log("수직 수평 벽 생성 끝");

    }       // CreateWalls()

    /// <summary>
    /// 벽 오브젝트 생성 함수
    /// </summary>    
    private void CreateWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        #region 벽생성만
        //Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        #endregion 벽생성만
        #region 1층구조 벽 생성(벽 생성후 YPos조절까지)
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        //GameObject wallObjClone;
        //wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        //Vector3 wallPos = wallObjClone.transform.position;
        //wallPos.y = wallObjClone.transform.localScale.y / 2;
        //wallObjClone.transform.position = wallPos;
        #endregion 1층구조 벽 생성(벽 생성후 YPos조절까지)
        #region 2층 구조 벽 생성 (한 칸에 벽 2개 생성 2번째 생성되는벽은 Y축 추가 조절)
        float randomCreatWall = UnityEngine.Random.Range(0, 1001);     // 0 ~ 1000 임시
        // 나중에 sclae이 다른 벽을 Instance할때에 position잡을때 직전에 만든 벽의 Y를 담아둘 변수
        float tempWallPosY;
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        GameObject wallObjClone;
        if (randomCreatWall < wallBreakDownPercentage)
        {
            wallObjClone = Instantiate(wallBreakdown, wallPosition, Quaternion.identity, wallParent.transform);
            wallObjClone.tag = "EventWall";
        }
        else
        {
            wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
            wallObjClone.tag = "Wall";
        }


        Vector3 wallPos = wallObjClone.transform.position;
        wallPos.y = wallObjClone.transform.localScale.y / 2;

        wallObjClone.transform.position = wallPos;

        // 2번째 벽 생성 
        wallPos.y = wallPos.y * 3;
        tempWallPosY = wallPos.y;
        wallObjClone = Instantiate(wallPrefab, wallPos, Quaternion.identity, wallParent.transform);
        wallObjClone.tag = "Wall";
        // 3번째 벽 생성
        wallObjClone = Instantiate(wallPrefab, wallPos, Quaternion.identity, wallParent.transform);
        wallObjClone.transform.localScale = new Vector3(1f, 22f, 1f);
        wallPos.y = +(tempWallPosY * 1.33f) + (wallObjClone.transform.localScale.y / 2);
        wallObjClone.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, 9f);
        wallObjClone.GetComponent<MeshRenderer>().material = twoFloorWallMat;
        wallObjClone.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        //wallPos.y = +((2.5f * 2f) / 2) + (wallObjClone.transform.localScale.y / 2); 나중에 이 방식으로 식 바꾸기 시도해봐야겠음
        wallObjClone.transform.position = wallPos;
        wallObjClone.tag = "Wall";



        //roopYpos.y = wallObjClone.transform.localScale.y;

        #endregion 2층 구조 벽 생성 (한 칸에 벽 2개 생성 2번째 생성되는벽은 Y축 추가 조절)



    }       // CreateWall()

    /// <summary>
    /// 방 크기에 따른 메시 생성 함수
    /// </summary>    
    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

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
        for (int i = 0; i < uvs.Length; i++)
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

        GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));


        bspMeshList.Add(dungeonFloor);  // -> 이부분이 좀 의심됨


        dungeonFloor.gameObject.tag = "Floor";

        InItNum++;

        #region 메시에 해당 매쉬 꼭지점들을 알수 있도록 스크립트 넣어주고 해당 좌표 생성자로 기입
        //dungeonFloor.AddComponent<FloorMesh>();
        #endregion 메시에 해당 매쉬 꼭지점들을 알수 있도록 스크립트 넣어주고 해당 좌표 생성자로 기입

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, floorYPos, (topLeftV.z + bottomLeftV.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;

        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
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
            var wallPosition = new Vector3(row + 0.5f, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        // 가로 상단
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row + 0.5f, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        // 세로 좌측
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col + 0.5f);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        // 세로 우측
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col + 0.5f);
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
        GameObject dungeonInspectionClone;
        dungeonInspectionClone = Instantiate(dungeonInspection, Vector3.zero, Quaternion.identity, dungeonFloor.transform);

        BoxCollider floorCol = dungeonInspectionClone.GetComponent<BoxCollider>();
        nodeCenter.y = floorYPos;
        floorCol.center = nodeCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        colSizeX = colSizeX - 2f;
        colSizeZ = colSizeZ - 2f;
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;
    }       // CreateDungeonInspection()

    private void CreateFloor(Vector3 bottomLeft, Vector3 topRight, GameObject floorParent)
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(bottomLeft.x, 0, topRight.z),
            new Vector3(topRight.x, 0, topRight.z),
            new Vector3(bottomLeft.x, 0, bottomLeft.z),
            new Vector3(topRight.x, 0, bottomLeft.z)
        };
        GameObject dungeonFloor = Instantiate(floorPrefabs[0]);
        dungeonFloor.transform.position = new Vector3((bottomLeft.x + topRight.x) / 2, 0, (bottomLeft.z + topRight.z) / 2);
        dungeonFloor.transform.localScale = new Vector3(topRight.x - bottomLeft.x, 1, topRight.z - bottomLeft.z);
        dungeonFloor.transform.parent = floorParent.transform;
    }


    // 방 크기에 따른 메시 와 바닥 생성
    private void CreateMeshInFloor(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        // 프리팹으로 바닥 생성
        int numTilesX = Mathf.FloorToInt((topRightV.x - bottomLeftV.x) / 1);
        int numTilesZ = Mathf.FloorToInt((topRightV.z - bottomLeftV.z) / 1);

        for (int x = 0; x < numTilesX; x++)
        {
            for (int z = 0; z < numTilesZ; z++)
            {
                Vector3 tileBottomLeft = new Vector3(bottomLeftV.x + x * 1, 0, bottomLeftV.z + z * 1);
                Vector3 tileTopRight = new Vector3(tileBottomLeft.x + 1, 0, tileBottomLeft.z + 1);
                CreateFloor(tileBottomLeft, tileTopRight, floorParent);

            }
        }


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


        //GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftCorner,
        //    typeof(MeshFilter), typeof(MeshRenderer),typeof(BoxCollider));

        //InItNum++;

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
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }       // DestroyAllChildren()

    #region CustomRoomCreate
    /// <summary>
    /// // 플레이어 시작 위치 방 생성하는 함수
    /// </summary>    
    private void PlayerStartRoomCreate(GameObject floorParent)
    {       // 고정적인 방

        // 처음으로 매쉬가 생성된 방의 꼭지점Pos 얻기
        FloorMeshPos firstRoomPos = floorParent.transform.GetChild(0).GetComponent<FloorMeshPos>();
        //GFunc.LogFormat("FPChildCount -> {0}", floorParent.transform.childCount);

        // 방의 하단 중앙위치
        float bspfirstRoomBottomCenterPoint = (firstRoomPos.bottomLeftCorner.x + firstRoomPos.bottomRightCorner.x) * 0.5f;
        // 방의 상단 중앙위치
        float bspFirstRoomTopCenterPoint = (firstRoomPos.topLeftCorner.x + firstRoomPos.topRightCorner.x) * 0.5f;

        //// 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 topLeftV = new Vector3
            (bspfirstRoomBottomCenterPoint - (pcRoomWidth * 0.5f), 0f, firstRoomPos.bottomLeftCorner.z - pcRoomDistance);
        Vector3 topRightV = new Vector3
            (bspfirstRoomBottomCenterPoint + (pcRoomWidth * 0.5f), 0f, firstRoomPos.bottomRightCorner.z - pcRoomDistance);
        Vector3 bottomLeftV = new Vector3(topLeftV.x, 0f, topLeftV.z - pcRoomHeight);
        Vector3 bottomRightV = new Vector3(topRightV.x, 0f, topLeftV.z - pcRoomHeight);

        //GFunc.Log($"11PCMeshPos!\n pcRoomWidth : {pcRoomWidth}\nPCRoomHeight : {pcRoomHeight}\n PCRoomDistance : {pcRoomDistance}");
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
        for (int i = 0; i < uvs.Length; i++)
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
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        //GFunc.Log($"PCMeshPos!\n TopLeft : {topLeftV}\n TopRight : {topRightV}\n BottomLeft :{bottomLeftV}\n BottomRight : {bottomRightV}");


        GameObject dungeonFloor = new GameObject("PCRoomMesh" + InItNum + bottomLeftV,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));

        pcRoomFloorObj = dungeonFloor;
        //pcRoomFloorObj.transform.parent = this.transform;
        /* 던전 1 차수정 주석 처리 _ 2023.12.22_ 15:35 */
        //GameObject wallParnet = new GameObject("CustomRoomWallParent");
        dungeonFloor.transform.parent = this.transform;
        //wallParnet.transform.parent = dungeonFloor.transform;

        dungeonFloor.gameObject.tag = "Floor";

        InItNum++;

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        //Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0, (topLeftV.z + bottomLeftV.z) / 2);
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * 0.5f, floorYPos, (topLeftV.z + bottomLeftV.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        colSizeY = 1;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        //Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;

        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        // Obj에게 자신 꼭지점 좌표를 담을수 있는 컴포넌트 추가
        dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);
        playerRoomCornerPos = dungeonFloor.GetComponent<FloorMeshPos>();


        // 필요 필드 변수
        // GameObject ,Vector3 -> FloorMeshPos로 대체
        // 이 위에 까지가 바닥 생성
        CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);

        #region PlayerStart CustomRoom
        //CustomRoomCorridorCreateMinusPos(wallParnet, bottomLeftV, bottomRightV, topLeftV, topRightV, false);
        //CustomRoomCorridorMeshCreate(false, bspfirstRoomBottomCenterPoint, bspFirstRoomTopCenterPoint, firstRoomPos, dungeonFloor, false, false, false);
        //CreatePlayerRoomRoof(bottomLeftV, bottomRightV, topLeftV, topRightV, dungeonFloor);
        //CreateEntrance(bottomLeftV, bottomRightV, topLeftV, topRightV, dungeonFloor);
        #endregion PlayerStart CustomRoom
    }       // PlayerStartRoomCreate()

    /// <summary>
    /// 플레이어 바닥 이외의것을 추가적으로 생성하는 함수
    /// </summary>
    private void PlayerStartRoomBuild(GameObject _bspFloorParent)
    {
        GameObject wallParnet = new GameObject("CustomRoomWallParent");
        wallParnet.transform.parent = pcRoomFloorObj.transform;

        // 처음으로 매쉬가 생성된 방의 꼭지점Pos 얻기
        //FloorMeshPos firstRoomPos = _bspFloorParent.transform.GetChild(0).GetComponent<FloorMeshPos>();
        FloorMeshPos firstRoomPos = _bspFloorParent.transform.GetComponent<FloorMeshPos>();
        //GFunc.Log($"FloorMeshPos == null? : {firstRoomPos == null}");
        // 방의 하단 중앙위치
        float bspfirstRoomBottomCenterPoint = (firstRoomPos.bottomLeftCorner.x + firstRoomPos.bottomRightCorner.x) * 0.5f;
        // 방의 상단 중앙위치
        float bspFirstRoomTopCenterPoint = (firstRoomPos.topLeftCorner.x + firstRoomPos.topRightCorner.x) * 0.5f;


        CustomRoomCorridorCreateMinusPos(wallParnet, playerRoomCornerPos.bottomLeftCorner, playerRoomCornerPos.bottomRightCorner
            , playerRoomCornerPos.topLeftCorner, playerRoomCornerPos.topRightCorner, false);

        CustomRoomCorridorMeshCreate(false, bspfirstRoomBottomCenterPoint, bspFirstRoomTopCenterPoint,
            firstRoomPos, _bspFloorParent, false, false, false);

        CreatePlayerRoomRoof(playerRoomCornerPos.bottomLeftCorner, playerRoomCornerPos.bottomRightCorner,
            playerRoomCornerPos.topLeftCorner, playerRoomCornerPos.topRightCorner, _bspFloorParent);

        CreateEntrance(playerRoomCornerPos.bottomLeftCorner, playerRoomCornerPos.bottomRightCorner,
            playerRoomCornerPos.topLeftCorner, playerRoomCornerPos.topRightCorner, _bspFloorParent);

    }       // PlayerStartRoomBuild()

    /// <summary>
    /// 플레이어 리스폰 지역 생성하는 함수
    /// </summary>    
    private void CreateEntrance(Vector3 bottomLeftV, Vector3 bottomRightV,
        Vector3 topLeftV, Vector3 topRightV, GameObject dungeonFloor)
    {
        GameObject prefabClone = entrancePrefab;
        Vector3 roomTopCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * 0.5f, (roopYpos.y + 7) + 100,
            (bottomLeftV.z + topLeftV.z) * 0.5f);

        prefabClone = Instantiate(entrancePrefab, roomTopCenter, Quaternion.identity, dungeonFloor.transform);

    }       // CreateEntrance()

    /// <summary>
    /// 플레이어 시작 위치에 벽 생성해주는 함수 (해당 방의 포지션이 음수인지 양수인지에 따라 계산이 다르기때문에 둘로 쪼개놓음)
    /// </summary>    
    private void CustomRoomCorridorCreateMinusPos(GameObject wallParent_,
        Vector3 bottomLeftV_, Vector3 bottomRightV_, Vector3 topLeftV_, Vector3 topRightV_, bool isCustomCorridor)
    {       // 매개변수는 위 PlayerStartRoomCreate에 있는 Vector3변수를 이용
        Vector3 createPos;

        // 좌측 세로
        createPos = bottomLeftV_;
        for (float i = bottomLeftV_.z; i < topLeftV_.z; i++)
        {
            createPos.z = i;
            CreateCustomRoomWall(wallParent_, createPos, wallVertical);
        }
        // 우측 세로
        createPos = bottomRightV_;
        for (float i = bottomRightV_.z; i < topRightV_.z; i++)
        {
            createPos.z = i;
            CreateCustomRoomWall(wallParent_, createPos, wallVertical);
        }
        if (isCustomCorridor == false)
        {
            // 상단 가로
            createPos = topLeftV_;
            for (float i = topLeftV_.x; i < topRightV_.x; i++)
            {
                createPos.x = i;
                CreateCustomRoomWall(wallParent_, createPos, wallHorizontal);
            }
            // 하단 가로
            createPos = bottomLeftV_;
            for (float i = bottomLeftV_.x; i < bottomRightV_.x; i++)
            {
                createPos.x = i;
                CreateCustomRoomWall(wallParent_, createPos, wallHorizontal);
            }
        }
        else if (isCustomCorridor == true)
        {
            //상단 세로
            createPos = topLeftV_;
            for (float i = topLeftV_.x + 1.5f; i < topRightV_.x - 1; i++)
            {
                createPos.x = i;
                CreateDemolisherWall(wallParent_, createPos, demolisherWall);
            }
            // 하단 가로
            createPos = bottomLeftV_;
            for (float i = bottomLeftV_.x + 1.5f; i < bottomRightV_.x - 1; i++)
            {
                createPos.x = i;
                CreateDemolisherWall(wallParent_, createPos, demolisherWall);
            }
        }


    }       // PlayerStartRoomCorridorCreate()


    /// <summary>
    /// 커스텀룸와 인근 방을 이어주는 복도 제작       
    /// </summary>
    /// isPositive : PC = flase ,Boss : true, NextStage : ture
    private void CustomRoomCorridorMeshCreate(bool isPositive_, float bspRoomBottomCenterPoint_,
        float bspRoomTopCenterPoint_, FloorMeshPos bspRoomPos_, GameObject parentRoom_,
        bool _isBspRoom, bool _isBossRoom, bool _isNextRoom)
    {
        Vector3 topLeftV;
        Vector3 topRightV;
        Vector3 bottomLeftV;
        Vector3 bottomRightV;

        if (isPositive_ == false)
        {       // if : CustomPCRoom
            // 바닥 메시 생성을 위한 꼭지점 좌표 설정
            topLeftV = new Vector3
                (bspRoomBottomCenterPoint_ - (corridorWidth * 0.5f), 0f, bspRoomPos_.bottomLeftCorner.z);
            topRightV = new Vector3
                (bspRoomBottomCenterPoint_ + (corridorWidth * 0.5f), 0f, bspRoomPos_.bottomLeftCorner.z);

            bottomLeftV = new Vector3(topLeftV.x, 0f, topLeftV.z - pcRoomDistance);
            bottomRightV = new Vector3(topRightV.x, 0f, topLeftV.z - pcRoomDistance);

        }
        else
        {   // else : BossRoom,NextStageRoom
            bottomLeftV = new Vector3
                (bspRoomTopCenterPoint_ - (corridorWidth / 2), 0f, bspRoomPos_.topLeftCorner.z);
            bottomRightV = new Vector3
                (bspRoomTopCenterPoint_ + (corridorWidth / 2), 0f, bspRoomPos_.topRightCorner.z);

            topLeftV = new Vector3(bottomLeftV.x, 0f, bottomLeftV.z + bossRoomDistance);
            topRightV = new Vector3(bottomRightV.x, 0f, bottomRightV.z + bossRoomDistance);
        }
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
        for (int i = 0; i < uvs.Length; i++)
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

        GameObject dungeonFloor;
        if (isPositive_ == false)
        {
            dungeonFloor = new GameObject("PCRoomCorridorMesh" + InItNum + bottomLeftV,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        }
        else
        {
            dungeonFloor = new GameObject("Boss||NestStageRoomCorridorMesh" + InItNum + bottomLeftV,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        }


        dungeonFloor.gameObject.tag = "Floor";

        InItNum++;

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        //Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0f, (topLeftV.z + bottomLeftV.z) / 2);
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, floorYPos, (topLeftV.z + bottomLeftV.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;

        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        // Obj에게 자신 꼭지점 좌표를 담을수 있는 컴포넌트 추가
        dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);

        dungeonFloor.transform.parent = parentRoom_.transform;

        CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);

        if (isPositive_ == false)
        { CustomRoomCorridorCreateMinusPos(dungeonFloor, bottomLeftV, bottomRightV, topLeftV, topRightV, true); }
        else { CustomRoomCorridorCreatePlusPos(dungeonFloor, bottomLeftV, bottomRightV, topLeftV, topRightV, true); }

        bottomLeftV.y = roopYpos.y;
        bottomRightV.y = roopYpos.y;
        topLeftV.y = roopYpos.y;
        topRightV.y = roopYpos.y;
        CreateCustomRoomRoof(bottomLeftV, bottomRightV, topLeftV, topRightV, parentRoom_);
        CreateCorridorDoor(dungeonFloor.transform, _isBspRoom, _isBossRoom, _isNextRoom);

    }       // PlayerStartRoomCorridorCreate()

    /// <summary>
    /// 벽 오브젝트 생성 함수     CustomRoomCorridorCreateMinusPos() 참조용
    /// </summary>    
    private void CreateCustomRoomWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        // 나중에 sclae이 다른 벽을 Instance할때에 position잡을때 직전에 만든 벽의 Y를 담아둘 변수
        float tempWallPosY;
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        GameObject wallObjClone;

        wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        Vector3 wallPos = wallObjClone.transform.position;
        wallPos.y = wallObjClone.transform.localScale.y / 2;
        wallObjClone.transform.position = wallPos;
        wallObjClone.tag = "Wall";

        // 2번째 벽 생성 
        wallPos.y = wallPos.y * 3;
        tempWallPosY = wallPos.y;
        wallObjClone = Instantiate(wallPrefab, wallPos, Quaternion.identity, wallParent.transform);
        wallObjClone.tag = "Wall";
        // 3번째 벽 생성
        wallObjClone = Instantiate(wallPrefab, wallPos, Quaternion.identity, wallParent.transform);
        wallObjClone.transform.localScale = new Vector3(1f, 22f, 1f);
        wallObjClone.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1f, 9f);
        wallObjClone.GetComponent<MeshRenderer>().material = twoFloorWallMat;
        wallObjClone.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        wallPos.y = +(tempWallPosY * 1.33f) + (wallObjClone.transform.localScale.y / 2);

        //wallPos.y = +((2.5f * 2f) / 2) + (wallObjClone.transform.localScale.y / 2); 나중에 이 방식으로 식 바꾸기 시도해봐야겠음
        wallObjClone.transform.position = wallPos;
        wallObjClone.tag = "Wall";

    }       // CreateWall()

    /// <summary>
    /// 벽 파괴하는 벽 생성해주는 함수 CustomRoomCorridorCreateMinusPos() 참조용
    /// </summary>    
    private void CreateDemolisherWall(GameObject wallParent, Vector3 wallPosition, GameObject wallPrefab)
    {
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        GameObject wallObjClone;

        wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        Vector3 wallPos = wallObjClone.transform.position;
        wallPos.y = wallObjClone.transform.localScale.y / 2;
        wallObjClone.transform.position = wallPos;

    }       // CreateDemolisherWall()

    /// <summary>
    /// 커스텀방 지붕 생성
    /// </summary>    
    private void CreateCustomRoomRoof(Vector3 bottomLeftV_, Vector3 bottomRightV_,
        Vector3 topLeftV_, Vector3 topRightV_, GameObject roopParent)
    {
        topLeftV_.y = roopYpos.y;
        topRightV_.y = roopYpos.y;
        bottomLeftV_.y = roopYpos.y;
        bottomRightV_.y = roopYpos.y;

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV_,
            topRightV_,
            bottomLeftV_,
            bottomRightV_
        };

        // UV 매핑을 위한 배열 생성
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
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

        GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftV_,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        roopYpos.x = 0;
        roopYpos.z = 0;


        InItNum++;

        #region 메시의 콜라이더 Center,Size 수정

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        Vector3 colCenter = new Vector3((bottomLeftV_.x + bottomRightV_.x) / 2, roopYpos.y, (topLeftV_.z + bottomLeftV_.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV_.x - bottomRightV_.x;
        colSizeY = 0.03f;
        colSizeZ = bottomLeftV_.z - topLeftV_.z;
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
    /// 플레이어방 지붕 생성
    /// </summary>    
    private void CreatePlayerRoomRoof(Vector3 bottomLeftV_, Vector3 bottomRightV_,
        Vector3 topLeftV_, Vector3 topRightV_, GameObject roopParent)
    {
        topLeftV_.y = roopYpos.y;
        topRightV_.y = roopYpos.y;
        bottomLeftV_.y = roopYpos.y;
        bottomRightV_.y = roopYpos.y;

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV_,
            topRightV_,
            bottomLeftV_,
            bottomRightV_
        };

        // UV 매핑을 위한 배열 생성
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
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

        GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftV_,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
        roopYpos.x = 0;
        roopYpos.z = 0;


        InItNum++;

        #region 메시의 콜라이더 Center,Size 수정

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        Vector3 colCenter = new Vector3((bottomLeftV_.x + bottomRightV_.x) / 2, roopYpos.y, (topLeftV_.z + bottomLeftV_.z) / 2);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV_.x - bottomRightV_.x;
        colSizeY = 0.03f;
        colSizeZ = bottomLeftV_.z - topLeftV_.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        PlayerRoomTopFirstTrrigerController playerRoomRoof = dungeonFloor.AddComponent<PlayerRoomTopFirstTrrigerController>();
        playerRoomRoof.GetBoxCollider(floorCol);
        playerRoomRoof.StartTrrigerOn();
        #endregion 메시의 콜라이더 Center,Size 수정

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = roopMaterial;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        dungeonFloor.transform.parent = roopParent.transform;
        dungeonFloor.transform.position = roopYpos;

    }   // CreateRoof()

    private void BossRoomCreate(GameObject floorParent)
    {

        // 처음으로 매쉬가 생성된 방의 꼭지점Pos 얻기
        FloorMeshPos lastRoomPos = floorParent.transform.GetChild(floorParent.transform.childCount - 1).GetComponent<FloorMeshPos>();
        //GFunc.LogFormat("FPChildCount -> {0}", floorParent.transform.childCount);

        // 방의 하단 중앙위치
        float bspLastRoomBottomCenterPoint = (lastRoomPos.bottomLeftCorner.x + lastRoomPos.bottomRightCorner.x) / 2;
        // 방의 상단 중앙위치
        float bspLastRoomTopCenterPoint = (lastRoomPos.topLeftCorner.x + lastRoomPos.topRightCorner.x) / 2;

        //// 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3
            (bspLastRoomTopCenterPoint - (bossRoomWidth / 2), 0f, lastRoomPos.topLeftCorner.z + bossRoomDistance);
        Vector3 bottomRightV = new Vector3
            (bspLastRoomTopCenterPoint + (bossRoomWidth / 2), 0f, lastRoomPos.topRightCorner.z + bossRoomDistance);
        Vector3 topLeftV = new Vector3(bottomLeftV.x, 0f, bottomLeftV.z + bossRoomHeight);
        Vector3 topRightV = new Vector3(bottomRightV.x, 0f, bottomRightV.z + bossRoomHeight);

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
        for (int i = 0; i < uvs.Length; i++)
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

        GameObject dungeonFloor = new GameObject("BossRoomMesh" + InItNum + bottomLeftV,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));

        // 던전 1차 수정 2023.12.22 (15 : 53)
        //GameObject wallParnet = new GameObject("CustomRoomWallParent");
        //dungeonFloor.transform.parent = this.transform;
        //wallParnet.transform.parent = dungeonFloor.transform;

        bossRoomFloorObj = dungeonFloor;
        dungeonFloor.transform.parent = this.transform;

        dungeonFloor.gameObject.tag = "Floor";

        InItNum++;

        #region 메시의 콜라이더 Center,Size

        //메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        //Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * 0.5f, floorYPos, (topLeftV.z + bottomLeftV.z) * 0.5f);
        BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        floorCol.center = colCenter;
        // Size
        float colSizeX, colSizeY, colSizeZ;
        colSizeX = bottomLeftV.x - bottomRightV.x;
        colSizeY = floorSize;
        colSizeZ = bottomLeftV.z - topLeftV.z;
        // 음수값이 나오면 양수로 치환
        if (colSizeX < 0) { colSizeX = -colSizeX; }
        if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;

        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;  // JH : 그림자 꺼주기

        // Obj에게 자신 꼭지점 좌표를 담을수 있는 컴포넌트 추가
        dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);
        bossRoomCornerPos = dungeonFloor.GetComponent<FloorMeshPos>();

        CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);

        // CustomRoomCorridorCreatePlusPos(wallParnet, bottomLeftV, bottomRightV, topLeftV, topRightV, false);        
        //CustomRoomCorridorMeshCreate(true, bspLastRoomBottomCenterPoint, bspLastRoomTopCenterPoint, lastRoomPos, dungeonFloor,false,true,false);
        //CreateCustomRoomRoof(bottomLeftV, bottomRightV, topLeftV, topRightV, dungeonFloor);
        //CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);
        //BossRoomAddComponent(bossRoomCornerPos, dungeonFloor,colCenter);
    }       // BossRoomCreate()


    private void BossRoomBuild(GameObject _bspRoomParent)
    {
        // 맨 마지막에 생성된 방의 좌표 얻기
        //FloorMeshPos lastRoomPos = _bspRoomParent.transform.GetChild(_bspRoomParent.transform.childCount - 1).GetComponent<FloorMeshPos>();
        FloorMeshPos lastRoomPos = _bspRoomParent.transform.GetComponent<FloorMeshPos>();

        Vector3 roomCenter = new Vector3((bossRoomCornerPos.bottomLeftCorner.x + bossRoomCornerPos.bottomRightCorner.x) * 0.5f,
            floorYPos,
            (bossRoomCornerPos.topLeftCorner.z + bossRoomCornerPos.bottomLeftCorner.z) * 0.5f);

        // 방의 하단 중앙위치
        float bspLastRoomBottomCenterPoint = (lastRoomPos.bottomLeftCorner.x + lastRoomPos.bottomRightCorner.x) * 0.5f;
        // 방의 상단 중앙위치
        float bspLastRoomTopCenterPoint = (lastRoomPos.topLeftCorner.x + lastRoomPos.topRightCorner.x) * 0.5f;

        GameObject wallParnet = new GameObject("CustomRoomWallParent");
        wallParnet.transform.parent = bossRoomFloorObj.transform;

        CustomRoomCorridorCreatePlusPos(wallParnet, bossRoomCornerPos.bottomLeftCorner, bossRoomCornerPos.bottomRightCorner,
            bossRoomCornerPos.topLeftCorner, bossRoomCornerPos.topRightCorner, false);

        CustomRoomCorridorMeshCreate(true, bspLastRoomBottomCenterPoint, bspLastRoomTopCenterPoint,
            lastRoomPos, bossRoomFloorObj, false, true, false);

        CreateCustomRoomRoof(bossRoomCornerPos.bottomLeftCorner, bossRoomCornerPos.bottomRightCorner,
            bossRoomCornerPos.topLeftCorner, bossRoomCornerPos.topRightCorner, bossRoomFloorObj);

        BossRoomAddComponent(bossRoomCornerPos, bossRoomFloorObj, roomCenter);
    }

    /// <summary>
    /// 보스방 바닥에 보스 스폰과 사망 처리 해주는 컴포넌트 Add해주는 함수
    /// </summary>    
    private void BossRoomAddComponent(FloorMeshPos _floorMeshPos, GameObject _dungeonFloor, Vector3 _centerPos)
    {
        #region LEGACY
        //GameObject monsterParent = new GameObject("BossMonster");
        //monsterParent.transform.parent = _dungeonFloor.transform;

        //GameObject bossClone;
        //Vector3 bossPos = _centerPos;
        //bossPos.y = bossPos.y + 3f;

        //bossClone = Instantiate(bossMonsterSkin, bossPos, Quaternion.Euler(0f, 180f, 0f), monsterParent.transform);
        ////bossClone.transform.localRotation = Quaternion.EulerRotation
        //bossPos = _centerPos;
        //bossPos.z = bossPos.z - 3f;
        //bossPos.y = 1f;


        //bossClone = Instantiate(bossMonsterCapsule, bossPos, Quaternion.identity, monsterParent.transform);
        #endregion LEGACY

        // LEGACY:
        //_dungeonFloor.AddComponent<BossRoom>().VariablesInIt(_floorMeshPos, bossMonsterSkin[GameManager.instance.nowFloor - 1], bossMonsterCapsule[GameManager.instance.nowFloor - 1], _dungeonFloor.transform, _centerPos);

        int bossID = _bossMonsterIDs[GameManager.instance.nowFloor - 1];
        _dungeonFloor.AddComponent<BossRoom>().NewVariablesInIt(_floorMeshPos, bossID, _dungeonFloor.transform, _centerPos);
    }       // CreateBossMonster()

    /// <summary>
    /// 보스방,다음스테이지방 위치에 벽 생성해주는 함수 (해당 방의 포지션이 음수인지 양수인지에 따라 계산이 다르기때문에 둘로 쪼개놓음)           
    /// bool 값 : Mesh = false,  Corridor = ture
    /// </summary>    
    private void CustomRoomCorridorCreatePlusPos(GameObject wallParent_,
        Vector3 bottomLeftV_, Vector3 bottomRightV_, Vector3 topLeftV_, Vector3 topRightV_, bool isCustomCorridor)
    {       // 매개변수는 위 PlayerStartRoomCreate에 있는 Vector3변수를 이용
        Vector3 createPos;

        // 좌측 세로
        createPos = bottomLeftV_;
        for (float i = bottomLeftV_.z; i < topLeftV_.z; i++)
        {
            createPos.z = i;
            CreateCustomRoomWall(wallParent_, createPos, wallVertical);
        }
        // 우측 세로
        createPos = bottomRightV_;
        for (float i = bottomRightV_.z; i < topRightV_.z; i++)
        {
            createPos.z = i;
            CreateCustomRoomWall(wallParent_, createPos, wallVertical);
        }
        if (isCustomCorridor == false)
        {
            // 상단 가로
            createPos = topLeftV_;
            for (float i = topLeftV_.x; i < topRightV_.x; i++)
            {
                createPos.x = i;
                CreateCustomRoomWall(wallParent_, createPos, wallHorizontal);
            }
            // 하단 가로
            createPos = bottomLeftV_;
            for (float i = bottomLeftV_.x; i < bottomRightV_.x; i++)
            {
                createPos.x = i;
                CreateCustomRoomWall(wallParent_, createPos, wallHorizontal);
            }
        }
        else if (isCustomCorridor == true)
        {
            //상단 세로
            createPos = topLeftV_;
            for (float i = topLeftV_.x + 1.5f; i < topRightV_.x - 1; i++)
            {
                createPos.x = i;
                CreateDemolisherWall(wallParent_, createPos, demolisherWall);
            }
            // 하단 가로
            createPos = bottomLeftV_;
            for (float i = bottomLeftV_.x + 1.5f; i < bottomRightV_.x - 1; i++)
            {
                createPos.x = i;
                CreateDemolisherWall(wallParent_, createPos, demolisherWall);
            }
        }

    }       // PlayerStartRoomCorridorCreate()

    /// <summary>
    /// 다음 스테이지로 이동하기위한방 생성
    /// </summary>    
    private void NextStageRoomCreate()
    {

        // 방의 하단 중앙위치
        float bspLastRoomBottomCenterPoint = (bossRoomCornerPos.bottomLeftCorner.x + bossRoomCornerPos.bottomRightCorner.x) * 0.5f;
        // 방의 상단 중앙위치
        float bspLastRoomTopCenterPoint = (bossRoomCornerPos.topLeftCorner.x + bossRoomCornerPos.topRightCorner.x) * 0.5f;

        //// 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3
            (bspLastRoomTopCenterPoint - (nextStageRoomWidth * 0.5f), 0f, bossRoomCornerPos.topLeftCorner.z + nextStageRoomDistance);
        Vector3 bottomRightV = new Vector3
            (bspLastRoomTopCenterPoint + (nextStageRoomWidth * 0.5f), 0f, bossRoomCornerPos.topRightCorner.z + nextStageRoomDistance);
        Vector3 topLeftV = new Vector3(bottomLeftV.x, 0f, bottomLeftV.z + nextStageRoomHeight);
        Vector3 topRightV = new Vector3(bottomRightV.x, 0f, bottomRightV.z + nextStageRoomHeight);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };
        #region LEGACY
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

        #endregion LEGACY

        GameObject dungeonFloor = new GameObject("NextStageRoomMesh" + InItNum + bottomLeftV);
        //GameObject dungeonFloor = new GameObject("NextStageRoomMesh" + InItNum + bottomLeftV,
        //    typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));

        //GameObject wallParnet = new GameObject("CustomRoomWallParent");
        ////dungeonFloor.transform.parent = this.transform;
        //wallParnet.transform.parent = dungeonFloor.transform;
        dungeonFloor.transform.parent = this.transform;

        dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);
        nextStageRoomCornerPos = dungeonFloor.GetComponent<FloorMeshPos>();

        nextStageFloorObj = dungeonFloor;

        //dungeonFloor.gameObject.tag = "Floor";

        //InItNum++;

        //#region 메시의 콜라이더 Center,Size

        ////메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        ////Center
        Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0f, (topLeftV.z + bottomLeftV.z) / 2);

        #region LEGACY
        //BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        //floorCol.center = colCenter;
        //// Size
        //float colSizeX, colSizeY, colSizeZ;
        //colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        //colSizeZ = bottomLeftV.z - topLeftV.z;
        //// 음수값이 나오면 양수로 치환
        //if (colSizeX < 0) { colSizeX = -colSizeX; }
        //if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        //Vector3 colSize = new Vector3(colSizeX, colSizeY, colSizeZ);
        //floorCol.size = colSize;

        //BoxCollider stepOffCol = dungeonFloor.AddComponent<BoxCollider>();
        //dungeonFloor.AddComponent<NextstageRoomColliderController>().GetColliders(floorCol, stepOffCol);


        //stepOffCol.center = colCenter;
        //stepOffCol.size = new Vector3((float)nextStageRoomUnderObjCount, 0f, (float)nextStageRoomUnderObjCount);
        //#endregion 메시의 콜라이더 Center,Size

        //dungeonFloor.transform.position = Vector3.zero;
        //dungeonFloor.transform.localScale = Vector3.one;

        //dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        //dungeonFloor.GetComponent<MeshRenderer>().material = material;

        // Obj에게 자신 꼭지점 좌표를 담을수 있는 컴포넌트 추가
        //dungeonFloor.AddComponent<FloorMeshPos>().InItPos(bottomLeftV, bottomRightV, topLeftV, topRightV);
        //nextStageRoomCornerPos = dungeonFloor.GetComponent<FloorMeshPos>();

        #endregion LEGACY


        //CustomRoomCorridorCreatePlusPos(wallParnet, bottomLeftV, bottomRightV, topLeftV, topRightV, false);
        //CustomRoomCorridorMeshCreate(true, bspLastRoomBottomCenterPoint, bspLastRoomTopCenterPoint, bossRoomCornerPos_, dungeonFloor,false,false,true);
        //CreateCustomRoomRoof(bottomLeftV, bottomRightV, topLeftV, topRightV, dungeonFloor);
        //CreateExitObj(bottomLeftV, bottomRightV, topLeftV, topRightV, dungeonFloor);
        CreateDungeonInspection(colCenter, bottomLeftV, bottomRightV, topLeftV, dungeonFloor);

        //CreateNextStageStoneObj(colCenter, dungeonFloor);

    }       // NextStageRoomCreate()

    private void NextStageRoomBuild()
    {
        GameObject wallParnet = new GameObject("CustomRoomWallParent");
        wallParnet.transform.parent = nextStageFloorObj.transform;

        // 방의 하단 중앙위치
        float bspLastRoomBottomCenterPoint = (bossRoomCornerPos.bottomLeftCorner.x + bossRoomCornerPos.bottomRightCorner.x) * 0.5f;
        // 방의 상단 중앙위치
        float bspLastRoomTopCenterPoint = (bossRoomCornerPos.topLeftCorner.x + bossRoomCornerPos.topRightCorner.x) * 0.5f;



        CustomRoomCorridorCreatePlusPos(wallParnet, nextStageRoomCornerPos.bottomLeftCorner, nextStageRoomCornerPos.bottomRightCorner,
            nextStageRoomCornerPos.topLeftCorner, nextStageRoomCornerPos.topRightCorner, false);

        CustomRoomCorridorMeshCreate(true, bspLastRoomBottomCenterPoint, bspLastRoomTopCenterPoint, bossRoomCornerPos,
            nextStageFloorObj, false, false, true);

        CreateCustomRoomRoof(nextStageRoomCornerPos.bottomLeftCorner, nextStageRoomCornerPos.bottomRightCorner,
            nextStageRoomCornerPos.topLeftCorner, nextStageRoomCornerPos.topRightCorner, nextStageFloorObj);

        CreateExitObj(nextStageRoomCornerPos.bottomLeftCorner, nextStageRoomCornerPos.bottomRightCorner,
            nextStageRoomCornerPos.topLeftCorner, nextStageRoomCornerPos.topRightCorner, nextStageFloorObj);

    }       // NextStageRoomBuild()



    /// <summary>
    /// 다음방 이동 오브젝트 생성
    /// </summary>    
    private void CreateExitObj(Vector3 bottomLeftV, Vector3 bottomRightV,
        Vector3 topLeftV, Vector3 topRightV, GameObject dungeonFloor)
    {
        GameObject prefabClone = exitObjPrefab;
        Vector3 roomUnderCenter = new Vector3((bottomLeftV.x + bottomRightV.x) * 0.5f, 0f,
            (bottomLeftV.z + topLeftV.z) * 0.5f);

        prefabClone = Instantiate(exitObjPrefab, roomUnderCenter, Quaternion.identity, dungeonFloor.transform);

    }       // CreateExitObj()
    #region LEGACY
    ///// <summary>
    ///// 다음 던전이동 하기위한 뚫리는 돌 오브젝트 생성
    ///// </summary>
    //private void CreateNextStageStoneObj(Vector3 meshCenter, GameObject dungeonFloor)
    //{
    //    // 포지션과 스케일 값을 수정해줄 Vector3
    //    Vector3 cloneScale = new Vector3(3f, 3f, 3f);
    //    Vector3 clonePos;

    //    // 프리펩을 클론딸 게임오브젝트
    //    GameObject nextStageStoneObj;

    //    nextStageStoneObj = Instantiate(nextStageStone, meshCenter, Quaternion.identity, dungeonFloor.transform);

    //    // 이후 스프레트 시트로 바뀔수 있음        
    //    nextStageStoneObj.transform.localScale = cloneScale;

    //    clonePos = nextStageStoneObj.transform.position;
    //    clonePos.y = (-nextStageStoneObj.transform.localScale.y / 2) + 0.1f;
    //    nextStageStoneObj.transform.position = clonePos;

    //    CreateNextStageStoneWall(meshCenter, dungeonFloor, cloneScale, clonePos);
    //}       // CreateNextStageStoneObj()

    ///// <summary>
    ///// 다음 스테이지 이동시 뚫리는 바닥주위로 벽 생성해주는 함수
    ///// </summary>   
    //private void CreateNextStageStoneWall(Vector3 colCenter, GameObject dungeonFloor,
    //    Vector3 stoneScale, Vector3 stonePos)
    //{   // 던전의 벽만들듯이 꼭지점을 이용해서 제작
    //    Vector3 bottomLeftV;
    //    Vector3 bottomRightV;
    //    Vector3 topLeftV;
    //    Vector3 topRightV;

    //    bottomLeftV = new Vector3(((colCenter.x - (stoneScale.x / 2)) - 1f), colCenter.y - 1f,
    //        ((colCenter.z - (stoneScale.z / 2)) - 1f));
    //    bottomRightV = new Vector3(((colCenter.x + (stoneScale.x / 2)) + 1f), colCenter.y - 1f,
    //        ((colCenter.z - (stoneScale.z / 2)) - 1f));
    //    topLeftV = new Vector3(bottomLeftV.x, bottomLeftV.y, (colCenter.z + (stoneScale.z / 2f)) + 1f);
    //    topRightV = new Vector3(bottomRightV.x, bottomRightV.y, (colCenter.z + (stoneScale.z / 2f)) + 1f);                                                                                                                                                                                                                                                                             

    //    // 땅속 벽들을 담아둘 게임 오브젝트
    //    GameObject underWalls = new GameObject("UnderWall");
    //    underWalls.transform.parent = dungeonFloor.transform;
    //    // 인스턴스하기위한 게임오브젝트
    //    GameObject underWallClone;
    //    Vector3 tempPosition = bottomLeftV;     // 인스턴스해줄 위치를 저장할 임시 V3
    //    float tempPosX = tempPosition.x;
    //    #region 가로벽 생성
    //    // 가로벽 생성
    //    // Bottom
    //    for (float horizontalY = 0f; horizontalY < (stoneScale.y * 2) * 3; horizontalY++)
    //    {       // 높이
    //        tempPosition.x = tempPosX;

    //        for (float horizontal = bottomLeftV.x; horizontal <= bottomRightV.x; horizontal++)
    //        {       // 가로 깔기
    //            underWallClone = Instantiate(floorPrefabs[0],tempPosition,Quaternion.identity,underWalls.transform);
    //            tempPosition.x = tempPosition.x + 1;
    //        }
    //        tempPosition.y -= 1f;
    //    }
    //    tempPosition = topLeftV;
    //    tempPosX = tempPosition.x;
    //    // Top
    //    for (float horizontalY = 0f; horizontalY < (stoneScale.y * 2) * 3; horizontalY++)
    //    {       // 높이
    //        tempPosition.x = tempPosX;
    //        for (float horizontal = topLeftV.x; horizontal <= topRightV.x; horizontal++)            
    //        {   // 가로
    //            underWallClone = Instantiate(floorPrefabs[0], tempPosition, Quaternion.identity, underWalls.transform);
    //            tempPosition.x += 1f;
    //        }
    //        tempPosition.y -= 1f;
    //    }
    //    #endregion 가로벽 생성

    //    tempPosition = bottomLeftV;
    //    float tempPosZ = tempPosition.z;
    //    #region 세로벽 생성
    //    // 세로벽 생성
    //    // L
    //    for (float verticalY = 0; verticalY < (stoneScale.y * 2) * 3; verticalY++)
    //    {       // 높이
    //        tempPosition.z = tempPosZ;
    //        for (float vertical = bottomLeftV.z; vertical <= topLeftV.z; vertical++)
    //        {   // 세로
    //            underWallClone = Instantiate(floorPrefabs[0], tempPosition, Quaternion.identity, underWalls.transform);
    //            tempPosition.z += 1f;
    //        }
    //        tempPosition.y -= 1f;
    //    }
    //    tempPosition = bottomRightV;
    //    tempPosZ = tempPosition.z;
    //    // R
    //    for (float verticalY = 0; verticalY < (stoneScale.y * 2) * 3; verticalY++)
    //    {       // 높이
    //        tempPosition.z = tempPosZ;
    //        for (float vertical = bottomRightV.z; vertical <= topRightV.z; vertical++)
    //        {   // 세로
    //            underWallClone = Instantiate(floorPrefabs[0], tempPosition, Quaternion.identity, underWalls.transform);
    //            tempPosition.z += 1f;
    //        }
    //        tempPosition.y -= 1f;
    //    }

    //    #endregion 세로벽 생성

    //    CreateNextStagePotal(dungeonFloor, bottomLeftV, bottomRightV, topLeftV, topRightV);
    //}       // CreateNextStageStoneWall()

    ///// <summary>
    ///// 다음스테이지로 이동시켜줄 게임오브젝트 생성
    ///// </summary>   
    //private void CreateNextStagePotal(GameObject dungeonFloor, Vector3 bottomLeftV, 
    //    Vector3 bottomRightV, Vector3 topLeftV, Vector3 topRightV)
    //{       // TODO : 1회차 다회차 마다 어디로 이동시켜줄지 구별시켜야함
    //    Vector3 underCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, -18f, (bottomLeftV.z + topLeftV.z) / 2);

    //    GameObject potalClone;

    //    potalClone = Instantiate(nextStagePotal, underCenter, Quaternion.identity, dungeonFloor.transform);        
    //    potalClone.AddComponent<NextStagePotal>();                

    //}       // CreateNextStagePotal()

    #endregion LEGACY


    #endregion CustomRoomCreate


    private void CheckRecreate()
    {
        if (DungeonInspectionManager.dungeonManagerInstance.FloorCollision == true)
        {
            isReCreate = true;
        }
        else { /*Pass*/ }
    }

    IEnumerator BuildDelay()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        CheckRecreate();

        if (isReCreate == false)
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
        yield return new WaitForSeconds(10);

        BoxCollider boxCollider;
        Vector3 reSize;
        for (int i = 0; i < bspMeshList.Count; i++)
        {
            boxCollider = bspMeshList[i].GetComponent<BoxCollider>();
            reSize = boxCollider.size;
            reSize = new Vector3(reSize.x + 1f, reSize.y, reSize.z + 1f);
            boxCollider.size = reSize;
        }

        GFunc.Log($"던전 바닥 콜라이더 사이즈 조정 완료");
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
