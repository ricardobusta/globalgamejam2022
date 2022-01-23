using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject model;
    [SerializeField] private float speed = 10;
    [SerializeField] private float rotateSpeed = 400;

    private void Update()
    {
        var v = Input.GetAxisRaw("Vertical");
        var h = Input.GetAxisRaw("Horizontal");
        var direction = new Vector3(h, 0, v).normalized;

        if (direction != Vector3.zero)
        {
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation,
                Quaternion.LookRotation(direction, Vector3.up), rotateSpeed * Time.deltaTime);
        }
        
        direction.y = -0.1f;
        characterController.Move(direction * (Time.deltaTime * speed));
    }
}
