using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PathFindingManager path;
    private Rigidbody2D rigid2D;
    public float maxSpeed = 2f;


    public Vector2 nextPos;
    public Vector2 finalPos;
    public Vector2 currentDest;
    public int currentIndex = 1;

    public float speed = 2f;
    
    public bool canMove;

    private Vector2Int startPos;
    
    private void Awake()
    {
        path = GetComponent<PathFindingManager>();
        rigid2D = GetComponent<Rigidbody2D>();
        var pos = MapManager.Instance.ChangeCellPos(transform.position);
        startPos = new Vector2Int(pos.x,pos.y);
        
        StartCoroutine(PathFinding());
    }

    private void OnEnable()
    {
        MapManager.Instance.mapChangeEvent += MapChange;
        path.PathFinding(startPos,MapManager.Instance.heartRoomPos);
    }

    private void OnDisable()
    {
        MapManager.Instance.mapChangeEvent -= MapChange;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid2D.velocity = new Vector2(0,rigid2D.velocity.y);
        }
    }

    private void MapChange()
    {
        StartCoroutine(PathFinding());
    }

    private IEnumerator PathFinding()
    {
        path.FinalNodeList.Clear();
        yield return null;
        path.PathFinding(startPos,MapManager.Instance.heartRoomPos);
    }
    
    

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        var finalNodeList = path.FinalNodeList;
        if (finalNodeList.Count > 0)
        {
            if ((currentDest - new Vector2(transform.position.x,transform.position.y)).magnitude < 0.1f)
            {
                if (currentIndex >= finalNodeList.Count - 1)
                {
                    rigid2D.position = currentDest;
                    finalNodeList.Clear();
                    canMove = false;
                    return;
                }
                currentIndex++;
            }
            else
            {
                Vector2 toTarget = (currentDest - new Vector2(transform.position.x, transform.position.y)).normalized;
                rigid2D.MovePosition(rigid2D.position+toTarget*speed*Time.fixedDeltaTime);
            }

            if (finalNodeList.Count <= currentIndex)
            {
                currentIndex = finalNodeList.Count - 1;
            }
            currentDest = new Vector2(finalNodeList[currentIndex].x + 0.5f, finalNodeList[currentIndex].y + 0.5f);
        }
        
//        
//        float h = Input.GetAxisRaw("Horizontal");
//        rigid2D.AddForce(Vector2.right * h,ForceMode2D.Impulse);
//
//        if (rigid2D.velocity.x > maxSpeed)
//        {
//            rigid2D.velocity = new Vector2(maxSpeed,rigid2D.velocity.y);
//        }
//        else if (rigid2D.velocity.x < -maxSpeed)
//        {
//            rigid2D.velocity = new Vector2(-maxSpeed,rigid2D.velocity.y);
//        }
    }
}
