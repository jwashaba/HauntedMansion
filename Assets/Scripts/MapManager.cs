// using UnityEngine;
// using System.Collections;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using UnityEngine.Tilemaps;

// public class MapManager : MonoBehaviour
// {
//     [SerializeField] private Camera cam;
//     [SerializeField] private GameObject player;

//     [Header(("Map Objects"))]
//     [SerializeField] private GameObject mapPrefab;
//     [SerializeField] private GameObject currMap;
//     [SerializeField] private GameObject _nextMap;
//     private TilemapRenderer _currMapRenderer;
//     [SerializeField] private int _mapCount = 0;

//     [Header("Obstacles")]
//     [SerializeField] private GameObject[] obstaclePrefabs;
//     [SerializeField] private int obstacleGap; // Gap between obstacles
//     [SerializeField] private int minObstaclesPerChunk;
//     [SerializeField] private int maxObstaclesPerChunk;
//     [SerializeField] private int _setCount = 0; // Number of times a chunk of obstacles have been spawned
//     private ArrayList _currObstacles = new ArrayList(); // purely obstacle-tagged objects in chunk
//     private ArrayList _nextObstacles = new ArrayList();
//     private ArrayList _currObjects = new ArrayList(); // any objects in chunk
//     private ArrayList _nextObjects = new ArrayList();

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         _currMapRenderer = currMap.transform.GetChild(0).GetComponent<TilemapRenderer>(); // get first layer's renderer
//         _nextMap = Instantiate(mapPrefab, new Vector3(0f, _currMapRenderer.bounds.size.y, 0f), Quaternion.identity);
//     }

//     void SwapTilemaps()
//     {
//         // set current map
//         Vector3 savedPos = new Vector3(0f, _nextMap.transform.position.y, 0f);
//         Destroy(currMap);
//         currMap = _nextMap;
//         _currMapRenderer = currMap.transform.GetChild(0).GetComponent<TilemapRenderer>(); // get first layer's renderer

//         _nextMap = Instantiate(mapPrefab, savedPos + new Vector3(0f, _currMapRenderer.bounds.size.y, 0f), Quaternion.identity);
//         _mapCount++;

//         // GenerateObstacles(savedPos); // don't call until we actually have obstacles
//     }
    
//     private void GenerateObstacles(Vector3 savedPos)
//     {
//         _currObstacles = _nextObstacles;

//         int setsToSpawn = Random.Range(minObstaclesPerChunk, maxObstaclesPerChunk + 1); // Number of obstacles to spawn in this chunk
//         _nextObstacles = new ArrayList();
//         for (int i = 0; i < setsToSpawn; i++)
//         {
//             _setCount++;

//             int randInt = Random.Range(0, obstaclePrefabs.Length);
//             int obstacleOffset = maxObstaclesPerChunk / setsToSpawn * obstacleGap * (i); // Offset to evenly space out obstacles depending on how many there are
//             GameObject setObject = Instantiate(obstaclePrefabs[randInt], savedPos + new Vector3(0f, _currMapRenderer.bounds.size.y + obstacleOffset), Quaternion.identity);
//             // tracking and collecting obstacles for publisher/subscriber system while glitch coin is active
//             for (int j = 0; j < setObject.transform.childCount; j++)
//             {
//                 GameObject child = setObject.transform.GetChild(j).gameObject;
//                 if (child.CompareTag("Obstacle"))
//                 {
//                     _nextObstacles.Add(child);
//                 }
//                 _nextObjects.Add(child);
//             }
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), _currMapRenderer.bounds)) SwapTilemaps();
//     }
// }


using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [Header("Scene Refs")]
    [SerializeField] private Camera cam;                // orthographic camera that follows player upward
    [SerializeField] private GameObject mapPrefab;      // room prefab
    [SerializeField] private GameObject currMap;        // starting room instance in the scene

    private GameObject nextMap;
    private Bounds currBounds;                          // combined tilemap bounds for current room
    private int mapCount = 0;

    void Start()
    {
        currBounds = GetMapBounds(currMap);
        // spawn next so its bottom sits on current top
        Vector3 nextPos = new Vector3(
            currBounds.center.x,
            currBounds.max.y + currBounds.extents.y,
            0f);
        nextMap = Instantiate(mapPrefab, nextPos, Quaternion.identity);
    }

    void Update()
    {
        // trigger when the CAMERA top crosses current room's top
        float camTop = cam.transform.position.y + cam.orthographicSize;
        if (camTop >= currBounds.max.y - 0.05f)
        {
            SwapTilemaps();
        }
    }

    private void SwapTilemaps()
    {
        Destroy(currMap);
        currMap = nextMap;

        currBounds = GetMapBounds(currMap);

        Vector3 nextPos = new Vector3(
            currBounds.center.x,
            currBounds.max.y + currBounds.extents.y,
            0f);
        nextMap = Instantiate(mapPrefab, nextPos, Quaternion.identity);

        mapCount++;
    }

    // Combine bounds of all TilemapRenderers so placement is exact even with multiple layers
    private Bounds GetMapBounds(GameObject mapRoot)
    {
        var rends = mapRoot.GetComponentsInChildren<TilemapRenderer>();
        if (rends.Length == 0)
        {
            Debug.LogError("MapManager: No TilemapRenderer found in map prefab/instance.");
            return new Bounds(mapRoot.transform.position, Vector3.one);
        }

        Bounds b = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++)
            b.Encapsulate(rends[i].bounds);
        return b;
    }
}
