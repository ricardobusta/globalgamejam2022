using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed;
    
    
    private Vector3 _rotation;
    
    private void Start()
    {
        _rotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        _rotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(_rotation);
    }
}
