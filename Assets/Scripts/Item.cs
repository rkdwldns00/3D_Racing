using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite[] sprites;

    bool isGet = false;

    public void GetItem(Car car)
    {
        if (isGet || car.abils.Count > 1)
        {
            return;
        }
        isGet = true;

        int itemType = UnityEngine.Random.Range(0, 3);
        Ability abil = new();
        abil.sprite = sprites[itemType];
        switch (itemType)
        {
            case 0:
                abil.action = () =>
                {

                };
                break;
            case 1:
                abil.action = () =>
                {

                };
                break;
            case 2:
                abil.action = () =>
                {

                };
                break;
        }

        car.abils.Add(abil);
        Destroy(gameObject);
    }
}

public class Ability
{
    public Sprite sprite;
    public Action action;
}