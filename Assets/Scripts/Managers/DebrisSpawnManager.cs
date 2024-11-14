using UnityEngine;

public class DebrisSpawnManager : MonoBehaviour
{
    public Sprite[] sprites;  
    public float spawnInterval = 2f;  // ��ȯ ����
    public float moveSpeed = 5f;  // �̵� �ӵ�

    public float minY = -5f; 
    public float maxY = 5f;   

    private float screenWidth;  // ȭ��

    private void Start()
    {
        screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;

        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    private void SpawnObject()
    {
        int randomIndex = Random.Range(0, sprites.Length);
        Sprite randomSprite = sprites[randomIndex];

        float randomY = Random.Range(minY, maxY);

        GameObject newObject = new GameObject("MovingObject");
        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = randomSprite;

        newObject.transform.position = new Vector3(screenWidth / 2, randomY, 0);

        MovingObject mover = newObject.AddComponent<MovingObject>();
        mover.speed = moveSpeed;
    }
}
