using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum    delayTiro
{
    cima,
    baixo,
    esquerda,
    direita
}

public class IAInimigoA : MonoBehaviour
{
    private GameController  _GameController;

    public  float       velocidadeMovimento;

    public  float       pontoInicialCurva;

    private bool        isCurva;

    public  float       grausCurva;

    public  float       incrementar;

    private float       incrementado;

    private float       rotacaoZ;

    public  int             idBullet;
    public  tagBullets      tagTiro;

    public  Transform   arma;
    public  float       velocidadeTiro;

    public  float       delayTiro;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        rotacaoZ = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        controleCurva();

    }

    void atirar()
    {
        GameObject temp = Instantiate(_GameController.bullet[idBullet], arma.position, transform.localRotation);

        temp.transform.tag = _GameController.aplicarTag(tagTiro);

        temp.GetComponent<Rigidbody2D>().velocity = transform.up * -1 * velocidadeTiro;
    }

    void OnBecameVisible()
    {

        StartCoroutine("controleTiro");
    }

    IEnumerator controleTiro()
    {
        yield return new WaitForSeconds(delayTiro);
        atirar();
        StartCoroutine("controleTiro");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
        case "playerShot":

            GameObject temp = Instantiate(_GameController.explosaoPrefab, transform.position, _GameController.explosaoPrefab.transform.localRotation);
            Destroy(col.gameObject);
            Destroy(this.gameObject);

            break;
        }
    }

    void controleCurva()
    {
        if(transform.position.y <= pontoInicialCurva && isCurva == false)
        {
            isCurva = true;
        }

        if(isCurva == true && incrementado < grausCurva)
        {
            rotacaoZ += incrementar;
            transform.rotation = Quaternion.Euler(0,0, rotacaoZ);

            if(incrementar < 0)
            {
                incrementado += (incrementar * -1);
            }
            else
            {
                incrementado += incrementar;
            }
            
        }
        
        transform.Translate(Vector3.down * velocidadeMovimento * Time.deltaTime);
    }
}
