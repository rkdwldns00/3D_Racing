using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public enum UIType
    {
        BoostGauge
    }

    public UIType type;
    public Slider slider;
    public Text text;
    Car target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerControll>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case UIType.BoostGauge:
                text.text = (int)(target.boostGauge * 100) + "%";
                if (target.boostGauge == 0)
                {
                    slider.value = target.boostTimer;
                }
                else
                {
                    slider.value = target.boostGauge;
                }
                break;
        }
    }
}
