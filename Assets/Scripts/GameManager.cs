using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float openDoorTime;
    public float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= openDoorTime)
        {
            //만약에 저장된 씰포스 리스트가 있다면 그 문은 닫힌문!
            var sealPosList = MapManager.Instance.sealPosList;
            
            //아닐경우에
        }
    }
}
