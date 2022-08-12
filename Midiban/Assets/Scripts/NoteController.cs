using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _moveSpeed;

    [Header("Data")]
    private Vector3 _newPos;
    private Vector3 _oldPos;

    private bool _moving;

    private void Start()
    {
        _newPos = transform.position;
    }

    private void Update()
    {
        if (transform.position != _newPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, _newPos, _moveSpeed * Time.deltaTime);
        }
        else
        {
            _moving = false;
        }
    }

    public void PushObject(string direction)
    {
        if (!_moving)
        {
            if (direction == "Up")
            {
                _oldPos = transform.position;
                _newPos = _oldPos + new Vector3(0, 1, 0);

                _moving = true;
            }
            else if (direction == "Down")
            {
                _oldPos = transform.position;
                _newPos = _oldPos + new Vector3(0, -1, 0);

                _moving = true;
            }
            else if (direction == "Right")
            {
                _oldPos = transform.position;
                _newPos = _oldPos + new Vector3(1, 0, 0);

                _moving = true;
            }
            else if (direction == "Left")
            {
                _oldPos = transform.position;
                _newPos = _oldPos + new Vector3(-1, 0, 0);

                _moving = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            if (_moving)
            {
                _newPos = _oldPos;
            }
        }
        else if (collision.collider.CompareTag("Player"))
        {
            return;
        }
    }
}
