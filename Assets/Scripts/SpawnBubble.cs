using UnityEngine;

public class SpawnBubble : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public Vector2 spawnPosition = new Vector2(0.9f, -12.0f);

    public float initialDelay = 0.0f;
    public float spawnDelayAfterCollision = 0.5f;
    public bool spawnbubblecolided = true;
    public GameObject currentBubble;
    public static SpawnBubble instance;
    private bool canSpawnNewBubble = true;
    private BubbleGroupSpawner spawn;
    public static SpawnBubble newspawn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        spawn = BubbleGroupSpawner.instance;
        newspawn = this;
        Invoke(nameof(SpawnRandomBubble), initialDelay);
    }

    public void SpawnRandomBubble()
    {
        while (GameOver.gameOver.isGameActive == true)
        {
            if (!canSpawnNewBubble) return;

            int randomIndex = Random.Range(0, bubblePrefabs.Length);
            currentBubble = Instantiate(bubblePrefabs[randomIndex], spawnPosition, Quaternion.identity);
            currentBubble.tag = "ActiveBubble";
            Rigidbody2D rb = currentBubble.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                //rb.mass = 5.0f;
            }

            BubbleCollisionHandler collisionHandler = currentBubble.GetComponent<BubbleCollisionHandler>();
            if (collisionHandler == null)
            {
                collisionHandler = currentBubble.AddComponent<BubbleCollisionHandler>();
            }

            canSpawnNewBubble = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            spawnbubblecolided = true;
        }

        else
        {
            spawnbubblecolided = false;
        }
    }

    public void OnBubbleCollided()
    {
        canSpawnNewBubble = true;
        Invoke(nameof(SpawnRandomBubble), spawnDelayAfterCollision);
    }

    public void ForceSpawnNewBubble()
    {
        canSpawnNewBubble = true;
        SpawnRandomBubble();
    }

}


