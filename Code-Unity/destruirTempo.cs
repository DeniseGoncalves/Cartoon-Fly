using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruirTempo : MonoBehaviour
{

    public  float   tempoDestruir;    

    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(this.gameObject, tempoDestruir);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
