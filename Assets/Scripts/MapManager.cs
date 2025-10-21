// using UnityEngine;
// using System.Collections;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using UnityEngine.Tilemaps;

// public class MapManager : MonoBehaviour
// {
//     [SerializeField] private Camera cam;
//     [SerializeField] private GameObject player;
//     private Collider2D playerCollider;

//     [Header(("Map Objects"))]
//     [SerializeField] private GameObject roomPrefab;
//     private float cellH;
//     [SerializeField] private GameObject currRoom;
//     private Tilemap currTM;
//     private TilemapRenderer currRend;
//     private Vector3 currRoomPos;
//     [SerializeField] private int roomCount = 0;

//     // [Header("Obstacles")]
//     // [SerializeField] private GameObject[] obstaclePrefabs;
//     // [SerializeField] private int obstacleGap; // Gap between obstacles
//     // [SerializeField] private int minObstaclesPerChunk;
//     // [SerializeField] private int maxObstaclesPerChunk;
//     // [SerializeField] private int _setCount = 0; // Number of times a chunk of obstacles have been spawned
//     // private ArrayList _currObstacles = new ArrayList(); // purely obstacle-tagged objects in chunk
//     // private ArrayList _nextObstacles = new ArrayList();
//     // private ArrayList _currObjects = new ArrayList(); // any objects in chunk
//     // private ArrayList _nextObjects = new ArrayList();

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         // currRoom set to starting room in editor
//         playerCollider = player.GetComponent<Collider2D>();
//         currRend = currRoom.transform.GetChild(0).GetComponent<TilemapRenderer>(); // get first layer's renderer
//         currRoomPos = currRoom.transform.position; // set current map
//         currTM = currRoom.GetComponentInChildren<Tilemap>();
//         cellH = currTM.layoutGrid.cellSize.y;
//     }

//     void AddRoomAbove()
//     {
//         currTM = currRoom.GetComponentInChildren<Tilemap>();
//         var currEdgeY = currTM.CellToWorld(new Vector3Int(currTM.cellBounds.xMin, currTM.cellBounds.yMax, 0)).y;
//         var newRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
//         var newTM = newRoom.GetComponentInChildren<Tilemap>();
//         var nextHeight = newTM.cellBounds.size.y * cellH;

//         var newRend = newRoom.GetComponentInChildren<TilemapRenderer>();
//         float centerOffsetY = newRend.bounds.center.y - newRoom.transform.position.y;
//         float targetCenterY = currEdgeY + nextHeight * 0.5f;
//         newRoom.transform.position = new Vector3(0f, targetCenterY - centerOffsetY, 0f);

//         currRoom = newRoom;
//         currRend = newRend;
//         roomCount++;

//         // GenerateObstacles(savedPos); // call when we actually have obstacles
//     }
    
//     void AddRoomRight()
//     {
//         currTM = currRoom.GetComponentInChildren<Tilemap>();
//         var currEdgeX = currTM.CellToWorld(new Vector3Int(currTM.cellBounds.xMax, currTM.cellBounds.yMin, 0)).x;
//         var newRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
//         var newTM = newRoom.GetComponentInChildren<Tilemap>();
//         var nextWidth = newTM.cellBounds.size.x * cellH;

//         var newRend = newRoom.GetComponentInChildren<TilemapRenderer>();
//         float centerOffsetX = newRend.bounds.center.x - newRoom.transform.position.x;
//         float targetCenterX = currEdgeX + nextWidth * 0.5f;
//         newRoom.transform.position = new Vector3(targetCenterX - centerOffsetX, 0f, 0f);

//         currRoom = newRoom;
//         currRend = newRend;
//         roomCount++;

//         // GenerateObstacles(savedPos); // call when we actually have obstacles
//     }
    
//     // private void GenerateObstacles(Vector3 savedPos)
//     // {
//     //     _currObstacles = _nextObstacles;

//     //     int setsToSpawn = Random.Range(minObstaclesPerChunk, maxObstaclesPerChunk + 1); // Number of obstacles to spawn in this chunk
//     //     _nextObstacles = new ArrayList();
//     //     for (int i = 0; i < setsToSpawn; i++)
//     //     {
//     //         _setCount++;

//     //         int randInt = Random.Range(0, obstaclePrefabs.Length);
//     //         int obstacleOffset = maxObstaclesPerChunk / setsToSpawn * obstacleGap * (i); // Offset to evenly space out obstacles depending on how many there are
//     //         GameObject setObject = Instantiate(obstaclePrefabs[randInt], savedPos + new Vector3(0f, _currMapRenderer.bounds.size.y + obstacleOffset), Quaternion.identity);
//     //         // tracking and collecting obstacles for publisher/subscriber system while glitch coin is active
//     //         for (int j = 0; j < setObject.transform.childCount; j++)
//     //         {
//     //             GameObject child = setObject.transform.GetChild(j).gameObject;
//     //             if (child.CompareTag("Obstacle"))
//     //             {
//     //                 _nextObstacles.Add(child);
//     //             }
//     //             _nextObjects.Add(child);
//     //         }
//     //     }
//     // }

//     // Update is called once per frame
//     void Update()
//     {
//         // check for collision of player with edge of rooms
//         if (playerCollider.bounds.max.y >= currRend.bounds.max.y)
//         {
//             AddRoomAbove();
//         }
//         else if (playerCollider.bounds.max.x >= currRend.bounds.max.x)
//         {
//             AddRoomRight();
//         }
//     }
// }

using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject startRoom;

    private Collider2D playerCol;

    // stuff for room grid
    private Dictionary<Vector2Int, GameObject> rooms = new();
    private Vector2 roomSize;         // world units (width, height)
    private Vector2 originBL;         // world bottom-left of starting room, key (0,0)
    private Vector2 rendCenterOffset; // bounds.center - transform.position
    private Vector2Int currKey;

    private int roomCount;

    // one-shot gates for each edge of current room
    private bool wasBelowTop     = true;
    private bool wasLeftOfRight  = true;
    private bool wasAboveBottom  = true;
    private bool wasRightOfLeft  = true;

    void Start()
    {
        playerCol = player.GetComponent<Collider2D>();

        var startTM = startRoom.GetComponentInChildren<Tilemap>();
        var grid = startTM.layoutGrid;
        float cellW = grid.cellSize.x; // cell width in grid
        float cellH = grid.cellSize.y; // cell height in grid.... should be same tbh
        roomSize = new Vector2(startTM.cellBounds.size.x * cellW, startTM.cellBounds.size.y * cellH);

        var rend = startRoom.GetComponentInChildren<TilemapRenderer>();
        rendCenterOffset = (Vector2)rend.bounds.center - (Vector2)startRoom.transform.position;

        originBL = new Vector2(rend.bounds.min.x, rend.bounds.min.y);
        rooms[new Vector2Int(0, 0)] = startRoom;

        currKey = WorldToKey(playerCol.bounds.center);
    }

    void Update()
    {
        currKey = WorldToKey(playerCol.bounds.center); // key for the room the player is currently in
        var currRoom = rooms[currKey];
        var currRend = currRoom.GetComponentInChildren<TilemapRenderer>();

        // bounds of current room
        float topY    = currRend.bounds.max.y;
        float rightX  = currRend.bounds.max.x;
        float bottomY = currRend.bounds.min.y;
        float leftX   = currRend.bounds.min.x;

        // player colliders
        float pTop    = playerCol.bounds.max.y;
        float pRight  = playerCol.bounds.max.x;
        float pBottom = playerCol.bounds.min.y;
        float pLeft   = playerCol.bounds.min.x;

        // idempotent edge crossing checks for new room creation
        bool nowAboveTop     = pTop    >= topY;
        bool nowRightOfEdge  = pRight  >= rightX;
        bool nowBelowBottom  = pBottom <= bottomY;
        bool nowLeftOfEdge   = pLeft   <= leftX;

        if (wasBelowTop    && nowAboveTop)    TrySpawn(currKey + Vector2Int.up);
        if (wasLeftOfRight && nowRightOfEdge) TrySpawn(currKey + Vector2Int.right);
        if (wasAboveBottom && nowBelowBottom) TrySpawn(currKey + Vector2Int.down);
        if (wasRightOfLeft && nowLeftOfEdge)  TrySpawn(currKey + Vector2Int.left);

        // reset edge crossing checks for next update
        wasBelowTop    = !nowAboveTop;
        wasLeftOfRight = !nowRightOfEdge;
        wasAboveBottom = !nowBelowBottom;
        wasRightOfLeft = !nowLeftOfEdge;
    }

    Vector2Int WorldToKey(Vector2 worldPos) // coordinate system for storing room identities
    {
        float dx = worldPos.x - originBL.x;
        float dy = worldPos.y - originBL.y;
        int kx = Mathf.FloorToInt(dx / roomSize.x);
        int ky = Mathf.FloorToInt(dy / roomSize.y);
        return new Vector2Int(kx, ky);
    }

    // Vector3 KeyToWorldPos(Vector2Int key) // get position of room given the key, in case i need it for special rooms??
    // {
    //     Vector2 bl = originBL + new Vector2(key.x * roomSize.x, key.y * roomSize.y);
    //     Vector2 center = bl + roomSize * 0.5f;
    //     Vector2 targetPos = center - rendCenterOffset;
    //     return new Vector3(targetPos.x, targetPos.y, 0f);
    // }

    void TrySpawn(Vector2Int key)
    {
        if (!rooms.ContainsKey(key))
        {
            var newRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

            var newRend = newRoom.GetComponentInChildren<TilemapRenderer>();
            var offset = (Vector2)newRend.bounds.center - (Vector2)newRoom.transform.position; // double-check offset in case we change room size and I forget

            Vector2 bl = originBL + new Vector2(key.x * roomSize.x, key.y * roomSize.y); // bottom left of new room
            Vector2 center = bl + roomSize * 0.5f;
            Vector2 targetPos = center - offset;
            newRoom.transform.position = new Vector3(targetPos.x, targetPos.y, 0f);

            rooms[key] = newRoom;
        }
    }
}
