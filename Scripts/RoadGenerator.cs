using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject turn;
    public GameObject turnLeft;
    public GameObject turnURight;
    public GameObject turnULeft;
    public GameObject levelUp;
    public Transform endPoint;
    private GameObject instantiated;
    public bool isInstantiated;
    private int roadType = 0;
    private int selectedRoad = 0;
    public static int lastRoadType;
    public static int roadCounter;
    public static int lastUTurn;

    /* Road types : 
     * Right 1
     * Left 2
     * RightU 3
     * LeftU 4
     * LevelUp 5
     */
    void Start()
    {
        isInstantiated = false;
        SelectRoad();
    }

    private void FixedUpdate()
    {
        if (isInstantiated == false)
        {
            StartCoroutine(StartInstantiate());
        }
    }
    IEnumerator StartInstantiate()
    {
        // oyunun akıcı çalışabilmesi adına her seferinde45 dönüş oluşturulur. kullanıcı her level atladığında 15 dönüş + 1 bitirme noktası daha eklenilir.
        if (roadCounter != 48)
        {
            isInstantiated = true;
        }
        yield return new WaitForEndOfFrame();
        if (roadCounter < 48)
        {
            InstantiateRoad(selectedRoad);
        }
    }

    // ışınlanacak olan yol tipi harita yapısını bozmayacak şekilde seçilir
    void SelectRoad()
    {
        if(roadCounter != 0 && roadCounter %16 == 0)
        {
            selectedRoad = 5;
            return;
        }
        if(lastRoadType == 0)
        {
            selectedRoad = 1;
        }
        if(lastRoadType == 1 )
        {
            
            roadType = Random.Range(1, 3);
            if (roadType == 1)
            {
                selectedRoad = 2;
            }   
            if(roadType == 2)
            {
                if (lastUTurn !=4 && roadCounter % 16 != 0)
                {
                    selectedRoad = 4;
                }
                else
                {
                    selectedRoad = 2;
                }
            }
        }
        if (lastRoadType == 2)
        {
            roadType = Random.Range(1, 3);
            if (roadType == 1)
            {
                selectedRoad = 1;
            }
            if (roadType ==2)
            {
                if (lastUTurn != 3 && roadCounter % 16 != 0)
                {
                    selectedRoad = 3;
                }
                else
                {
                    selectedRoad = 1;
                }
            }
        }
        if (lastRoadType == 3)
        {
            selectedRoad = 2;
        }
        if (lastRoadType == 4)
        {
            selectedRoad = 1;
        }

    }

    // seçilen yol tipi kalan en son yolun sonuna eklenir
    void InstantiateRoad(int road)
    {
        if (road == 1)
        {
            instantiated = Instantiate(turn);
            instantiated.transform.position = endPoint.transform.position;
            instantiated.transform.rotation = endPoint.transform.rotation;
            lastRoadType = 1;

        }
        if (road == 2)
        {
            instantiated = Instantiate(turnLeft);
            instantiated.transform.position = endPoint.transform.position;
            instantiated.transform.rotation = endPoint.transform.rotation;
            lastRoadType = 2;
        }
        if (road == 3)
        {
            instantiated = Instantiate(turnURight);
            instantiated.transform.position = endPoint.transform.position;
            instantiated.transform.rotation = endPoint.transform.rotation;
            lastRoadType = 3;
            lastUTurn = 3;
        }
        if (road == 4)
        {
            instantiated = Instantiate(turnULeft);
            instantiated.transform.position = endPoint.transform.position;
            instantiated.transform.rotation = endPoint.transform.rotation;
            lastRoadType = 4;
            lastUTurn = 4;
        }
        if (road == 5)
        {
            instantiated = Instantiate(levelUp);
            instantiated.transform.position = endPoint.transform.position;
            instantiated.transform.rotation = endPoint.transform.rotation;
            lastRoadType = 2;
        }
        roadCounter++;
        Destroy(GetComponent<RoadGenerator>());
    }
}
