using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private Vector3 mousePosition;
    public LayerMask wallLayer;
    public LayerMask landLayer;
    public LayerMask lasss;

    public PathFindingManager path;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePosition,0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] overCol2D = Physics2D.OverlapCircleAll(mousePosition, 0.01f);

            if (overCol2D.Length <= 0)
            {
                return;
            }

            foreach (var col in overCol2D)
            {
                if (1 << col.gameObject.layer == wallLayer)
                {
                    MapManager.Instance.StartEmptyCell(mousePosition);
                    return;
                }
            }
            
            foreach (var col in overCol2D)
            {
                if (1 << col.gameObject.layer == landLayer)
                {
                    var pos = MapManager.Instance.ChangeCellPos(mousePosition);
                    Vector2Int changePos = new Vector2Int(pos.x,pos.y);
                    path.PathFinding(changePos,MapManager.Instance.heartRoomPos);
                    break;
                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] overCol2D = Physics2D.OverlapCircleAll(mousePosition, 0.01f);

            if (overCol2D.Length <= 0)
            {
                return;
            }

            foreach (var col in overCol2D)
            {
                if (1 << col.gameObject.layer == wallLayer)
                {
                    MapManager.Instance.ReadyForEnemy(mousePosition);
                    return;
                }
            }
        }
    }
}
