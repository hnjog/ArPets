using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosCheck : MonoBehaviour
{
    public Text text;

    private void Update()
    {
        text.text = transform.position + "";
    }
}
