using UnityEngine;

public class Obstacle : MonoBehaviour {
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _deactivationDistance = 10;

    private ObjectPool _pool;

    private void Awake(){
        _pool = GameObject.Find("Scene").GetComponent<ObjectPool>();
    }

    protected void Update() {
        transform.Translate(0, -_speed * Time.deltaTime, 0);

        if (transform.position.y < -_deactivationDistance) {
            _pool.ReturnToPool(gameObject, gameObject);
        }
    }
}
