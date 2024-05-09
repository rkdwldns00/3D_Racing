using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("오브젝트 참조")]
    public WheelCollider[] wheels;
    public Transform wayPoints;
    [Header("타이어 설정")]
    public float wheelRotationRange = 45;
    public float wheelMaxTorque = 30000;
    public float breakTorque = 500;
    [Header("부스트 설정")]
    public Transform[] boostPos;
    public GameObject boostTrailPrefab;
    public float boostSpeed = 10;
    public float boostGazyChargeTime = 5;
    [Header("기타 설정")]
    public float steamPressureForce = 4;
    public float sprintSpeed = 8; // 이 속도까지는 최대출력
    public float maxSpeed = 38; // 이 속도에 가까워질수록 출력이 작아짐

    protected Rigidbody rigid;
    public float boostTimer { get; set; }
    bool isBreaking;
    bool tryBreakThisFrame;
    public float boostGauge { get; set; }
    protected int sensorIndex = 0;
    protected GameObject[] sensor;
    public List<Ability> abils { get; set; }

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody>();
        sensor = new GameObject[wayPoints.childCount + 1];
        for (int i = 0; i < wayPoints.childCount; i++)
        {
            sensor[i] = wayPoints.GetChild(i).gameObject;
        }
        sensor[sensor.Length - 1] = gameObject;
    }

    public virtual void Update()
    {
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            Vector3 reduced = rigid.velocity * Time.deltaTime;
            rigid.velocity -= reduced;
            rigid.velocity += (transform.rotation * Vector3.forward * (boostSpeed * Time.deltaTime + reduced.magnitude));
        }
        else
        {
            boostGauge = Mathf.Min(1f, boostGauge + 1f / boostGazyChargeTime * Time.deltaTime);
        }
    }

    public void Boost()
    {
        if (boostGauge >= 0.3f && boostTimer <= 0)
        {
            boostTimer = boostGauge;
            boostGauge = 0;
            foreach (var t in boostPos)
            {
                Instantiate(boostTrailPrefab,t.position,Quaternion.identity).GetComponent<Boost>().target = t;
            }
        }
    }

    public void Rotate(float angle)
    {
        angle *= wheelRotationRange;
        wheels[2].transform.localEulerAngles = new Vector3(0, angle, 0);
        wheels[3].transform.localEulerAngles = new Vector3(0, angle, 0);
        wheels[2].steerAngle = angle;
        wheels[3].steerAngle = angle;
    }

    public void Move(float power)
    {
        power = Mathf.Min(power, (rigid.velocity.magnitude - maxSpeed) / -sprintSpeed);
        for (int i = 0; i < 2; i++)
        {
            wheels[i].motorTorque = wheelMaxTorque * Time.fixedDeltaTime * power;
        }
    }

    public void Break()
    {
        tryBreakThisFrame = true;
    }

    public void ResetPos()
    {
        transform.position = sensor[sensorIndex].transform.position;
        transform.rotation = sensor[sensorIndex].transform.rotation;
    }

    public virtual void LateUpdate()
    {
        if (tryBreakThisFrame)
        {
            isBreaking = true;
            if (boostGauge != 0)
            {
                boostGauge += Mathf.Abs(rigid.angularVelocity.y) * Time.deltaTime;
            }
        }
        else if (isBreaking)
        {
            isBreaking = false;
        }
        tryBreakThisFrame = false;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        for (int i = 0; i < 2; i++)
        {
            if (isBreaking)
            {
                wheels[i].brakeTorque = breakTorque;
            }
            else
            {
                wheels[i].brakeTorque = 0f;
            }
        }

        rigid.AddForce(Vector3.down * rigid.velocity.magnitude * steamPressureForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sensor[sensorIndex + 1])
        {
            sensorIndex++;
        }
    }
}
