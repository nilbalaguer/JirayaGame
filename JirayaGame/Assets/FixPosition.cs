using UnityEngine;

public class FixPosition : MonoBehaviour
{
    Transform parent;
    Vector3 positionLocal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent;
        positionLocal = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {

       if (parent == null) return;

        transform.localPosition = positionLocal;

        transform.localScale = Vector3.one;

        transform.rotation = Quaternion.identity;
    }
}
