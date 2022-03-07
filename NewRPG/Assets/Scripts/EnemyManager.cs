using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager _instance;

    public GameObject wolfBabyPrefab;
    public GameObject wolfNormalPrefab;
    public GameObject wolfBossPrefab;
    public Transform wolfBabyPosition;
    public Transform wolfNormalPosition;
    public Transform wolfBossPosition;

    public int wolfBabyCount;
    public int wolfNormalCount;
    public int wolfBossCount;
    public int wolfBabyMax=10;
    public int wolfNormalMax=5;
    public int wolfBossMax=1;

    private float timer_wolfBaby;
    private float timer_wolfNormal;
    private float timer_wolfBoss;

    private int timewait_wolfBaby = 3;
    private int timewait_wolfNormal = 5;
    private int timewait_wolfBoss = 10;
    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	    if (wolfBabyCount<wolfBabyMax)
	    {
	        timer_wolfBaby += Time.deltaTime;
	        if (timer_wolfBaby >= timewait_wolfBaby)
	        {
	            Vector3 pos = wolfBabyPosition.position;
	            pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);

                GameObject go = Instantiate(wolfBabyPrefab, pos, Quaternion.identity) as GameObject;
                go.transform.parent = this.transform;
                timer_wolfBaby = 0;
                wolfBabyCount++;
	        }
	    }

        if (wolfNormalCount < wolfNormalMax)
        {
            timer_wolfNormal += Time.deltaTime;
            if (timer_wolfNormal >= timewait_wolfNormal)
            {
                Vector3 pos = wolfNormalPosition.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                GameObject go = Instantiate(wolfNormalPrefab, pos, Quaternion.identity) as GameObject;
                go.transform.parent = this.transform;
                timer_wolfNormal = 0;
                wolfNormalCount++;
            }
        }

        if (wolfBossCount < wolfBossMax)
        {
            timer_wolfBoss += Time.deltaTime;
            if (timer_wolfBoss >= timewait_wolfBoss)
            {
                GameObject go = Instantiate(wolfBossPrefab, wolfBossPosition.position, wolfBossPosition.rotation)as GameObject;
                go.transform.parent = this.transform;
                timer_wolfBoss = 0;
                wolfBossCount++;
            }
        }
    }
    public void wolfBabyCountChange()
    {
        wolfBabyCount--;
    }
    public void wolfNormalCountChange()
    {
        wolfNormalCount--;
    }
    public void wolfBossCountChange()
    {
        wolfBossCount--;
    }
}
