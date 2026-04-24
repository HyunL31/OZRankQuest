using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject third;
    public GameObject first;

    private bool isThird = true;

    void Start()
    {
        third.SetActive(true);
        first.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isThird)
        {
            third.SetActive(false);
            first.SetActive(true);

            isThird = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !isThird)
        {
            third.SetActive(true);
            first.SetActive(false);

            isThird = true;
        }
    }
}
