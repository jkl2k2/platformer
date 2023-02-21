using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private DateTime start;
    private void Start()
    {
        start = DateTime.Now;
    }

    private int SecondsSinceStart()
    {
        return (int) Math.Abs((start - DateTime.Now).TotalSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().SetText((375 - SecondsSinceStart()).ToString());
    }
}
