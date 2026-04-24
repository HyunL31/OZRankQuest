using UnityEngine;

public class Camera : MonoBehaviour
{
    public float offsetX = 10f;
    public float offsetY = 10f;
    public float offsetZ = 10f;

    public Transform player;

    void Update()
    {
        transform.position = new Vector3(player.position.x + offsetX, player.position.y + offsetY, player.position.z + offsetZ);
    }
}
