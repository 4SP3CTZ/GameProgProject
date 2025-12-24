using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutlineOnStart : MonoBehaviour
{
    private float delay = 0.1f;
    Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        Invoke("DisabledOutline", delay);
    }

    // Update is called once per frame
    private void DisabledOutline()
    {
        outline.enabled = false;
    }
}
