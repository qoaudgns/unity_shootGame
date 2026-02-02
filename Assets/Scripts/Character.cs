using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHP = 3;
    public GameObject HPGauge;
    
    private float HP;
    private float HPMaxWidth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HP = maxHP;

        if (HPGauge != null)
        {
            HPMaxWidth = HPGauge.GetComponent<RectTransform>().sizeDelta.x;    
        }
        
    }

    public void Initialize()
    {
        HP = maxHP;
    }

    /**
     * 살아있으면 true return
     */
    public bool Hit(float damage)
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
        }

        if (HPGauge != null)
        {
            HPGauge.GetComponent<RectTransform>().sizeDelta = new Vector2(HP/maxHP * HPMaxWidth, HPGauge.GetComponent<RectTransform>().sizeDelta.y);    
        }
        
        return HP > 0;
    }
}
