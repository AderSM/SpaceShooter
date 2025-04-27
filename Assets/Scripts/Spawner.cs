using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;
    [SerializeField] private TextMeshProUGUI textoScore;
    private ObjectPool<Enemigo> enemigoPool;
    private int enemigosDestruidos;
    private int enemigosDestruidos2;
    [SerializeField] private GameObject canvas1;
    [SerializeField] private AudioSource soundBum;
    // Start is called before the first frame update
    private void Awake()
    {
        enemigoPool = new ObjectPool<Enemigo>(CreateEnemigo, null, OnReleaseEnemigo, OnDestroyEnemigo);
    }
    
    private Enemigo CreateEnemigo()
    {
        Enemigo enemigoCopy = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        enemigoCopy.MiPool = enemigoPool;
        return enemigoCopy;
    }

    private void OnReleaseEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }
    private void OnDestroyEnemigo(Enemigo enemigo)
    {
        Destroy(enemigo.gameObject);
    }

    void Start()
    {
        StartCoroutine(SpawnearEnemigos());
        Enemigo.ResetearEnemigos();
    }

    // Update is called once per frame
    void Update()
    {
        enemigosDestruidos = Enemigo.GetEnemigosDestruidosPorDisparoPlayer();
        textoScore.text = "Score: " + enemigosDestruidos;
        if (enemigosDestruidos > enemigosDestruidos2)
        {
            soundBum.Play();
            enemigosDestruidos2 = enemigosDestruidos;
        }
    }

     IEnumerator SpawnearEnemigos()
    {
        for (int i = 0; i < 3; i++) //Niveles
        {
            for (int j = 0; j < 3; j++) //Oleadas
            {
                textoOleadas.text = "Nivel " + (i + 1) + " - " + "Oleada " + (j + 1);
                yield return new WaitForSeconds(3f);
                textoOleadas.text = "";
                for (int k = 0; k < 10; k++) //Enemigos
                {
                    Enemigo enemigoCopy = enemigoPool.Get();
                    enemigoCopy.transform.position = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                    enemigoCopy.gameObject.SetActive(true);
                    enemigoCopy.LiberadoAlPool = false;
                    enemigoCopy.StartCoroutine("SpawnearDisparos");
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
        }
        Time.timeScale = 0;
        canvas1.SetActive(true);
    }
}
