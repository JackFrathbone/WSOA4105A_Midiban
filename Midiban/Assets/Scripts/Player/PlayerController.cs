using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;

    [Header("References")]
    private Vector3 _newPos;
    private Vector3 _oldPos;

    [Header("Data")]
    private bool _moving;
    private bool _againstObject;

    private bool _stopControl;
    private bool _moveStop;

    private void Start()
    {
        _newPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!_stopControl)
        {
            CheckInput();
        }

        if (transform.position != _newPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, _newPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            _moving = false;
        }

        if (!_moving && _againstObject)
        {
            print("return");
            RoundToNearestHalf(_oldPos.x);
            RoundToNearestHalf(_oldPos.y);
            _newPos = _oldPos;
            _moving = true;
        }
    }

    private void CheckInput()
    {
        if (!_moving && !_moveStop)
        {
            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    _oldPos = transform.position;
                    _newPos = _oldPos + new Vector3(0, 1, 0);

                    _moving = true;
                    _moveStop = true;
                    StartCoroutine(WaitAndMove());
                }
                else
                if (Input.GetAxis("Vertical") < 0)
                {
                    _oldPos = transform.position;
                    _newPos = _oldPos + new Vector3(0, -1, 0);

                    _moving = true;
                    _moveStop = true;
                    StartCoroutine(WaitAndMove());
                }
            }

            if (Input.GetButton("Horizontal"))
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    _oldPos = transform.position;
                    _newPos = _oldPos + new Vector3(1, 0, 0);

                    _moving = true;
                    _moveStop = true;
                    StartCoroutine(WaitAndMove());
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    _oldPos = transform.position;
                    _newPos = _oldPos + new Vector3(-1, 0, 0);

                    _moving = true;
                    _moveStop = true;
                    StartCoroutine(WaitAndMove());
                }
            }
        }
    }

    public float RoundToNearestHalf(float a)
    {
        return a = Mathf.Round(a * 2f) * 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Wall"))
        {
            if (_moving)
            {
                _stopControl = false;
                _newPos = _oldPos;
            }
        }
        else if (collision.collider.CompareTag("Note"))
        {
            _againstObject = true;

            string direction = "";

            if (_newPos.y > _oldPos.y)
            {
                direction = "Up";
            }
            else if (_newPos.y < _oldPos.y)
            {
                direction = "Down";
            }

            if (_newPos.x > _oldPos.x)
            {
                direction = "Right";
            }
            else if (_newPos.x < _oldPos.x)
            {
                direction = "Left";
            }

            collision.collider.GetComponent<NoteController>().PushObject(direction);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Note"))
        {
            _againstObject = false;
        }
    }

    private IEnumerator WaitAndMove()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        _moveStop = false;
    }
}
