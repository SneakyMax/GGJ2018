using System;
using UnityEngine;
using UnityEngine.UI;

public class FormattableText : MonoBehaviour
{
    private Text text;
    private string format;

    public void Awake()
    {
        text = GetComponent<Text>();
        format = text.text;
    }

    public void Format(params object[] args)
    {
        text.text = String.Format(format, args);
    }
}