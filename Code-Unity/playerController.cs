using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private GameController  _GameController;

    private Rigidbody2D     playerRb;

    private SpriteRenderer  playerSr;
    public  SpriteRenderer  fumacaSr;
    public  GameObject       sombra;


    public  float           velocidade;

    public  Transform       armaPosition;

    public  float           velocidadeTiro;

    public  int             idBullet;
    public  tagBullets      tagTiro;

    public  Color           corInvencivel;
    public  float           delayPiscar;


    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

        _GameController._playerController = this;
        _GameController.isAlivePlayer = true;

        playerRb = GetComponent<Rigidbody2D>();
        playerSr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(_GameController.currentState == gameState.gameplay)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical   = Input.GetAxis("Vertical");

            playerRb.velocity = new Vector2(horizontal * velocidade, vertical * velocidade);

            if(Input.GetButtonDown("Fire1")) //Fire1 corresponde a tecla Ctrl
            {
                shot();
            }
        }

    }

    void shot()
    {

        GameObject temp = Instantiate(_GameController.bullet[idBullet]);

        temp.transform.tag = _GameController.aplicarTag(tagTiro);
        
        temp.transform.position = armaPosition.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeTiro);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
        case "inimigoShot":

            _GameController.hitPlayer();

            Destroy(col.gameObject);

            break;
        }
    }

    IEnumerator invencivel()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        playerSr.color = corInvencivel;
        fumacaSr.color = corInvencivel;
        StartCoroutine("piscarPlayer");

        yield return new WaitForSeconds(_GameController.tempoInvencivel);
        col.enabled = true;
        playerSr.color = Color.white;
        fumacaSr.color = Color.white;
        playerSr.enabled = true;
        fumacaSr.enabled = true;
        StopCoroutine("piscarPlayer");
    }

    IEnumerator piscarPlayer()
    {
        yield return new WaitForSeconds(delayPiscar);
        playerSr.enabled = !playerSr.enabled;
        fumacaSr.enabled = !fumacaSr.enabled;
        StartCoroutine("piscarPlayer");
    }

}
