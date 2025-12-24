using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panel;
    public KeyCode openPanelKey = KeyCode.B;

    public void Update()
    {
        if (Input.GetKeyDown(openPanelKey))
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
}
