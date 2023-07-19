using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour
{


    //currently will just reload same scene again
    public void LoadNewArea()
    {
        SceneManager.LoadScene("SampleScene");
    }

     public void OnTriggerEnter2D(Collider2D col)
     {
        GameObject isPlayer = col.gameObject;
        if(isPlayer.tag=="Player")
        {
            LoadNewArea();
        }
    }
    //reload the scene again
}
