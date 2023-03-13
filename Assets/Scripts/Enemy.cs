using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PathFindingManager path;
    private Rigidbody2D rigid2D;
    public float maxSpeed = 2f;
    public int damage = 5;


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
        path.bottomLeft = new Vector2Int(-MapManager.Instance.col/2,-MapManager.Instance.row/2);
        path.topRight = new Vector2Int(MapManager.Instance.col/2,MapManager.Instance.row/2);
        
        MapManager.Instance.mapChangeEvent += MapChange;
        path.PathFinding(startPos,MapManager.Instance.heartRoomPos);
    }

    private void OnDisable()
    {
        MapManager.Instance.mapChangeEvent -= MapChange;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            other.gameObject.GetComponent<Player>().Damaged(damage);
        }
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
