using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LODCreator : MonoBehaviour
{

    [Header("TransformationMesh")]
    /// <summary>
    /// 메쉬의 원본 (LOD를 적용할 매쉬)
    /// </summary>
    public Mesh originalMesh;
    /// <summary>
    /// 원본 매쉬의 클론
    /// </summary>
    private Mesh meshClone;

    [Header("Setting")]
    public bool hardMode;          // 지정한 %까지 못줄이면 강제로 줄이도록
    [Range(0, 2)]
    public int makeLODCount;       // 0 ~ 2 중 몇까지 만들것인지
    [Space]
    [Range(25, 100)]
    public int lOD_0Quality;      // LOD의 퀄리티
    [Range(25, 100)]
    public int lOD_1Quality;
    [Range(25, 100)]
    public int lOD_2Quality;
    [Space]
    [Range(1, 4)]
    public int overlapVerteicesCount;   // 정점이 몇개랑 연결되어있는것을 추릴지


    private void Start()
    {
        CreateLOD();
    }

    /// <summary>
    /// LOD제작을 시작하는 함수
    /// </summary>
    public void CreateLOD()
    {
        Debug.Log("CreateLOD는 들어오나?");

        // 폴더가 존재할 경우에만 폴더를 생성해주는 함수
#if UNITY_EDITOR
        LODFileGenerator.LODForderCreate();
#endif
        // 클론을 제작할 매쉬 오리지날과 동일하도록 제작
        meshClone = MeshCloneSetting(originalMesh);

        RemoveVertices();



    }       // CreateLOD()

    /// <summary>
    /// Vertex들을 없얘는 작업
    /// </summary>
    private void RemoveVertices()
    {
        Debug.Log("RemoveVertices()는 들어오나?");
        Mesh tempMesh = MeshCloneSetting(meshClone);

        // overlappingVerticeIndex : 위 변수에 설정한 것과 과 동일하거나 그 이상으로 vertex를 참조하는것들을 모아둔 배열
        int[] overlappingVerticeIndex = PickOverlappingVerticesIndex(tempMesh.triangles);


    }       // RemoveVertices()


    /// <summary>
    /// 새로운 매쉬를 만들고 Copy한후 반환해주는 함수
    /// </summary>
    /// <param name="originalMesh">Copy할 원본 Mesh</param>
    /// <returns></returns>
    private Mesh MeshCloneSetting(Mesh originalMesh)
    {
        Mesh cloneMesh = new Mesh();

        cloneMesh.vertices = originalMesh.vertices;
        cloneMesh.uv = originalMesh.uv;
        cloneMesh.triangles = originalMesh.triangles;
        cloneMesh.normals = originalMesh.normals;

        cloneMesh.name = originalMesh.name + "SG_LOD " + "Original";

        return cloneMesh;
    }       // MeshCloneSetting()

    /// <summary>
    /// 한 좌표를 일정수 이상의 Triangle이 참조하고있으면 추려서 반환해주는 함수
    /// </summary>
    /// <param name="_serchMeshTriangles">확인할 매쉬의 Triangles</param>
    /// <returns></returns>
    private int[] PickOverlappingVerticesIndex(int[] _serchMeshTriangles)
    {
        List<int> compareTriangleList = new List<int>();    // 같은것이 몇개존재하는지 판단할때 사용할 List
        List<int> returnTrianglesList = new List<int>();    // 결과적으로 반환할 리스트
        int isCompare;
        bool isPassTurn = false;
        
        for (int i = 0; i < _serchMeshTriangles.Length; i++)
        {
            compareTriangleList.Clear();
            #region 이미 같은좌표를 담았으면 넘기는 기능
            isPassTurn = false;
            foreach (int index in returnTrianglesList)
            {
                if(index == _serchMeshTriangles[i])
                {
                    isPassTurn = true;
                }
            }
            if(isPassTurn == true)
            {
                continue;
            }
            #endregion 이미 같은좌표를 담았으면 넘기는 기능

            for (int j = 0; j < _serchMeshTriangles.Length; j++)
            {
                isCompare = _serchMeshTriangles[j];
                if (_serchMeshTriangles[i] == isCompare)
                {   // else == PASS
                    compareTriangleList.Add(_serchMeshTriangles[j]);
                }

                if (compareTriangleList.Count >= overlapVerteicesCount)
                {
                    //Debug.Log($"중복값 확인하고 들어온 값 : {compareTriangleList[0]}");
                    returnTrianglesList.Add(compareTriangleList[0]); // 아무거나 넣어도됨 어차피 같은 값만 넣었음
                    break;
                }

            }
        }

        //foreach(int t in returnTrianglesList)
        //{
        //    Debug.Log($"제거할 VertexIndex : {t}");
        //}


        return returnTrianglesList.ToArray();

    }       // PickOverlappingVertices()

}       // ClassEnd
