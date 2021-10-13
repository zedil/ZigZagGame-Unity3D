using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator { get {return GetComponent<Animator>(); } }

    public float moveSpeed = 2;
    public Transform origin;
    bool lookingRight = true;
    public Text scoreText, hScoreText;
    int score, hScore;

    public ParticleSystem effectPrefab;

    delegate void TurnDelegate();
    TurnDelegate turn;

    //findobjectoftype<GameManager> içine aldığı tipe göre döndürür
    //GameManageri bulup gameManager olarak getiriyor.
    GameManager gameManager {get {return FindObjectOfType<GameManager>(); }}
    // Start is called before the first frame update
    void Start()
    {
        //oyuncunun yaptığı puanı alıyoruz
        hScore = PlayerPrefs.GetInt("myhscore");

        //ekrana yazdırma
        hScoreText.text = hScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStarted)
        {
            //tüm aksiyonları triggerdan yaptık.
            animator.SetTrigger("GameStarted");

            //hareketi oluşturuyoruz
            //transform.position += transform.forward * moveSpeed *Time.deltaTime;
            //yukarıdaki kodla aynı işlev
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);



            //editördeyken ve mobildeyken hangisini kullanacağına bu şekilde karar verebiliriz.
            #if UNITY_EDITOR
                turn = TurnUsingKeyboard;
            #endif
            #if UNITY_ANDROID
                turn = TurnUsingFinger;
            #endif

            turn();
            //Turn();

            CheckFalling();
        }
        
    }

    private void CheckFalling()
    {
        //aşağı doğru ışın yaparak düşmeyi anlayabiliriz.
        //transform.down da yapabilriiz
        //Altta bir şey varsa demek => (Physics.Raycast(origin.position, Vector3.down
        if (!Physics.Raycast(origin.position, Vector3.down))
        {
            animator.SetTrigger("faling");
            gameManager.RestartGame();
            
        }
    }

    Vector2 effectPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("crystal"))
        {
            //crystal spawn olduğunda ve oyuncu ona değdiğinde pozisoynu al
            effectPos = other.transform.position;
            MakeScore();
            other.gameObject.SetActive(false);
            MakeEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //bastığı her block 2 saniye sonra yok edilmeli.
        Destroy(collision.gameObject, 2f);
    }

    private void MakeEffect()
    {
        //prefab ve kristal konumu
        var effect = Instantiate(effectPrefab,effectPos,Quaternion.identity);
        
        //efekti yok et
        Destroy(effect.gameObject, 1f);
    }

    private void MakeScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score > hScore)
        { 

            hScore = score;
            hScoreText.text = hScore.ToString();

            //kaydetme
            PlayerPrefs.SetInt("myhscore", hScore);

        }
    }

    private void TurnUsingKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Turn();
        }
    }

    //mobil cihazlar için, parmakla ilgili olaylar
    private void TurnUsingFinger()
    {
        float firstTouchX = 0;
        float lastTouchX = 0;

        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
            case TouchPhase.Began:
                firstTouchX = Input.GetTouch(0).position.x;
                break;
            case TouchPhase.Moved:
                lastTouchX = Input.GetTouch(0).position.x;
                break;
            case TouchPhase.Ended:
                if (Mathf.Abs(lastTouchX-firstTouchX) > 50) Turn();
                break;
            }
            
        }


    }

    private void Turn()
    {
        //Space'e bastığımızda olsun istiyoruz.
        if(Input.GetKeyDown(KeyCode.Space))
        {

             //Her turn işleminden sonra oyun bir miktar hızlansın
            moveSpeed += Time.deltaTime *2;

            if (lookingRight)
            {
                //sola dön
                //dönüş için rotate y'de 90 derecelik fark oluyor
                transform.Rotate(new Vector3(0, -90, 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, 90, 0));
            }
            lookingRight = !lookingRight;

        }

    }
}
