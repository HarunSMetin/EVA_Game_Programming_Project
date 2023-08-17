using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOrder : MonoBehaviour
{
    public GameObject[] cubes; // 4 cube'u bu dizi içinde tutacaðýz
    private float[] targetZPositions; // Hedeflenen Z pozisyonlarýný tutacaðýz
    private int currentCubeIndex = 0; // Sýradaki cube'u takip edeceðimiz indeks

    void Start()
    {
        // cubes dizisine cube'larý atayalým (Unity Inspector'dan da yapabilirsiniz)
        cubes = new GameObject[4];
        cubes[0] = GameObject.Find("Cube1");
        cubes[1] = GameObject.Find("Cube2");
        cubes[2] = GameObject.Find("Cube3");
        cubes[3] = GameObject.Find("Cube4");

        // Hedeflenen Z pozisyonlarýný oluþturalým
        targetZPositions = new float[cubes.Length];
        for (int i = 0; i < cubes.Length; i++)
        {
            targetZPositions[i] = (i + 1) * 1.0f;
        }
    }

    // Her frame'de kontrol edecek kodumuzu Update metodu içinde yazacaðýz
    void Update()
    {
        // Eðer sýradaki cube'un Z pozisyonu, hedeflenen Z pozisyonuna yakýnsa, bir sonraki cube'a geçelim
        if (Mathf.Approximately(cubes[currentCubeIndex].transform.position.z, targetZPositions[currentCubeIndex]))
        {
            currentCubeIndex++;

            // Eðer tüm cube'lar hedeflenen pozisyonlara gelmiþse, "kapi acil" mesajýný yazdýralým
            if (currentCubeIndex >= cubes.Length)
            {
                Debug.Log("kapi acil");
                // Ýstediðiniz ekstra iþlemleri burada yapabilirsiniz
            }
        }
    }
}
