using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPMove : MonoBehaviour
{
    // Hareketin hızı
    [SerializeField] private float moveSpeed = 5f;

    // İleri ve geri (Z ekseninde) hareket edebileceği maksimum mesafe
    [SerializeField] private float moveRange = 5f;

    // Başlangıç pozisyonunu saklamak için değişken
    private Vector3 startPosition;

    // Start ile başlatma işlemleri
    void Start()
    {
        // Başlangıç pozisyonunu saklıyoruz
        startPosition = transform.position;
    }

    // Update her karede çağrılır
    void Update()
    {
        // Fare hareketi bilgilerini al
        float mouseX = Input.GetAxis("Mouse X");

        // Fare hareketine göre Z ekseninde spawn point'i hareket ettir
        float zPosition = transform.position.z + mouseX * moveSpeed * Time.deltaTime;

        // Hareketin sınırlarını belirle (başlangıç pozisyonuna göre Z ekseninde)
        zPosition = Mathf.Clamp(zPosition, startPosition.z - moveRange, startPosition.z + moveRange);

        // Spawn point'in yeni pozisyonunu ayarla
        transform.position = new Vector3(startPosition.x, startPosition.y, zPosition);
    }
}
