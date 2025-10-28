using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleThrow : MonoBehaviour
{
    [SerializeField]private float speed = 380f;
    private Rigidbody2D rb;
    private SpawnBubble _bubble;
    private BubbleGroupSpawner _bubbleGroup;
    private BubbleCollisionHandler _bubbleCollisionHandler;
    [SerializeField] private Vector2 mousePressDownPos;
    private Vector2 mouseReleasePos;
    private bool isShoot = false;
    public bool isvalidprefab = false;
    public bool isshoot = false;
    public static BubbleThrow instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }

    private void Start()
    {
        _bubble = SpawnBubble.instance;
        _bubbleGroup = BubbleGroupSpawner.instance;
        _bubbleCollisionHandler = BubbleCollisionHandler.Instance;
        isshoot = false;
    }

    private void Update()
    {
        

        if (transform.position.x > 0f && transform.position.x < 4.0f && transform.position.y < -8.0f && transform.position.y > -16.0f)
        {
            isvalidprefab = true;
        }
        else
        {
            isvalidprefab = false;
        }

        if (Input.GetMouseButtonDown(0) && isvalidprefab)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if (worldMousePos.y < -12.0f)
            {
                return;
            }

            Shoot();
        }
       
    }

    private void Shoot()
    {

        if (isShoot) return; 
        isShoot = true;
        isshoot = true;

        Vector3 mousePos = Input.mousePosition;

        mousePos.z = Mathf.Abs(Camera.main.transform.position.z); 
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

            worldMousePos.z = 0f;

        Vector2 direction = (worldMousePos - transform.position).normalized;

        if (direction.y < 0)
            direction.y = Mathf.Abs(direction.y);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        AudioPlayer.player.OnThrowSound();
        rb.AddForce(direction * speed , ForceMode2D.Impulse);
        _bubbleGroup.OnPlayerShot();
    }

}
