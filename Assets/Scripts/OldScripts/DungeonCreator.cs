using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int roomAmountMin;
    public int roomAmountMax;
    private int roomAmount;

    public int roomSizeMin;
    public int roomSizeMax;

    public int DungeonSizeX;
    public int DungeonSizeY;

    public float chanceOfExtension;
    public int ExtensionSizeMin;
    public int ExtensionSizeMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //creates rooms by calling other functions
    public void RoomCreation()
    {
        //start with room amount which will then be used by a loop to go through other methods
        int rooms=RoomAmount();
        for(int i=0; i<=rooms; i++)
        {

        }
    }

    //sets the amount of rooms
    private int RoomAmount()
    {
        roomAmount = Random.Range(roomAmountMin, roomAmountMax+1);
        return roomAmount;
    }

    //need to add another room generation for amount of 1x1 rooms which can be used to create more interesting paths

    //returns the x and y length of the room
    private int[] RoomSize()
    {
        int roomSizeX= Random.Range(roomSizeMin, roomSizeMax+1);
        int roomSizeY= Random.Range(roomSizeMin, roomSizeMax+1);
         int[] roomSize = new int[]{roomSizeX,roomSizeY};
         return roomSize;
    }


    //determine if rooms will have an extension
    private bool IsExtended()
    {        
        if(chanceOfExtension>100)
        {
            chanceOfExtension=100;
        }
        if(chanceOfExtension<0)
        {
            chanceOfExtension=0;
        }
        if(chanceOfExtension<=Random.Range(0, 101))
        {
            return true;
        }
        return false;
    }

    //returns 1, 2,3 or 4 depending on the axis and side
    private int WhichAxis()
    {
        int whichAxis =Random.Range(1, 5);
        return whichAxis;
    }

    //returns the start and end point of extension
    //this method has a chance of making dead ends which could create some interesting gameplay
    private int[] ExtensionSize(int min, int max)
    {
        int firstValue =Random.Range(min, max+1);
        int secondValue =Random.Range(min, max+1);
        int[] ExtensionValues = new int[]{firstValue,secondValue};
        return ExtensionValues;
    }

    //how long the extension of the room is
    private int ExtensionAmount()
    {
        int extensionSize = Random.Range(ExtensionSizeMin, ExtensionSizeMax+1);
        return extensionSize;
    }

    //find where room needs to go
    //this will require checks in order to ensure it does not collide (or do I want to let them collide. Could do both) with other rooms or walls
    //dependin on how I implement I might need to change from int to void
    private int RoomPos()
    {
        return 1;
    }

    //set once all the rooms are made //will need some method of knowing where rooms are
    private void SetStairPosition()
    {

    }
    private void SetPlayerPosition()
    {

    }
}
