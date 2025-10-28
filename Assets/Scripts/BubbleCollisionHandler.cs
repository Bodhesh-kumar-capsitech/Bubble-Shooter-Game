using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleCollisionHandler : MonoBehaviour
{
    private int wallCollisionCount = 0;
    public bool hasCollided = false;
    private const int MaxWallCollision = 4;
    private PlayerController playerController;
    private BubbleGroupSpawner spawn;
    private Rigidbody2D rb;
    public static BubbleCollisionHandler Instance;
    public int score = 0;
    private ScoreManager scoreManager;
    private int pointsPerBubble = 10;
    public GameObject bubbleffect;
    private float connectionradius = 2.0f;
    //public ParticleSystem effect;
    private GameObject tempg;
    private bool isbubblecolided = false;

    private void Start()
    {
        spawn = BubbleGroupSpawner.instance;
        scoreManager = ScoreManager.Instance;
        playerController = PlayerController.instance;
        rb = GetComponent<Rigidbody2D>();
        Instance = this;
        //Debug.Log("Score is:" + score);
        //effect = GetComponent<ParticleSystem>();
        DestroyFloatingBubbles();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided && (collision.gameObject.CompareTag("Bubble") ||
            collision.gameObject.CompareTag("Top Wall")))
        {
            //BubbleGroupSpawner.instance.OnPlayerShot();
            tempg = gameObject;
            hasCollided = true;
            gameObject.tag = "Bubble";

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }

            if (SpawnBubble.instance != null)
            {
                SpawnBubble.instance.OnBubbleCollided();
                isbubblecolided = true;
            }


            if (collision.gameObject.CompareTag("Bubble"))
            {
                Color myColor = GetComponent<SpriteRenderer>().color;
                Color otherColor = collision.gameObject.GetComponent<SpriteRenderer>().color;

                if (myColor == otherColor)
                {
                    HandleClusterDestruction();
                }
            }
        }
       
        if (collision.gameObject.CompareTag("Left Wall"))
        {
            wallCollisionCount++;
            Debug.Log($"Wall collision count: {wallCollisionCount}");

            if (wallCollisionCount > MaxWallCollision)
            {
                Destroy(gameObject);

                if (!hasCollided && SpawnBubble.instance != null)
                {
                    SpawnBubble.instance.OnBubbleCollided();
                }
            }
        }

        playerController.iscollided = true;
        playerController.ismoving = false;

        if (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Top Wall"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.mass = 0f;
            rb.Sleep();
            rb.bodyType = RigidbodyType2D.Static;
            playerController.isprojectilecolided = true;
        }

    }

    private void HandleClusterDestruction()
    {
        List<GameObject> cluster = FindConnectedBubbles(gameObject);

        Debug.Log("Cluster count is:" + cluster.Count);

        if (cluster.Count >= 3)
        {
            foreach (var b in cluster)
            {
                var effectPos = b.transform.position;   
                var effectObject =  Instantiate(bubbleffect,effectPos,Quaternion.identity);
                effectObject.GetComponent<ParticleSystem>().Play();
                Destroy(b);
                AudioPlayer.player.OnDestroySound();
                Debug.Log("Cluster count is:" + cluster.Count);

                spawn.bubbleDestroyed = true;
                spawn.missedShotsCount = 0;


            }


            scoreManager.AddScore(cluster.Count * pointsPerBubble);
        }
        else
        {
            spawn.bubbleDestroyed = false;
        }

        DestroyFloatingBubbles();
    }

    private List<GameObject> FindConnectedBubbles(GameObject start)
    {
        List<GameObject> connected = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        Color targetColor = start.GetComponent<SpriteRenderer>().color;

        queue.Enqueue(start);
        connected.Add(start);


        while (queue.Count > 0)
        {
            GameObject current = queue.Dequeue();
            Collider2D[] hits = Physics2D.OverlapCircleAll(current.transform.position, connectionradius);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Bubble") && !connected.Contains(hit.gameObject))
                {
                    Color c = hit.GetComponent<SpriteRenderer>().color;
                    if (c == targetColor)
                    {
                        connected.Add(hit.gameObject);
                        queue.Enqueue(hit.gameObject);
                    }
                }
            }
        }
        return connected;
    }

    private void DestroyFloatingBubbles()
    {
        GameObject[] allBubbles = GameObject.FindGameObjectsWithTag("Bubble");
        if (allBubbles == null || allBubbles.Length == 0) return;

        HashSet<GameObject> visited = new HashSet<GameObject>();

        string[] wallTags = new string[] { "Top Wall", "Left Wall", "Right Wall" };

        foreach (var bubble in allBubbles)
        {
            if (bubble == null) continue;
            if (visited.Contains(bubble)) continue;

            List<GameObject> component = new List<GameObject>();
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(bubble);
            visited.Add(bubble);
            component.Add(bubble);

            while (queue.Count > 0)
            {
                GameObject current = queue.Dequeue();
                if (current == null) continue;

                Collider2D[] hits = Physics2D.OverlapCircleAll(current.transform.position, connectionradius);
                foreach (var hit in hits)
                {
                    if (hit == null) continue;
                    GameObject neighbor = hit.gameObject;
                    if (neighbor == null) continue;
                    if (!neighbor.CompareTag("Bubble")) continue;
                    if (visited.Contains(neighbor)) continue;

                    visited.Add(neighbor);
                    component.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            bool connectedToWall = false;
            foreach (var member in component)
            {
                if (member == null) continue;

                Collider2D[] hits = Physics2D.OverlapCircleAll(member.transform.position, connectionradius);
                foreach (var hit in hits)
                {
                    if (hit == null) continue;
                    GameObject obj = hit.gameObject;
                    if (obj == null) continue;

                    foreach (var wt in wallTags)
                    {
                        if (obj.CompareTag(wt))
                        {
                            connectedToWall = true;
                            break;
                        }
                    }
                    if (connectedToWall) break;
                }
                if (connectedToWall) break;
            }

            if (!connectedToWall)
            {
                foreach (var member in component)
                {
                    if (member != null)
                    {
                        var effectPos = member.transform.position;
                        var effectObject = Instantiate(bubbleffect, effectPos, Quaternion.identity);
                        effectObject.GetComponent<ParticleSystem>().Play();
                        var totalmember = component.Count;
                        Destroy(member);
                        AudioPlayer.player.OnDestroySound();
                        //scoreManager.AddScore( totalmember * pointsPerBubble);
                    }
                }
            }
        }

    }

    public void DestroyAllBubble()
    {
        GameObject[] allBubbles = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (var item in allBubbles)
        {
            Destroy(item);
        }

        GameObject spawnbubble = GameObject.FindGameObjectWithTag("ActiveBubble");
        Destroy(spawnbubble);

    }

    void OnDrawGizmos()

    {

        if (tempg != null)

        {

            Gizmos.color = UnityEngine.Color.yellow;

            Gizmos.DrawWireSphere(tempg.transform.position, 1.8f);

        }

    }

   



}