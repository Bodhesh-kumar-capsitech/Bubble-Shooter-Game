using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public TextMeshProUGUI text;
    public bool isGameActive;
    public static GameOver gameOver;
    public GameObject gameoverpannel;
    public GameObject Scorepannel;
    public GameObject Score;
    public GameObject Restart;
    public GameObject Exit;
    public GameObject Trajectoryline;
    public Button pause;
    public Button play;
    public Button exit;
    public GameObject won;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = this;
        isGameActive = true;
        //text.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousepos = Input.mousePosition;

        if(ScoreManager.Instance.Score >= 3000)
        {
            won.gameObject.SetActive(true);
            BubbleCollisionHandler.Instance.DestroyAllBubble();
            play.gameObject.SetActive(false);
            pause.gameObject.SetActive(false);
            Trajectoryline.SetActive(false);
            Restart.SetActive(true);
            Exit.SetActive(true);
        }
       
    }

    public void OnGameover()
    {
        BubbleCollisionHandler.Instance.DestroyAllBubble();
        isGameActive = false;
        text.gameObject.SetActive(true);
        gameoverpannel.SetActive(true);
        Restart.SetActive(true);   
        Exit.SetActive(true);
        play.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        Trajectoryline.SetActive(false);
        Scorepannel.SetActive(false);
        Score.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        pause.gameObject.SetActive(false);
        play.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
        //Trajectoryline.SetActive(false);
    }

    public void Play()
    {
        Time.timeScale = 1.0f;
        pause.gameObject.SetActive(true);
        play.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        //Trajectoryline.SetActive(true);

    }
}
