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
    [Header("DungeonAxis")]
    public int dungeonXAxis;
    public int dungeonYAxis;
    [Header("Dungeon room size")]
    public int minAxisSize;
    public Slider minAxisSizeSlider;
    public int maxAxisSize;
    public Slider maxAxisSizeSlider;


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
    //in all of this the numbers will need to be updated in realtime for the user
    
    //set active again. Will not distory between
    //every time the stairs are touched it will be visble and player will start it again (the ui)
    public void LoadGeneration()
    {
        WebVersionUI.SetActive(false);
        Instantiate(CreatedSpawner, new Vector3(0,0,0), Quaternion.identity);
        tempCamera.enabled = false;
    }

    //x and y axises of the dungeon
    public void DungeonXAxis(float value)
    {
        dungeonXAxis = (int)value;
        if (dungeonXAxis < dungeonYAxis)
        {
            if (maxAxisSizeSlider.maxValue>dungeonXAxis/2)
            {
                maxAxisSizeSlider.maxValue = dungeonXAxis/2;
                if (maxAxisSizeSlider.maxValue / 2 < minAxisSizeSlider.maxValue)
                {
                    minAxisSizeSlider.maxValue = maxAxisSizeSlider.maxValue / 2;
                    if (minAxisSizeSlider.maxValue < minAxisSize)
                    {
                        minAxisSize = (int)minAxisSizeSlider.maxValue;
                    }
                }
            }
        }
        if (dungeonXAxis > dungeonYAxis)
        {
            maxAxisSizeSlider.maxValue = dungeonYAxis / 2;
            minAxisSizeSlider.maxValue = maxAxisSizeSlider.value / 2;
            if (minAxisSizeSlider.maxValue < minAxisSize)
            {
                minAxisSize = (int)minAxisSizeSlider.maxValue;
            }
        }
    }
    public void DungeonYAxis(float value)
    {
        dungeonYAxis = (int)value;
        if (dungeonYAxis < dungeonXAxis)
        {
            if (maxAxisSizeSlider.maxValue > dungeonYAxis / 2)
            {
                maxAxisSizeSlider.maxValue = dungeonYAxis / 2;
                if(maxAxisSizeSlider.maxValue/2< minAxisSizeSlider.maxValue)
                {
                    minAxisSizeSlider.maxValue = maxAxisSizeSlider.maxValue / 2;
                    if (minAxisSizeSlider.maxValue < minAxisSize)
                    {
                        minAxisSize = (int)minAxisSizeSlider.maxValue;
                    }

                }
            }
        }
        if (dungeonYAxis > dungeonXAxis)
        {
            maxAxisSizeSlider.maxValue = dungeonXAxis / 2;
            minAxisSizeSlider.maxValue = maxAxisSizeSlider.value / 2;
            if (minAxisSizeSlider.maxValue < minAxisSize)
            {
                minAxisSize = (int)minAxisSizeSlider.maxValue;
            }
        }
    }

    public void MinAxisSize(float value)
    {
        minAxisSize= (int)value;
    }
    public void MaxAxisSize(float value)
    {
        maxAxisSize = (int)value;
        if (maxAxisSize / 2 < minAxisSizeSlider.maxValue)
        {
            minAxisSizeSlider.maxValue = maxAxisSize / 2;
            if(minAxisSizeSlider.maxValue<minAxisSize)
            {
                minAxisSize = (int)minAxisSizeSlider.maxValue;
            }
        }
    }


}
