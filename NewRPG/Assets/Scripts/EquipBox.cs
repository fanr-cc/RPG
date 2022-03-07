using UnityEngine;
using System.Collections;

public class EquipBox : MonoBehaviour {

    private Animation anim;
    private AudioSource audio;
    private bool isGet = false;

    void Awake()
    {
        anim = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !PublicFunctionManager.Instance.IsPointerOverUIObject()&&!isGet)
        {
            StartCoroutine(OpenBox());
            isGet = true;
        }
    }


    IEnumerator OpenBox()
    {   Destroy(this.gameObject,3.5f);
        audio.Play();
        anim.CrossFade("opening");
        yield return new WaitForSeconds(1.5f);
        BagManager._instance.GetObject(Random.Range(2001, 2023), 1);
        BagManager._instance.GetObject(Random.Range(1001, 1004), Random.Range(3, 6));
        anim.CrossFade("closing");
       
    }
}
