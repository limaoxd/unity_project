using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    private RectTransform rectComponent;
    public float rotateSpeed = 50f;
    // Start is called before the first frame update
    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
