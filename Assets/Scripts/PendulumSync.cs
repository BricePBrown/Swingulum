using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumSync : MonoBehaviour
{
    public GameObject[] pList;
    // Start is called before the first frame update
    void Start()
    {
        pList = GameObject.FindGameObjectsWithTag("Pendulum");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int y = 1; y < pList.Length; y++)
        {
            pList[y].transform.eulerAngles = new Vector3(pList[y].transform.eulerAngles.x,
                                                         pList[y].transform.eulerAngles.y,
                                                         pList[0].transform.eulerAngles.z);
        }
    }
}
