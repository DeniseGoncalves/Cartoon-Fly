using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajetoInimigo : MonoBehaviour
{
    public  Transform     naveInimiga;
    public  Transform[]   checkPoints;

    public  float         velocidadeMovimento;
    public  float         delayParado;

    private int           idCheckPoints;
    private bool          movimentar;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("iniciarMovimento");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(movimentar == true)
        {
            naveInimiga.localPosition = Vector3.MoveTowards(naveInimiga.localPosition, checkPoints[idCheckPoints].position, velocidadeMovimento * Time.deltaTime);
            if(naveInimiga.localPosition == checkPoints[idCheckPoints].position)
            {
                movimentar = false;
                StartCoroutine("iniciarMovimento");
            }
        }

    }

    IEnumerator iniciarMovimento()
    {
        idCheckPoints += 1;
        if(idCheckPoints >= checkPoints.Length)
        {
            idCheckPoints = 0;
        }

        yield return new WaitForSeconds(delayParado);
        movimentar = true;
    }
}
