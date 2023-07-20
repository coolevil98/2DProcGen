using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WebKdTree : MonoBehaviour
{
    public static WebKdTree instance;
    #region Variables
    [Header("Size Values of Dungeon")]
    [Range(25, 1000)]
    [Tooltip("The X axis length of the entire dungeon. 1000 x 1000 is not recommended due to framerate drops")]
    public int dungeonXAxis;
    [Range(25, 1000)]
    [Tooltip("The Y axis length of the entire dungeon. 1000 x 1000 is not recommended due to framerate drops")]
    public int dungeonYAxis;
    //might need to look into scene management beyond COMP3151 to improve this
    //look at unloading objects when not seen by camera
    [Header("Size Values of Rooms")]
    [Tooltip("Minimum size of area in which the room can spawn. Rooms spawned in these areas will be 2 smaller than the area length. Note this should be not bigger than a half of the max axis size and not less than 5")]
    public int minAxisSize;
    [Tooltip("Max size of area in which the room can spawn. Rooms themself will be two sizes smaller than max size. Note this should not be smaller than min axis size. The max size should be less than half of the smallest dungeon axis.")]
    public int maxAxisSize;

    [Header("Odds")]
    [Tooltip("The minimum amount of items a room can have generated as long as it is bigger than minSizeForItemsToSpawn.")]
    public int amountOfItemsPerRoomMin;
    [Tooltip("The max amount of items a room can have generated")]
    public int amountOfItemsPerRoomMax;
    [Tooltip("This method is used for preventing rooms of certain size not to have items. So if it is set to 10, rooms of size 9 will not have items generate")]
    public int minSizeForItemsToSpawn;
    [Tooltip("The minimum amount of traps a room can have generated as long as it is bigger than minSizeForTrapsToSpawn. Must be at least 1")]
    public int amountOfTrapsPerRoomMin;
    [Tooltip("The max amount of traps a room can have generated")]
    public int amountOfTrapsPerRoomMax;
    [Tooltip("This method is used for preventing rooms of certain size not to have traps. So if it is set to 10, rooms of size 9 will not have items generate")]
    //[SerializeField]
    public int minSizeForTrapsToSpawn;
    [Tooltip("The higher this is, the less likely rooms will divide again long as the size is smaller than max axis size. Must be at least 1")]
    public int chanceOfRoomDivideAgain;

    [Header("Spawned")]
    [Tooltip("Ground object prefab")]
    public GameObject square;
    [Tooltip("Player prefab")]
    public GameObject player;
    [Tooltip("Stairs to next floor prefab")]
    public GameObject stairs;
    private int tracker = 0;
    private int playerRoom;
    [Tooltip("Items that can be spawned. List length should be equal to OddsForItemsToSpawn")]
    public List <GameObject> itemsToSpawn;
    [Tooltip("Odds for each item that can be spawn. List length should be equal to itemsToSpawn")]
    public List<int> OddsForItemsToSpawn;
    [Tooltip("Traps that can be spawned. List length should be equal to OddsForTrapsToSpawn")]
    public List <GameObject> trapsToSpawn;
    [Tooltip("Odds for each trap that can be spawn. List length should be equal to trapsToSpawn")]
    public List<int> OddsForTrapsToSpawn;
    //might need to add checker to ensure these are equal

    [Header("Rooms")]
    [Tooltip("List of all the rooms centre points. This should not be modified")]
    public List<Vector3> Rooms = new List<Vector3>();

    [Header("Cameras")]
    [Tooltip("The Camera used by the minimap")]
    public Camera miniMap;



    #endregion
    // Start is called before the first frame update

    void Awake()
    {
        instance=this;
    }
    void Start()
    {
        dungeonXAxis = WebVersion.instance.dungeonXAxis;
        dungeonYAxis = WebVersion.instance.dungeonYAxis;


        if (minSizeForItemsToSpawn<=0)
        {
            minSizeForItemsToSpawn = 1;
        }
        if (minSizeForTrapsToSpawn <= 0)
        {
            minSizeForTrapsToSpawn = 1;
        }
        DivideRoom(dungeonXAxis, dungeonYAxis, 0, 0, 1);
        Paths(0);
        SetPlayerPos();
        SetMiniMap();
    }

    public bool DivideAgain(int axisSize)
    {
        if (axisSize > maxAxisSize)
        {
            return true;
        }


        int divide = Random.Range(0, chanceOfRoomDivideAgain);
        if (divide == 0 && axisSize > minAxisSize * 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DivideRoom(int topX, int topY, int bottomX, int bottomY, int currentAxis)
    {
        int xLength = topX - bottomX;
        int yLength = topY - bottomY;
        int roomX = Random.Range(bottomX, topX + 1);
        if (roomX - bottomY < minAxisSize)
        {
            roomX = topX - (xLength / 2);
        }
        int roomY = Random.Range(bottomY, topY + 1);
        if (roomY - bottomY < minAxisSize)
        {
            roomY = topY - (yLength / 2);
        }
        int axisLength;
        if (currentAxis == 1)
        {
            axisLength = topX - bottomX;
        }
        else
        {
            axisLength = topY - bottomY;

        }

        if (DivideAgain(axisLength)) //change the 2 to current size
        {
            //is cut down the y axis on a x point
            if (currentAxis == 1)
            {
                DivideRoom(topX, topY, roomX, bottomY, 2);
                DivideRoom(roomX, topY, bottomX, bottomY, 2);
            }
            //is cut down the x axis on a y point
            if (currentAxis == 2)
            {
                DivideRoom(topX, topY, bottomX, roomY, 1);
                DivideRoom(topX, roomY, bottomX, bottomY, 1);
            }
        }
        else
        {
            if (currentAxis == 1)
            {
                SetRoomSize(topX, topY, roomX, bottomY);
                SetRoomSize(roomX, topY, bottomX, bottomY);
            }
            if (currentAxis == 2)
            {
                SetRoomSize(topX, topY, bottomX, roomY);
                SetRoomSize(topX, roomY, bottomX, bottomY);
            }
        }

        //switch (DivideAgain(axisLength))
        //{
        //    case true:
        //        switch (currentAxis)
        //        {
        //            case 1:
        //                DivideRoom(topX, topY, roomX, bottomY, 2);
        //                DivideRoom(roomX, topY, bottomX, bottomY, 2);
        //                break;
        //            case 2:
        //                DivideRoom(topX, topY, bottomX, roomY, 1);
        //                DivideRoom(topX, roomY, bottomX, bottomY, 1);
        //                break;
        //        }
        //        break;
        //    default:
        //        switch (currentAxis)
        //        {
        //            case 1:
        //                SetRoomSize(topX, topY, roomX, bottomY);
        //                SetRoomSize(roomX, topY, bottomX, bottomY);
        //                break;
        //            case 2:
        //                SetRoomSize(topX, topY, bottomX, roomY);
        //                SetRoomSize(topX, roomY, bottomX, bottomY);
        //                break;
        //        }
        //        break;
        //}
    }

    public void SetRoomSize(int topX, int topY, int bottomX, int bottomY)
    {
        int xLength = topX - bottomX;
        int yLength = topY - bottomY;

        if (minAxisSize <= xLength && minAxisSize <= yLength)
        {
            int roomX = Random.Range(minAxisSize, xLength);
            if (roomX < xLength / 2)
            {
                roomX = xLength / 2;
                if(roomX>maxAxisSize)
                {
                    roomX=maxAxisSize;
                }
            }
            int roomY = Random.Range(minAxisSize, yLength);
            if (roomY < yLength / 2)
            {
                roomY = yLength / 2;
                if(roomY>maxAxisSize)
                {
                    roomY=maxAxisSize;
                }
            }
            MakeRoom(topX, topY, topX - roomX, topY - roomY);
            AddToListOfRooms(topX, topY, topX - roomX, topY - roomY);
        }
    }

    public void MakeRoom(int topX, int topY, int bottomX, int bottomY)
    {
        //for (int i = bottomX + 1; i < topX - 1; i++)
        //{
        //    for (int j = bottomY + 1; j < topY - 1; j++)
        //    {
        //        Instantiate(square, new Vector3(i, j, 0), Quaternion.identity);
        //        yield return null;
        //    }
        //}
        var room = Instantiate(square, new Vector3((topX-(((float)topX-(float)bottomX)/2f)-0.5f), (topY-(((float)topY-(float)bottomY)/2f) - 0.5f), 0), Quaternion.identity);
        room.transform.localScale = new Vector3((topX - bottomX) - 2, (topY - bottomY) - 2, 1);

        //before adding to list maybe make each room a empty game object that holds all the object in a room
        //https://forum.unity.com/threads/list-of-lists-in-inspector.512085/
        //for list of list which might be used to store all values in rooms

        //need to go through each grid and spawn it
        if(itemsToSpawn.Count > 0) StartCoroutine(SpawnItems(topX - 1, topY - 1, bottomX + 1, bottomY + 1));
        if(trapsToSpawn.Count > 0) StartCoroutine(SpawnTrap(topX - 1, topY - 1, bottomX + 1, bottomY + 1));
    }

    public IEnumerator SpawnItems(int topX, int topY, int bottomX, int bottomY)
    {
        int itemsLeft=Random.Range(amountOfItemsPerRoomMin, amountOfItemsPerRoomMax+1);
        int spawnSpace=(topX-bottomX)*(topY-bottomY);
        if(itemsLeft>spawnSpace/ minSizeForItemsToSpawn)
        {
            itemsLeft = spawnSpace / minSizeForItemsToSpawn;
        }

        for (int i = bottomX; i < topX; i++)
        {
            for (int j = bottomY; j < topY; j++)
            {
                if(Random.Range(0, spawnSpace)<= itemsLeft && itemsLeft!=0)
                {
                    Instantiate(itemsToSpawn[ItemSelection()], new Vector3(i, j, -1), Quaternion.identity);
                    itemsLeft--;
                }
                spawnSpace--;
            }
        }
        yield return null;
    }

    public int ItemSelection()
    {
        int itemOdds=0;
        if (itemsToSpawn.Count() <= 1)
        {
            return 0;
        }
        for (int i=0; i< OddsForItemsToSpawn.Count(); i++)
        {
            itemOdds += OddsForItemsToSpawn[i];
        }
        int whichNumber = Random.Range(0, itemOdds + 1);
        for (int i = 0; i < OddsForItemsToSpawn.Count(); i++)
        {
            itemOdds += OddsForItemsToSpawn[i];
            if(itemOdds<= whichNumber)
            {
                return i;
            }
        }
        int whichItem = Random.Range(0, itemsToSpawn.Count());
        return whichItem;
    }
    public IEnumerator SpawnTrap(int topX, int topY, int bottomX, int bottomY)
    {
        int trapsLeft=Random.Range(amountOfTrapsPerRoomMin, amountOfTrapsPerRoomMax+1);
        int spawnSpace=(topX-bottomX)*(topY-bottomY);
        if(trapsLeft>spawnSpace/ minSizeForTrapsToSpawn)
        {
            trapsLeft = spawnSpace / minSizeForTrapsToSpawn;
        }

        for (int i = bottomX; i < topX; i++)
        {
            for (int j = bottomY; j < topY; j++)
            {
                if(Random.Range(0, spawnSpace)<= trapsLeft && trapsLeft!=0)
                {
                    Instantiate(trapsToSpawn[TrapSelection()], new Vector3(i, j, -1), Quaternion.identity);
                    trapsLeft--;
                }
                spawnSpace--;
            }
        }
        yield return null;
    }

      public int TrapSelection()
    {
        int trapOdds=0;
        if(trapsToSpawn.Count()<=1)
        {
            return 0;
        }
        for(int i=0; i< OddsForTrapsToSpawn.Count(); i++)
        {
            trapOdds += OddsForTrapsToSpawn[i];
        }
        int whichNumber = Random.Range(0, trapOdds + 1);
        for (int i = 0; i < OddsForTrapsToSpawn.Count(); i++)
        {
            trapOdds += OddsForTrapsToSpawn[i];
            if(trapOdds<= whichNumber)
            {
                return i;
            }
        }
        int whichItem = Random.Range(0, itemsToSpawn.Count());
        return whichItem;
    }
    public void SetMiniMap()
    {
        miniMap.transform.position=new Vector3(dungeonXAxis/2,dungeonYAxis/2, -2);
        if(dungeonXAxis>dungeonYAxis)
        {
            miniMap.GetComponent<Camera>().orthographicSize=dungeonXAxis/2;
        }
        else
        {
            miniMap.GetComponent<Camera>().orthographicSize=dungeonYAxis/2;
        }
    }

    public void AddToListOfRooms(int topX, int topY, int bottomX, int bottomY)
    {
        int tempx = (topX - bottomX) / 2;
        int tempy = (topY - bottomY) / 2;
        Rooms.Add(new Vector3(bottomX + tempx, bottomY + tempy, 0));
    }
    public void Paths(int current)
    {
        if (current + 1 < Rooms.Count())
        {
            Vector3 point1 = Rooms[current];
            Vector3 point2 = Rooms[current + 1];
            int point1x = (int)point1.x;
            int point2x = (int)point2.x;
            if (point1x > point2x)
            {
                int temp = point2x;
                point2x = point1x;
                point1x = temp;
            }
            int point1y = (int)point1.y;
            int point2y = (int)point2.y;
            if (point1y > point2y)
            {
                int temp = point2y;
                point2y = point1y;
                point1y = temp;
            }
            for (int i = point1x; i <= point2x; i++)
            {
                Instantiate(square, new Vector3(i, point1y, 0), Quaternion.identity); //this
            }

            for (int i = point1y; i <= point2y; i++)
            {
                int whichX = point1x;
                if (point1y != point1.y)
                {
                    whichX = (int)point1.x;
                }
                    //get a checker to make sure that the system starts the next point from the non-start point
                    Instantiate(square, new Vector3(whichX, i, 0), Quaternion.identity); //and this cause the bug due to the value they take in
            }
            Paths(current + 1);
        }
    }

    public void SetPlayerPos()
    {
        //access mid point and place in middle of one room
        int whichRoom = Random.Range(0, Rooms.Count());
        playerRoom = whichRoom;
        Instantiate(player, new Vector3(Rooms[whichRoom].x, Rooms[whichRoom].y, -1), Quaternion.identity);
        SetStairsPos();
    }
    public void SetStairsPos()
    {
        //access mid point and place in middle of one room
        int whichRoom = Random.Range(0, Rooms.Count());
        if(whichRoom==playerRoom)
        {
            if(playerRoom==0)
            {
                whichRoom = Random.Range(1, Rooms.Count());
            }
            else if(playerRoom== Rooms.Count())
            {
                whichRoom = Random.Range(0, Rooms.Count()-1);
            }
            else
            {
                whichRoom = Random.Range(0, playerRoom);
            }
        }
        Instantiate(stairs, new Vector3(Rooms[whichRoom].x, Rooms[whichRoom].y, -1), Quaternion.identity);
    }

    public Vector3 RandomRoom()
    {
        int room= Random.Range(0, Rooms.Count());
        return Rooms[room];
    }
}
