using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Clouds : MonoBehaviour
{
    public Transform closeCloud1;
    public Transform closeCloud2;
    public Transform farCloud1;
    public Transform farCloud2;

    private Vector3 close1;
    private Vector3 close2;
    private Vector3 far1;
    private Vector3 far2;

    public float speedClose;
    public float speedFar;

    void Start()
    {
        close1 = closeCloud1.transform.position;
        close2 = close1 + Vector3.forward * closeCloud1.localScale.y;

        closeCloud1.position = close1;
        closeCloud2.position = close2;


        far1 = farCloud1.transform.position;
        far2 = far1 + Vector3.forward * farCloud1.localScale.y;

        farCloud1.position = far1;
        farCloud2.position = far2;
    }

    // Will break if going too fast.
    void Update()
    {
        close1 -= Vector3.forward * speedClose * Time.deltaTime;
        close2 -= Vector3.forward * speedClose * Time.deltaTime;

        if (close1.z <= -60) close1.z += closeCloud1.localScale.y * 2;
        if (close2.z <= -60) close2.z += closeCloud2.localScale.y * 2;

        closeCloud1.position = close1;
        closeCloud2.position = close2;


        far1 -= Vector3.forward * speedFar * Time.deltaTime;
        far2 -= Vector3.forward * speedFar * Time.deltaTime;

        if (far1.z <= -140) far1.z += farCloud1.localScale.y * 2;
        if (far2.z <= -140) far2.z += farCloud2.localScale.y * 2;

        farCloud1.position = far1;
        farCloud2.position = far2;
    }
}
