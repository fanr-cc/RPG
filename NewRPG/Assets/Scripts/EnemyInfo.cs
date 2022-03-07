using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour {

    private float takeDamageTextUP;
    private Transform enemyTransform;
    private bool isUP=false;
   // Update is called once per frame
	void Update () {
	    if (isUP&& enemyTransform!=null)
	    {
            takeDamageTextUP += 60 * Time.deltaTime;
            transform.position = Camera.main.WorldToScreenPoint(enemyTransform.position) + new Vector3(0, 50 + takeDamageTextUP, 0);
        }
    }

    //动画播放结束后，将TakeDamageText隐藏
    public void SetInfo(Transform trans)
    {
        enemyTransform =trans;
        isUP = true;
        Destroy(this.gameObject, 1f);
    }
}
