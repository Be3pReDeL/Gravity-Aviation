using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float deltaX = 5f;

    private ObjectPool pool;
    private float timer;

    private void Awake() 
    {
        pool = GetComponent<ObjectPool>();
    }

    private void Update() 
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval) 
        {
            GameObject randomPrefab = pool.GetRandomPrefab();
            GameObject obstacle = pool.GetFromPool(randomPrefab);

            obstacle.transform.position = GetSpawnPosition();
            
            timer = 0;
        }
    }

    private Vector3 GetSpawnPosition() 
    {
        float xPosition = Random.Range(-deltaX, deltaX);
        return new Vector3(xPosition, transform.position.y, 0);
    }
}
