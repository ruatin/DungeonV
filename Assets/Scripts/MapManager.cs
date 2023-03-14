using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    
    public int row = 9;
    public int col = 9;

    public int[][] tileData;

    public GameObject textRoot;
    public GameObject roomSlot;

    public Text[][] texts;
    public TileBase[][] landTiles;
    public TileBase[][] brickTiles;

    public int mineCount;

    private bool isDead;

    public Tilemap wallTileMap;
    public Tilemap landTileMap;
    public Tilemap buildingTileMap;
    public Tilemap enemyBuildingTileMap;
    public Tilemap unBreakableTileMap;
    public TileBase brickTile;
    public TileBase unBreakableTile;
    public TileBase landTile;
    public TileBase mineTile;
    public TileBase goldTile;
    public TileBase heartTile;
    public TileBase trapTile;

    public GameObject heartRoom;
    
    private Vector3 offsetTileAnchor;
    
    public Text numberText;
    
    public int canStartCount = 1;

    public bool canOpen;

    public int DungeonPoint
    {
        get => dungeonPoint;
        set => dungeonPoint = value;
    }

    private int dungeonPoint;

    List<Vector3Int> openList = new List<Vector3Int>();
    List<Vector3Int> closeList = new List<Vector3Int>();
    List<Vector3Int> numList = new List<Vector3Int>();
    List<Vector3Int> startPointList = new List<Vector3Int>();
    
    [HideInInspector] public Vector2Int heartRoomPos;

    public delegate void MapChange();
    public event MapChange mapChangeEvent;

    public GameObject enemy;
    
    public List<Vector3Int> sealPosList = new List<Vector3Int>();
    
    private Text startTileText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        
        SetTileMapPos();
        GenerateLand();
        PlaceMine();
        UpdateRoom();
        SelectHeartRoom();
    }

    private void SetTileMapPos()
    {
        var x = row % 2 == 0 ?0.5f : 0f;
        var y = col % 2 == 0 ?0.5f : 0f;
        var anchorX = 0.5f - x;
        var anchorY = 0.5f - y;

        offsetTileAnchor = new Vector3(anchorX,anchorY,0);
        wallTileMap.tileAnchor = new Vector3(x,y,0);
    }
    
    private void GenerateLand()
    {
        landTiles = new TileBase[row][];
        brickTiles = new TileBase[row][];
        tileData = new int[row][];
        
        for (var k = 0; k < row; k++)
        {
            landTiles[k] = new TileBase[col];
            brickTiles[k] = new TileBase[col];
            tileData[k] = new int[col];
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                tileData[i][j] = 0;
                
                landTileMap.SetTile(new Vector3Int(i-row/2,j-col/2,0),landTile);
                landTiles[i][j] = landTileMap.GetTile(new Vector3Int(i-row/2, j-col/2, -0));

                wallTileMap.SetTile(new Vector3Int(i-row/2,j-col/2,0),brickTile);
                brickTiles[i][j] = wallTileMap.GetTile(new Vector3Int(i-row/2, j-col/2, 0));
            }
        }
        
        for (int i = -1; i < row+1; i++)
        {
            for (int j = -1; j < col+1; j++)
            {
                if (i == -1 || j == -1 || i == row || j == col)
                {
                    unBreakableTileMap.SetTile(new Vector3Int(i-row/2,j-col/2,0),unBreakableTile);
                }

            }
        }
    }

    private void PlaceMine()
    {
        for (int i = 0; i < mineCount; i++)
        {
            var a = Random.Range(0, row);
            var b = Random.Range(0, col);

            if (tileData[a][b] == 'm')
            {
                i--;
            }
            else
            {
                tileData[a][b] = 'm';
                CountMine(a, b, tileData);
            }
        }

    }

    private void CountMine(int x, int y ,int[][] arr)
    {
        for (int i = x-1; i < x+2; i++)
        {
            for (int j = y-1; j < y+2; j++)
            {
                if (i < 0 || j < 0  || i == arr.Length || j == arr[i].Length)
                {
                    continue;
                }
                
                arr[i][j] += arr[i][j] != 'm' ? 1 : 0;
            }
        }
        


    }
    
    public List<Vector3Int> minePosList = new List<Vector3Int>();

    private void UpdateRoom()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
                
            {
                if (tileData[i][j] == 'm')
                {
                    var minePos = new Vector3Int(i - row / 2, j - col / 2, 0);
                    minePosList.Add(minePos);
                    enemyBuildingTileMap.SetTile(minePos,mineTile);
                }
                else if(tileData[i][j] > 0)
                {
                    landTileMap.SetTile(new Vector3Int(i-row/2,j-col/2,0),goldTile);
                }
            }
        }
    }




    private void SelectHeartRoom()
    {
        var startPosList = new List<Vector3Int>();
        for (int i = 0; i < row; i +=3)
        {
            for (int j = 0; j < col; j++)
            {
                if(tileData[i][j] == 0)
                {
                    Vector3Int pos = new Vector3Int(i- row / 2,j- col / 2,0);
                    startPosList.Add(pos);
                }
            }
        }

        for (int i = 0; i < canStartCount; i++)
        {
            var randNum = Random.Range(0, startPosList.Count-1);
            var startPos = startPosList[randNum] + new Vector3Int(0,0,0);
            
//            buildingTileMap.SetTile(startPos,heartTile);
            Instantiate(heartRoom, startPos + new Vector3(0.5f,0.5f,0), quaternion.identity, buildingTileMap.transform);
            
            startTileText = Instantiate(numberText,startPos,quaternion.identity,textRoot.transform);
            startTileText.text = "!";
            
            startPointList.Add(startPos);
        }
    }

    public Vector3Int ChangeCellPos(Vector3 pos)
    {
        Vector3Int cellPos = wallTileMap.WorldToCell(pos);
        return cellPos;
    }



    public void ReadyForEnemy(Vector3 pos)
    {
        Vector3Int cellPos = wallTileMap.WorldToCell(pos + offsetTileAnchor);
        if (!wallTileMap.GetTile(cellPos) || !canOpen)
        {
            return;
        }
        
        sealPosList.Add(cellPos);
        //오른쪽 클릭해서 그 타일 위치를 저장하고
        //타일 저장하는 위치를 어디로 하나
    }

    public void StartEmptyCell(Vector3 pos)
    {
        Vector3Int cellPos = wallTileMap.WorldToCell(pos + offsetTileAnchor);
        if (!canOpen)
        {
            foreach (var point in startPointList)
            {
                if (point == cellPos)
                {
                    heartRoomPos = new Vector2Int(point.x,point.y);
                    Destroy(startTileText.gameObject);
                    canOpen = true;
                    break;
                }
            }
        }

        if (!wallTileMap.GetTile(cellPos) || !canOpen)
        {
            return;
        }

        //오픈리스트에 넣는다 처음
        openList.Add(cellPos);
        var x2 = cellPos.x + row / 2;
        var y2= cellPos.y + col / 2;

        var pickValue = tileData[x2][y2];
        wallTileMap.SetTile(cellPos,null);
        mapChangeEvent?.Invoke();
        
        if(pickValue == 'm' && !isDead)
        {
            minePosList.Remove(cellPos);
            Instantiate(enemy, cellPos+new Vector3(0.5f,0.5f,0), quaternion.identity);
        }
        else if (pickValue == 0)
        {
            while(openList.Count > 0)
            {
                var tempList = new List<Vector3Int>();
                foreach (var open in openList)
                {
                    var x = open.x + row / 2;
                    var y= open.y + col / 2;
                    wallTileMap.SetTile(open ,null);
                
                    for (int i = x-1; i < x+2; i++)
                    {
                        for (int j = y-1; j < y+2; j++)
                        {
                            if (i < 0 || j < 0  || i == tileData.Length || j == tileData[i].Length  || (i == x && j == y))
                            {
                                continue;
                            }
                        
                            var noneMinePos = new Vector3(i - row / 2, j - col / 2, 0) +offsetTileAnchor;
                            var noMinePos = wallTileMap.WorldToCell(noneMinePos);
                            var x1 = noMinePos.x + row / 2;
                            var y1= noMinePos.y + col / 2;

                        
                            if (tileData[x1][y1] == 0 && !closeList.Contains(noMinePos) && !openList.Contains(noMinePos) && !tempList.Contains(noMinePos))
                            {
                                //오픈리스트에 넣기
                                tempList.Add(noMinePos);
                            }
                            else if(tileData[x1][y1] != 0 && !numList.Contains(noMinePos))
                            {
                                numList.Add(noMinePos);
                            }
                        }
                    }
                    closeList.Add(open);
                }
                openList.Clear();
                openList = tempList;
            }

            foreach (var num in numList)
            {
                wallTileMap.SetTile(num,null);
                
                Vector3 pos1 = new Vector3(x2-row/2,y2-col/2,0);
                var x = num.x+row/2;
                var y = num.y+col/2;
                
                var numTile = Instantiate(numberText,num,quaternion.identity,textRoot.transform);
                numTile.text = tileData[x][y].ToString();
                
                dungeonPoint += tileData[x][y];
                UiManager.Instance.SetPointText(dungeonPoint);
            }
            
        }
        else
        {
            Vector3 pos1 = new Vector3(x2-row/2,y2-col/2,0);
            var numTile = Instantiate(numberText,pos1,quaternion.identity,textRoot.transform);
            numTile.text = tileData[x2][y2].ToString();

            dungeonPoint += tileData[x2][y2];
            UiManager.Instance.SetPointText(dungeonPoint);
        }

        openList.Clear();
        closeList.Clear();
        numList.Clear();


//        MakeEmpty(pos);
    }

    public void OpenTheMine(Vector3Int cellPos,int index)
    {
        minePosList.RemoveAt(index);
        wallTileMap.SetTile(cellPos,null);
        mapChangeEvent?.Invoke();
        Instantiate(enemy, cellPos+new Vector3(0.5f,0.5f,0), quaternion.identity);
    }

    public void BuildTrap(Vector3 pos)
    {
        Vector3Int cellPos = wallTileMap.WorldToCell(pos + offsetTileAnchor);
        buildingTileMap.SetTile(cellPos,trapTile);
    }

    private void Died()
    {
        isDead = true;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
//                buttons[i][j].image.enabled = false;
            }
        }
    }
}
