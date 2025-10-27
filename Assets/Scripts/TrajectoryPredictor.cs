using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform shootPoint;
    public LayerMask collisionLayerMask;
    public int maxBounces = 2;
    public float maxPredictionLength = 80f;
    private Vector3 mousePressPosition;
    private bool isDragging = false;

    private Camera mainCamera;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        mainCamera = Camera.main;

        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void Update()
    {
            if (Input.GetMouseButton(0))
            {
                lineRenderer.enabled = true;
                
                Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                Vector2 direction = (mouseWorld - shootPoint.position).normalized;
                PredictPath(shootPoint.position, direction);
        }
            else
            {
                lineRenderer.enabled = false;
            }
       
    }

    void PredictPath(Vector2 startPosition, Vector2 startDirection)
    {

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPosition);

        Vector2 currentPosition = startPosition;
        Vector2 currentDirection = startDirection.normalized;
        float remainingLength = maxPredictionLength;

        for (int i = 0; i <= maxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, remainingLength, collisionLayerMask);

            if (hit.collider != null)
            {
                // Add bounce point
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                remainingLength -= Vector2.Distance(currentPosition, hit.point);
                currentPosition = hit.point;

                // Reflect direction on hit
                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
            }
            else
            {
                // Draw straight line until end
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + currentDirection * remainingLength);
                break;
            }
        }
    }
}
