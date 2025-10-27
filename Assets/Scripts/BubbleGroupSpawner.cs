using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleGroupSpawner : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    private int columns = 9;
    private int rows = 22;
    private float bubbleradius = 1.7f;
    Rigidbody2D rb;
    public int missedShotsCount;
    public bool bubbleDestroyed = false;
    public static BubbleGroupSpawner instance;
    public bool iscollided = false;
    public bool spawnbubblecollided;
    private SpawnBubble _bubble;
    private BubbleCollisionHandler _collisioncheck;
    //use for bubble storage
    private GameObject[,] bubblegrid;
    private SpawnBubble newspawn;
    private float spacing;
    [SerializeField] public Transform spawnParent;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spacing = 3.4f;
        bubblegrid = new GameObject[rows,columns];
        instance = this;
        Generategrid();
        _bubble = SpawnBubble.instance;
        newspawn = SpawnBubble.newspawn;
        _collisioncheck = BubbleCollisionHandler.Instance;
    }


    private void Generategrid()
    {
        float totalwidth = (columns - 1) * spacing + 1.0f;
        float totalheight = (rows - 1) * spacing;

        //Vector2 startposition = new Vector2(topSpawnPosition.x - totalwidth / 2, topSpawnPosition.y);
        int length = 0;
        for (int x = 0; x < rows; x++)
        {
            for(int y = 0; y < columns; y++)
            {
                float extra = 0f;
                if((x & 1) == 0)
                {
                    extra = 2.2f;
                }
                float xpos = y * spacing - (totalwidth/2) + extra;
                float ypos = x * spacing + (totalheight/2);

                int randomIndex = Random.Range(0, bubblePrefabs.Length);
                GameObject prefab = bubblePrefabs[randomIndex];

                Vector2 spawnpos = new Vector2(xpos, ypos);

                GameObject newbubble = Instantiate(prefab,spawnpos,Quaternion.identity, spawnParent);
                bubblegrid[x, y] = newbubble;

                length++;
              

            }

        }

        //Debug.Log("length: " + length);
    }


     public void OnPlayerShot()
    {

        if (bubbleDestroyed == true)
        {
            missedShotsCount = 0;

        }
        else
        {
            missedShotsCount++;
            Debug.Log("Missedshots count is:" + missedShotsCount);

        }

        if (missedShotsCount == 3)
        {
            Invoke(nameof(MoveGridDown),1.4f);
            //MoveGridDown();
            missedShotsCount = 0;
        }

    }


    public void MoveGridDown()
    {
            Debug.Log("Moving grid down!");
            foreach (var bubble in GameObject.FindGameObjectsWithTag("Bubble"))
            {
                bubble.transform.position -= new Vector3(0, spacing, 0);

            }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject p1 = gameObject;   
        GameObject p2 = collision.gameObject;
        Rigidbody2D rb1 = p1.GetComponent<Rigidbody2D>();   
        Rigidbody2D rb2 = p2.GetComponent<Rigidbody2D>();   
        if (collision.gameObject.CompareTag(this.gameObject.tag) || collision.gameObject.CompareTag("Top Wall"))
        {
            rb1.linearVelocity = Vector2.zero;
            rb1.angularVelocity = 0f;
            rb2.linearVelocity = Vector2.zero;
            rb2.angularVelocity = 0f;
            rb1.bodyType = RigidbodyType2D.Kinematic;
            rb2.bodyType = RigidbodyType2D.Kinematic;
        }


        if (collision.gameObject == null && rb.linearVelocity == Vector2.zero)
        {
            Destroy(gameObject);
            AudioPlayer.player.OnDestroySound();


        }

    }

}

