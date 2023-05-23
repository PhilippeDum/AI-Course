using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 20;

    private Vector2 movement = Vector2.zero;

    private void Start()
    {
        
    }

    private void Update()
    {
        movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Translate(new Vector3(movement.x * speed * Time.deltaTime, 0, movement.y * speed * Time.deltaTime));
    }
}