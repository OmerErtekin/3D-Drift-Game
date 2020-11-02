using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    public GameObject barrel;
    private GameObject carObject;
    private float distance;

    private void Start()
    {
        carObject = GameObject.Find("Car");


    }
    void FixedUpdate()
    {
        distance = Vector3.Distance(carObject.transform.position,barrel.transform.position);
        if ((this.gameObject.CompareTag("Right") || this.gameObject.CompareTag("RightU")) && distance < 50)
        {
            //dönüşün yönüne göre açısal hareketi sağlayacak parça döndürülür. Noktadan uzaklaştıkça arabanın çizgisel hızı artacağından bu dengeyi yakalamak adına dönüş yavaşlatılır.
            if (distance < 10)
            {
                barrel.transform.Rotate(0, 20 / distance, 0);
            }
            else
            {
                barrel.transform.Rotate(0, 40 / distance, 0);
            }
        }
        if((this.gameObject.CompareTag("Left") || this.gameObject.CompareTag("LeftU")) && distance < 50)
        {
            if (distance < 10)
            {
                barrel.transform.Rotate(0, -20 / distance, 0);
            }
            else
            {
                barrel.transform.Rotate(0,- 40 / distance, 0);
            }
        }
    }
    

    public GameObject ReturnBarrel()
    {
        //CarScriptte aracın drift anında bağlanacağı varili döndüren fonskiyon.
        return barrel;
    }

    public int ReturnTurnDirection()
    {
        // arabanın ne yöne döneceğini belirten fonksiyon
        if (gameObject.CompareTag("Right") || gameObject.CompareTag("RightU"))
        {
            return 1;
        }
        else if (gameObject.CompareTag("Left") || gameObject.CompareTag("LeftU"))
        {
            return -1;
        }
        else return 0;
    }
}
