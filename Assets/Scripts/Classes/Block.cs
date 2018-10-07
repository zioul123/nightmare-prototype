using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Block 
{
    [SerializeField]
    private GameObject first, second;

    // Deactivate the blocks
    public void Deactivate () 
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    // Activate the blocks
    public void Acctivate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }
}
