using System.Collections;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public static RocketController Instance { get; private set; }
    public float moveSpeed = 10f;
    public float inertia = 0.95f;
    public float tiltSpeed = 5f;
    public float smoothness = 0.1f; // Плавность движения
    public float touchSensitivity = 0.2f;
    public GameObject hitEffectPrefab, coinsEffectPrefab, fuelEffectPrefab, cloudsEffectPrefab;

    private Vector3 targetSpeed = Vector3.zero;
    private Vector3 currentSpeed = Vector3.zero;
    private Vector3 lastTouchPosition = Vector3.zero;
    private bool isMoving = false;

    [SerializeField] private Animator animator;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));

            if (Vector3.Distance(lastTouchPosition, touchPosition) > touchSensitivity)
            {
                Vector3 newPosition = new Vector3(touchPosition.x, transform.position.y, transform.position.z);
                targetSpeed = (newPosition - transform.position).normalized * moveSpeed;
                lastTouchPosition = touchPosition;
                isMoving = true;
            }
        }
        else
        {
            if (isMoving)
            {
                targetSpeed *= inertia;
                isMoving = targetSpeed.magnitude > 0.01f;
            }
        }

        // Плавное изменение скорости
        currentSpeed = Vector3.Lerp(currentSpeed, targetSpeed, smoothness);
        transform.position += currentSpeed * Time.deltaTime;

        ApplyTilt();
    }

    private void ApplyTilt()
    {
        float tiltAngle = isMoving ? -30 * Mathf.Sign(currentSpeed.x) : 0;
        Quaternion targetRotation = Quaternion.Euler(0, 0, tiltAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bird") || collision.gameObject.CompareTag("Asteroid"))
        {
            GameManager.Instance.LoseLife();

            Instantiate(hitEffectPrefab, collision.transform.position, Quaternion.identity);
        }

        if (collision.gameObject.CompareTag("Clouds"))
        {
            Instantiate(cloudsEffectPrefab, collision.transform.position, Quaternion.identity);
        }

        if (collision.gameObject.CompareTag("Fuel"))
        {
            Instantiate(fuelEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject); 

            StartCoroutine(SetBlueFire());
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            GameManager.Instance.CollectCoin();
            Instantiate(coinsEffectPrefab, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject); // Уничтожаем монету после сбора
        }
    }

    private IEnumerator SetBlueFire(){
        animator.SetBool("Blue", true);

        moveSpeed = 3f;

        yield return new WaitForSeconds(10f);

        animator.SetBool("Blue", false);

        moveSpeed = 1f;
    }
}
