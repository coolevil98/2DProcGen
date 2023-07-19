using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //in here might want to check what the next value is so it cannot go over the ground onto the wall
       /* if(input.GetAxis("Horizontal"))
        {

        }*/
        
        if (Input.GetKey(KeyCode.A)&& timer<=0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+Vector3.left,Vector2.left,0.4f);
            if (hit.collider != null)
            {
            transform.Translate(-1, 0, 0);
            }
            timer=0.2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                timer = 0.01f;
            }
        }
        if (Input.GetKey(KeyCode.D)&& timer<=0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+Vector3.right,Vector2.right,0.4f);
            if (hit.collider != null)
            {
            transform.Translate(1,0,0);
            }
            timer=0.2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                timer = 0.01f;
            }
        }
        if (Input.GetKey(KeyCode.W)&& timer<=0)
        {
             RaycastHit2D hit = Physics2D.Raycast(transform.position+Vector3.up,Vector2.up,0.4f);
            if (hit.collider != null)
            {
            transform.Translate(0,1,0);
            }
            timer=0.2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                timer = 0.01f;
            }
        }
        if (Input.GetKey(KeyCode.S) && timer<=0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+Vector3.down,Vector2.down,0.4f);
            if (hit.collider != null)
            {
            transform.Translate(0,-1,0);
            }
            timer=0.2f;
            if(Input.GetKey(KeyCode.LeftShift))
            {
                timer = 0.01f;
            }


        }
        if(timer>0)
        {
            timer-=Time.deltaTime;
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            int ChangeLayer = LayerMask.NameToLayer("MiniMapSee");
            col.gameObject.layer = ChangeLayer;
        }
        if (col.gameObject.tag == "Item" && Input.GetKey(KeyCode.LeftShift) != true)
        {
            CoinManager.instance.addPoint();
            Destroy(col.gameObject);
        }
    }
}
