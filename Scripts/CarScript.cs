using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CarScript : MonoBehaviour
{
    public bool canTurn = false;
    private bool isDestroyed = false;
    private bool isCreated = false;
    private bool isItTurnU = false;
    private bool isAccerlated = false;
    public GameObject roadObject;
    private GameObject barrel;
    public GameObject carObject;
    public GameObject playerPanel;
    public GameObject retryPanel;
    public GameObject driftEffect;
    public GameObject levelUpEffect;
    public Text scoreText;
    private FixedJoint fixJoint;
    private TurnPoint turnScript;
    private Rigidbody carRB;
    private LineRenderer lineRenderer;
    private float carSpeed = 35;
    private float timer = 0;
    private float rotationConstant = 90f;
    private int score = 0;

    private void Start()    
    {
        playerPanel.SetActive(true);
        retryPanel.SetActive(false);
        Time.timeScale = 0;
        carRB = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        scoreText.text = "0";
        driftEffect.SetActive(false);
        levelUpEffect.SetActive(false);
    }

    private void Update()
    {
        carRB.velocity = carObject.transform.forward * carSpeed;
        if (Input.GetKeyDown(KeyCode.E))
        {
            RoadGenerator.roadCounter -= 16;
        }

        if (Input.GetMouseButton(0) && canTurn == true)
        {
            // ilk basışta normal dönüp bir süre döndükten sonra drifte başlıyor.
            timer += Time.deltaTime;
            Turn();
            if (timer >= 0.1f)
            {
                StartDrift();
            }
            if(timer>= 0.5f)
            {
                // Uzun dönüşlerde çok dönüp hakimiyet kaybetmemesi için bir ayarlama
                rotationConstant = 10;
            }

        }

        else if (canTurn == true)
        {
            StopDrift();
            timer = 0;
        }
        else if (canTurn == false)
        {
            timer = 0;
            SetCar(roadObject.transform);
            StopDrift();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            canTurn = false;
            roadObject = other.gameObject;
            Destroy(other.gameObject, 5);
        }
        if (other.gameObject.CompareTag("Turn"))
        {
            //her dönüşte joint'in bağlanacağı çember eşleniyor.
            isItTurnU = false;
            canTurn = true;
            turnScript = other.gameObject.GetComponentInParent<TurnPoint>();
            Destroy(other.gameObject, 5);
            Destroy(other.gameObject.GetComponentInParent<TurnPoint>(),5);
            Destroy(turnScript.ReturnBarrel(),5);
            barrel = turnScript.ReturnBarrel();

        }
        if(other.gameObject.CompareTag("TurnU"))
        {
            isItTurnU = true;
            canTurn = true;
            turnScript = other.gameObject.GetComponentInParent<TurnPoint>();
            Destroy(other.gameObject, 5);
            Destroy(other.gameObject.GetComponentInParent<TurnPoint>(), 5);
            Destroy(turnScript.ReturnBarrel(),5);
            barrel = turnScript.ReturnBarrel();
        }
        if (other.gameObject.CompareTag("Point"))
        {
            score++;
            scoreText.text = score.ToString();
        }
        if (other.gameObject.CompareTag("Die"))
        {
            Die();
        }
        if (other.gameObject.CompareTag("LevelUp"))
        {
            if(isAccerlated == false)
            {
                //her levelde yeni dönüşlerin oluşturulması adına counter azaltılır.
                levelUpEffect.SetActive(true);
                RoadGenerator.roadCounter -= 15;
                isAccerlated = true;
                carSpeed = 70;
                Destroy(other.gameObject, 5);
                return;
            }
            if(isAccerlated == true)
            {
                levelUpEffect.SetActive(false);
                isAccerlated = false;
                carSpeed = 35;
                Destroy(other.gameObject, 5);
                return;
            }

        }

    }


    void Turn()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, carObject.transform.position);
        lineRenderer.SetPosition(1, barrel.transform.position);
        //dönüşün yönünü öğrenip dönme açısını ayarlama
        if (turnScript.ReturnTurnDirection() == 1)
        {
            carObject.transform.Rotate(0, rotationConstant * Time.deltaTime, 0);
        }
        if(turnScript.ReturnTurnDirection() == -1)
        {
            carObject.transform.Rotate(0, -rotationConstant * Time.deltaTime, 0);
        } 
       
    }
    void StartDrift()
    {
        driftEffect.SetActive(true);
        if (isItTurnU == true)
        {
            // u dönüşünde drift yaparken hakimiyeti sağlamak adına bir ayarlama
            rotationConstant = 25;
        }
        else
        {
            rotationConstant = 65;
        }
        if (isCreated == false)
        {
            // her seferinde yeni bir joint bileşeni oluşturulup seçili dönme noktasına bağlanır
            isCreated = true;
            isDestroyed = false;
            gameObject.AddComponent<FixedJoint>();
            fixJoint = GetComponent<FixedJoint>();
            fixJoint.connectedBody = barrel.GetComponent<Rigidbody>();
        }
        
    }

    void StopDrift()
    {
        driftEffect.SetActive(false);
        lineRenderer.enabled = false;
        if (isDestroyed == false)
        {
            // dönme noktasına bağlı dönüş hareketinin durması için joint kaldırılır
            isDestroyed = true;
            isCreated = false;
            Destroy(GetComponent<FixedJoint>());
        }
        
    }


    public void SetCar(Transform road)
    {
        //dönüşlerden çıktıktan sonra araç yola paralel hale getirilir
        rotationConstant = 90;
        carObject.transform.rotation = Quaternion.Lerp(carObject.transform.rotation, road.rotation,2.75f * Time.deltaTime);
    }

    void Die()
    {
        retryPanel.SetActive(true);
        score = 0;
        RoadGenerator.roadCounter = 0;
        Time.timeScale = 0;

    }
    public void PlayerStart()
    {
        Time.timeScale = 1;
        playerPanel.SetActive(false);
    }
    public void Reborn()
    {
        retryPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    
}