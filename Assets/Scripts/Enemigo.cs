using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velodidad;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private GameObject spawnPoint;

    private ObjectPool<Enemigo> miPool;
    private bool liberadoAlPool = false;
    public ObjectPool<Enemigo> MiPool { set => miPool = value; }
    public bool LiberadoAlPool { get => liberadoAlPool; set => liberadoAlPool = value; }

    private ObjectPool<Disparo> dispEnemPool;
    private static int enemigosDestruidosPorDisparoPlayer = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        dispEnemPool = new ObjectPool<Disparo>(CreateDisparo, null, OnReleaseDisparo, OnDestroyDisparo);
    }
    private Disparo CreateDisparo()
    {
        Disparo disparoCopy = Instantiate(disparoPrefab, spawnPoint.transform.position, Quaternion.identity);
        disparoCopy.MiPool2 = dispEnemPool;
        return disparoCopy;
    }

    private void OnReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void OnDestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }

    void Start()
    {
        StartCoroutine(SpawnearDisparos());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velodidad * Time.deltaTime);
    }

    public IEnumerator SpawnearDisparos()
    {
        while (true)
        {
            Disparo disparoCopy = dispEnemPool.Get();
            if (disparoCopy != null)
            {
                disparoCopy.transform.position = spawnPoint.transform.position;
                disparoCopy.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (!liberadoAlPool)
        {
            if (elOtro.gameObject.CompareTag("DisparoPlayer"))
            {
                StopCoroutine("SpawnearDisparos");
                miPool.Release(this);
                liberadoAlPool = true;
                enemigosDestruidosPorDisparoPlayer++;

            }
            if ((elOtro.gameObject.CompareTag("Player") ))
            {
                miPool.Release(this);
                liberadoAlPool = true;
                enemigosDestruidosPorDisparoPlayer++;
            }
            if (elOtro.gameObject.CompareTag("Limite"))
            {
                miPool.Release(this);
                liberadoAlPool = true;
            }
        }
    }

    public static int GetEnemigosDestruidosPorDisparoPlayer()
    {
        return enemigosDestruidosPorDisparoPlayer;
    }

    public static void ResetearEnemigos()
    {
        enemigosDestruidosPorDisparoPlayer = 0;
    }
}
