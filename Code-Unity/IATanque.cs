using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATanque : MonoBehaviour
{
    private GameController      _GameController;

    public  int             idBullet;
    public  tagBullets      tagTiro;

    public  Transform   arma;
    public  float       velocidadeTiro;

    public  float       delayTiro;
    public  int         pontos;


    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void atirar()
    {
        if(_GameController.isAlivePlayer == true)
        {

        arma.up = _GameController._playerController.transform.position - transform.position;
        GameObject temp = Instantiate(_GameController.bullet[idBullet], arma.position, arma.localRotation);
        temp.transform.tag = _GameController.aplicarTag(tagTiro);

        temp.GetComponent<Rigidbody2D>().velocity = arma.up * velocidadeTiro;
        }
      
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
        case "playerShot":

            GameObject temp = Instantiate(_GameController.explosaoPrefab, transform.position, _GameController.explosaoPrefab.transform.localRotation);
            temp.transform.parent = _GameController.cenario;

            _GameController.addScore(pontos);

            Destroy(col.gameObject);
            Destroy(this.gameObject);

            break;
        }
    }
}
