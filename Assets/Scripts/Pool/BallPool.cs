using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledBalls;
        public GameObject ballPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledBalls = new Queue<GameObject>();

            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject ball = Instantiate(pools[i].ballPrefab, this.transform);
                ball.SetActive(false);
                pools[i].pooledBalls.Enqueue(ball);
            }
        }
    }

    public GameObject GetPooledBall(int ballType)
    {
        if (ballType >= pools.Length)
        {
            Debug.LogError("İstenen top türü aralık dışında.");
            return null;
        }

        GameObject ball = pools[ballType].pooledBalls.Dequeue();
        pools[ballType].pooledBalls.Enqueue(ball);
        return ball;
    }

    public int GetPoolCount()
    {
        return pools.Length;
    }
}