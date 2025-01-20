using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{

    private playerController    _playerController;

    public GameObject       explosaoPrefab;
    public GameObject[]     loot;

    public Transform        arma;
    public GameObject       tiro;

    public float[]           delayEntreTiros;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType(typeof(playerController)) as playerController;

        StartCoroutine("atirar");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
        case "playerShot":

            Destroy(col.gameObject);

            GameObject temp = Instantiate(explosaoPrefab, transform.position, transform.localRotation);
            Destroy(temp.gameObject, 0.5f);

            Spanwloot();

            Destroy(this.gameObject);

            break;
        }
    }

    void Spanwloot()
    {
        int idItem = 0;
        int rand = Random.Range(0,100);
        if(rand < 50)
        {
            rand = Random.Range(0,100);
            if(rand > 85)
            {
                idItem = 2; //CAIXA BOMBA
            }
            else if(rand > 50)
            {
                idItem = 1; //CAIXA SAUDE
            }
            else
            {
                idItem = 0; //CAIXA MOEDA
            }

            Instantiate(loot[idItem], transform.position, transform.localRotation);
        }
        
    }

    void shot()
    {
        arma.right = _playerController.transform.position - transform.position;
        GameObject temp = Instantiate(tiro, arma.position, arma.localRotation);
        temp.GetComponent<Rigidbody2D>().velocity = arma.right * 3;
    }

    IEnumerator atirar()
    {
        yield return new WaitForSeconds(Random.Range(delayEntreTiros[0],delayEntreTiros[1]));
        shot();
        StartCoroutine("atirar");
    }
}
