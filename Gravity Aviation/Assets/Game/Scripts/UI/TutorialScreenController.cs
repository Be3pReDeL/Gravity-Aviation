using UnityEngine;
using System.Collections;

public class TutorialScreenController : UIController {
    public override void Start(){
        if(PlayerPrefs.GetInt("Tutorial", 0) == 1)
            Destroy(gameObject);

        base.Start();

        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime(){
        yield return new WaitForSeconds(5f);

        PlayerPrefs.SetInt("Tutorial", 1);

        Destroy(gameObject);
    }
}
