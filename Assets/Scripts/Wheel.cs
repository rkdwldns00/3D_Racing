using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public GameObject makerPrefab;

    WheelCollider col;

    GameObject maker;

    void Start()
    {
        col = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);

        if (col.brakeTorque != 0)
        {
            if (maker == null)
            {
                maker = Instantiate(makerPrefab, transform.position, Quaternion.Euler(0, 0, 90));
            }
            maker.transform.position = transform.position;
        }
        else
        {
            if (maker != null)
            {
                maker = null;
            }
        }
    }
}
