using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;   // Hareket hızı
    [SerializeField] private float moveRange = 5f;   // Z ekseninde hareket sınırı
    private Vector3 startPosition;                   // Başlangıç pozisyonu

    private GameObject currentBall;  // Şu anki aktif top
    private Rigidbody ballRigidbody; // Topun Rigidbody'si

    private bool isBallReleased = false;  // Top serbest bırakıldı mı?

    // BallPool'dan rastgele top alacağız
    [SerializeField] private BallPool ballPool;

    void Start()
    {
        // SpawnPoint'in başlangıç pozisyonunu saklıyoruz
        startPosition = transform.position;

        // Oyun başladığında rastgele bir top aktif edilir
        SpawnRandomBall();
    }

    void Update()
    {
        // Eğer top serbest bırakılmadıysa (fare ile hareket edebilir)
        if (!isBallReleased && currentBall != null)
        {
            // Kamera ve mouse pozisyonunu kullanarak Z ekseninde hareket ettirme
            Vector3 mousePosition = Input.mousePosition;  // Ekrandaki fare pozisyonu
            mousePosition.z = 10.0f; // Kamera uzaklığını belirtiyoruz (dünya koordinatları için)
            
            // Fare pozisyonunu dünya koordinatlarına çeviriyoruz
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            // Z eksenindeki pozisyonu ayarla (X ve Y sabit)
            float zPosition = Mathf.Clamp(worldMousePosition.z, startPosition.z - moveRange, startPosition.z + moveRange);
            
            // Topun pozisyonunu güncelle
            currentBall.transform.position = new Vector3(currentBall.transform.position.x, currentBall.transform.position.y, zPosition);
        }

        // Mouse tıklamasıyla serbest düşüş başlatma
        if (Input.GetMouseButtonDown(0) && ballRigidbody != null && !isBallReleased)
        {
            // Y ekseninde serbest bırak, serbest düşüş başlasın
            ballRigidbody.constraints = RigidbodyConstraints.None;
            ballRigidbody.useGravity = true;  // Yerçekimini aktif et

            // Top serbest bırakıldı, artık fareyi takip etmeyecek
            isBallReleased = true;
        }
    }

    private void SpawnRandomBall()
    {
        // BallPool'dan rastgele bir top al
        currentBall = ballPool.GetPooledBall(Random.Range(0, ballPool.GetPoolCount()));

        if (currentBall != null)
        {
            currentBall.SetActive(true);
            ballRigidbody = currentBall.GetComponent<Rigidbody>();

            // Topu ilk başta X ve Y ekseninde kilitle, sadece Z ekseninde hareket etsin
            ballRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            ballRigidbody.useGravity = false;  // Başlangıçta yerçekimi kapalı olsun

            // Başlangıçta fare ile hareketi aktif yap
            isBallReleased = false;
        }
    }
}
