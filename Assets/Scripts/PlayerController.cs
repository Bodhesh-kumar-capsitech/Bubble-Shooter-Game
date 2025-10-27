using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private float groundrange = -15.6f;
    public bool iscollided = false;
    public bool ismoving = false;
    private SpawnBubble bubble;
    public bool outrange = false;
    private Vector2 aimDirection;
    private float bubblespeed = 20.0f;
    private float leftrange = -15.2f;
    private float rightrange = 18.0f;
    private Rigidbody2D rb;
    private bool iscollidedwithtopwal = false;
    public bool isprojectilecolided = false;
    public bool isdestroygameobject = false;
    public static PlayerController instance;
    private float bubbledownspacing = 3.4f;
    private BubbleGroupSpawner _Spawnbubble;
    private BubbleCollisionHandler _CollisionHandler;
    private bool isprefabcollidewithrowprefab = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        isprojectilecolided = false;
        isdestroygameobject = false;
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {


        if (transform.position.y > 40)
        {
            outrange = true;
        }


        if (transform.position.y < groundrange )
        {
            Destroy(gameObject);
            AudioPlayer.player.OnDestroySound();

        }

        if (iscollided == false)
        {
            Moveobject();

        }
        if(transform.position.x < leftrange || transform.position.x > rightrange)
        {
            Destroy(gameObject);
            AudioPlayer.player.OnDestroySound();

        }

        if (transform.position.y < -8.4f && !gameObject.CompareTag("ActiveBubble"))
        {
            GameOver.gameOver.OnGameover();
        }

    }


    void Moveobject()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ismoving = true;

        }

    }

}
