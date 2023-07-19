using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    private string scoreFormat = "Score: {0}";
    public TMP_Text score;
    private int currentPoint;
    public 
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoint()
    {
        currentPoint++;
        score.text = string.Format(scoreFormat, currentPoint);
    }
}
