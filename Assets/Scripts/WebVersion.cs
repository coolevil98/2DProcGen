using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WebVersion : MonoBehaviour
{
    public static WebVersion instance;
    public GameObject WebVersionUI;
    public GameObject CreatedSpawner;
    public Camera tempCamera;
    #region Variables
    public int dungeonXAxis;
    public int dungeonYAxis;

    public int minAxisSize;

    public int maxAxisSize;


    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //set active again. Will not distory between
    //every time the stairs are touched it will be visable and player will start it again
    public void LoadGeneration()
    {
        WebVersionUI.SetActive(false);
        Instantiate(CreatedSpawner, new Vector3(0,0,0), Quaternion.identity);
        tempCamera.enabled = false;
    }

    public void DungeonXAxis(float value)
    {
        dungeonXAxis = (int)value;
    }
    public void DungeonYAxis(float value)
    {
        dungeonYAxis = (int)value;
    }

}
