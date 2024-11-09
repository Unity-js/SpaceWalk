using UnityEngine;
public class MovingObject : MonoBehaviour
{
    public float speed;  // �̵� �ӵ�

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -Camera.main.orthographicSize * Camera.main.aspect) // ������Ʈ ����
        {
            Destroy(gameObject);
        }
    }
}