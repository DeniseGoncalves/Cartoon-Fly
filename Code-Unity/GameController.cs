using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public  enum    tagBullets
{
    player,
    inimigo
}

public  enum    gameState
{
    intro,
    gameplay
}

public class GameController : MonoBehaviour
{

    public  playerController    _playerController;

    public  gameState           currentState;

    public  GameObject          playerPrefab;
    public  int                 vidaExtra;
    public  Transform           spawnPlayer;

    public  float               delaySpawnPlayer;

    public  float               tempoInvencivel;

    [Header("Limites de Movimento")]
    public  Transform   limiteSuperior;
    public  Transform   limiteInferior;
    public  Transform   limiteEsquerdo;
    public  Transform   limiteDireito;
 
    public  Transform   posFinalFase;
    public  Transform   cenario;
    public  float       velocidadeFase;

 

    [Header("Prefabs")]
    public  GameObject[]    bullet;
    public  GameObject      explosaoPrefab;

    public  bool        isAlivePlayer;

    [Header("Config Intro")]
    public  float   tamanhoInicialNave;
    public  float   tamanhoOriginal;

    public  Transform posicaoInicialNave;
    public  Transform posicaoDecolagem;

    public  float     velocidadeDecolagem;
    private float     velocidadeAtual;

    private bool      isDecolar;

    public  Color     corInicialFumaca;
    public  Color     corFinalFumaca;

    public int        score;

    [Header("User Interface")]
    public  TMP_Text    txtScore;
    public  TMP_Text    txtVidasExtra;

    public  GameObject  ativarInimigos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("introFase");

        txtScore.text = "0";
        txtVidasExtra.text = "x" + vidaExtra.ToString();

        StartCoroutine("ativarNavesInimigas");
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlivePlayer == true)
        {
            limitarMovimentoPlayer();
        }
        
        if(isDecolar == true && currentState == gameState.intro)
        {
            _playerController.transform.position = Vector3.MoveTowards(_playerController.transform.position, posicaoDecolagem.position, velocidadeAtual * Time.deltaTime);

            if(_playerController.transform.position == posicaoDecolagem.position)
            {
                print("subir");
                StartCoroutine("subir");
                currentState = gameState.gameplay;
            }

            _playerController.fumacaSr.color = Color.Lerp(corInicialFumaca, corFinalFumaca, 0.1f);
        }
        

    }

    void LateUpdate()
    {
        if(currentState == gameState.gameplay)
        {
        cenario.position = Vector3.MoveTowards(cenario.position, new Vector3(cenario.position.x, posFinalFase.position.y, 0), velocidadeFase * Time.deltaTime);
        }
    }

    void limitarMovimentoPlayer()
    {
        float posY = _playerController.transform.position.y;
        float posX = _playerController.transform.position.x;

        if(posY > limiteSuperior.position.y)
        {
            _playerController.transform.position = new Vector3(posX, limiteSuperior.position.y, 0);
        }
        else if(posY < limiteInferior.position.y)
        {
            _playerController.transform.position = new Vector3(posX, limiteInferior.position.y, 0);
        }
        if(posX > limiteDireito.position.x)
        {
            _playerController.transform.position = new Vector3(limiteDireito.position.x, posY, 0);
        }
        else if(posX < limiteEsquerdo.position.x)
        {
            _playerController.transform.position = new Vector3(limiteEsquerdo.position.x, posY, 0);
        }
    }
    
    public  string  aplicarTag(tagBullets tag)
    {
        string retorno = null;

        switch(tag)
        {
        case tagBullets.player:
            retorno = "playerShot";
            break;
        case tagBullets.inimigo:
            retorno = "inimigoShot";
            break;        
        }

        return retorno;
    }

    public void hitPlayer() // Função ao tomar tiro inimigo
    {
        isAlivePlayer = false;
        Destroy(_playerController.gameObject);
        GameObject temp = Instantiate(explosaoPrefab, _playerController.transform.position, explosaoPrefab.transform.localRotation);
        vidaExtra -= 1;
                
        if(vidaExtra >= 0)
        {
           StartCoroutine("instanciarPlayer");
        }
        else
        {
            print("GAME OVER");
        }

        if(vidaExtra < 0)
        {
            vidaExtra = 0;
        }
        txtVidasExtra.text = "x" + vidaExtra.ToString();

    }

    IEnumerator instanciarPlayer()
    {
        yield return new WaitForSeconds(delaySpawnPlayer);
        GameObject temp = Instantiate(playerPrefab, spawnPlayer.position, spawnPlayer.localRotation);
        yield return new WaitForEndOfFrame();
        _playerController.StartCoroutine("invencivel");
       
    }

    IEnumerator introFase()
    {
        _playerController.fumacaSr.color = corInicialFumaca;
        _playerController.sombra.SetActive(false);
        _playerController.transform.localScale = new Vector3(tamanhoInicialNave, tamanhoInicialNave, tamanhoInicialNave);
        _playerController.transform.position = posicaoInicialNave.position;

        yield return new WaitForSeconds (2);
        isDecolar = true;

        print("autorizado");

        for(velocidadeAtual = 0; velocidadeAtual < velocidadeDecolagem; velocidadeAtual += 0.2f)
        {
            print(velocidadeAtual);
            yield return new WaitForSeconds(0.2f);
        }

    }

    IEnumerator subir()
    {
        _playerController.sombra.SetActive(true);
        for(float s = tamanhoInicialNave; s < tamanhoOriginal; s += 0.025f)
        {
            _playerController.transform.localScale = new Vector3(s,s,s);
            _playerController.sombra.transform.localScale = new Vector3(s,s,s);
            _playerController.fumacaSr.color = Color.Lerp(_playerController.fumacaSr.color, corFinalFumaca, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        print("altura máxima");
    }

    public void addScore(int pontos)
    {
        score += pontos;
        txtScore.text = score.ToString();
    }

    IEnumerator ativarNavesInimigas()
    {
        yield return new WaitForSeconds(10);
        ativarInimigos.SetActive(true);
    }
}
