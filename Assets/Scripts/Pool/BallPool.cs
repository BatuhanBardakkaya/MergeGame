using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    // Havuzdaki nesneleri (topları) tanımlayan yapı
    [System.Serializable]
    public struct Pool
    {
        // Havuzdaki kullanılabilir topları saklayan kuyruk
        public Queue<GameObject> pooledBalls;
        // Top prefabı (şablonu)
        public GameObject ballPrefab;
        // Havuzda başlangıçta oluşturulacak top sayısı
        public int poolSize;
    }

    // Farklı top türleri için havuzları içeren dizi
    [SerializeField] private Pool[] pools = null;

    // Oyun başladığında havuzları başlatan fonksiyon
    private void Awake()
    {
        InitializePools();
    }

    // Havuzları başlatan fonksiyon
    private void InitializePools()
    {
        // Her bir havuz için
        for (int i = 0; i < pools.Length; i++)
        {
            // Havuzdaki topları saklayan kuyruğu başlat
            pools[i].pooledBalls = new Queue<GameObject>();
            
            // Havuzdaki top sayısı kadar döngü çalıştır
            for (int j = 0; j < pools[i].poolSize; j++)
            {
                // Top prefabını BallPool GameObject'i altında oluştur ve aktifliğini kapat
                GameObject ball = Instantiate(pools[i].ballPrefab, this.transform);
                ball.SetActive(false);
                
                // Topu havuza (kuyruğa) ekle
                pools[i].pooledBalls.Enqueue(ball);
            }
        }
    }

    // Havuzdan bir top alıp döndüren fonksiyon
    public GameObject GetPooledBall(int ballType)
    {
        // İstenen top türü havuz sayısını geçerse hata ver
        if (ballType >= pools.Length)
        {
            Debug.LogError("İstenen top türü aralık dışında.");
            return null;
        }

        // Havuzdan bir top al, kuyruğun başına geri ekle ve döndür
        GameObject ball = pools[ballType].pooledBalls.Dequeue();
        pools[ballType].pooledBalls.Enqueue(ball);

        // Topu aktif hale getir
        ball.SetActive(true);

        return ball;
    }
}
