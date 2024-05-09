using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public Transform target;
    float timer;
    TrailRenderer trail;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
        timer += Time.deltaTime;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(new(0, -Mathf.Pow(timer - 0.5f, 2) + 0.25f));
        curve.AddKey(new(1, 0));
        trail.widthCurve = curve;
        if (timer > 1)
        {
            Destroy(gameObject);
        }
    }
}
