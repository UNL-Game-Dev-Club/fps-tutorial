using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    
    Vector3 _randomMovement = Vector3.zero;

    private void Start()
    {
        AssignRandomMovementDir();
    }

    private void FixedUpdate()
    {
        transform.position += _randomMovement;
    }

    private void AssignRandomMovementDir()
    {
        _randomMovement = new Vector3(
            Random.Range(-1.0f, 1.0f), 
            Random.Range(-1.0f, 1.0f), 
            Random.Range(-1.0f, 1.0f)).normalized * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        AssignRandomMovementDir();
    }
}
