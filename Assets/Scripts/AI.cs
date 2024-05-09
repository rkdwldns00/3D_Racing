using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Car
{
    [Header("����")]
    public float rotateRange; //���� ���� ������
    public float useBreakTorque; // ū ȸ���� �ʿ��Ҷ� �극��ũ ���
    public float breakSpeed; // �ʹ� �������� �극��ũ ���
    public float boostDistance; // ���� ��������Ʈ�� �� �Ÿ����� �ֶ��� �ν�Ʈ

    List<Vector3> postionHis = new List<Vector3>();

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 toVector = sensor[sensorIndex + 1].transform.position - transform.position;
        Debug.DrawRay(transform.position, toVector, Color.red);
        float needTurn = Mathf.Atan2(toVector.x, toVector.z) * Mathf.Rad2Deg - transform.rotation.eulerAngles.y;

        if (needTurn < -180)
        {
            needTurn += 360;
        }

        Rotate(Mathf.Clamp(needTurn / rotateRange, -1f, 1f));

        if (needTurn < rotateRange + 1 && toVector.magnitude > boostDistance)
        {
            Boost();
        }
        if (needTurn > useBreakTorque && rigid.velocity.magnitude > breakSpeed)
        {
            Break();
        }
        Move(1);

        postionHis.Add(transform.position);
        if (postionHis.Count > 60)
        {
            postionHis.RemoveAt(0);
            bool isNeedReset = true;
            foreach (var p in postionHis)
            {
                if (Vector3.Distance(p, transform.position) > 2)
                {
                    isNeedReset = false;
                    break;
                }
            }
            if (isNeedReset)
            {
                ResetPos();
                postionHis.Clear();
            }
        }
    }
}
