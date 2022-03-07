using UnityEngine;
using System.Collections;

public class MultiSkillCollider : MonoBehaviour
{

    public int attack;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == TagsManager.enemy)
        {
            transform.Rotate(transform.up * 180);
            collider.transform.GetComponent<Wolf>().TakeDamage(attack);
        }
    }
}
