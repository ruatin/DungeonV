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
            currentTime -= openDoorTime;
            //만약에 저장된 씰포스 리스트가 있다면 그 문은 닫힌문!
            var sealPosList = MapManager.Instance.sealPosList;
            var minePosList = MapManager.Instance.minePosList;
            
            //모든 지뢰리스트를 가져와서 지뢰중에서
            //랜덤하게 1개를 오픈하는데 그 지뢰랑 봉인 좌표랑 대조해서 같다면
            //오픈을하는데 봉인된 지뢰로 표시해서 오픈한다.
            //같지 않을 경우에 그냥 오픈한다.

            var randomCount = Random.Range(0, minePosList.Count - 1);
            var pos = minePosList[randomCount];

            if (sealPosList.Count <= 0)
            {
                //그냥 지뢰 오픈한다 openTheMine
                Debug.Log("openTheMine");
                return;
            }
            
            if (sealPosList.Contains(pos))
            {
                //오픈한다 봉인된 지뢰를 openTheSealMine
                Debug.Log("openTheSealMine");
                //오픈하면 마인 리스트에서 빼야한다.
            }
            else
            {
                //그냥 지뢰 오픈한다 openTheMine
                Debug.Log("openTheMine");
            }
            

        }
    }
}
