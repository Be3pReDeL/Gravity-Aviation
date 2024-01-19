using UnityEngine;

public class Asteroid : Obstacle {
    [SerializeField] private float _sizeDelta;

    private void OnEnable(){
        float currentSizeDelta = Random.Range(-_sizeDelta, _sizeDelta);

        transform.localScale = new Vector3(transform.localScale.x + currentSizeDelta, transform.localScale.y + currentSizeDelta, 0f);
    }
}
