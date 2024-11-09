using UnityEngine;
public class MovingObject : MonoBehaviour
{
    public float speed;  // 이동 속도

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -Camera.main.orthographicSize * Camera.main.aspect) // 오브젝트 삭제
        {
            Destroy(gameObject);
        }
    }
}