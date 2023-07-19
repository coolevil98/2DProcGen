using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && Input.GetKey(KeyCode.LeftShift)!=true)
        {
            Vector3 whereTo=KdTree.instance.RandomRoom();;
            //col.gameObject.transform.position=KdTree.instance.RandomRoom();
            col.gameObject.transform.position=new Vector3(whereTo.x, whereTo.y, -2);
            Destroy(gameObject);
        }
    }
}
