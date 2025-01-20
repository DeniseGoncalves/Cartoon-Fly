using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotController : MonoBehaviour
{
    
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
