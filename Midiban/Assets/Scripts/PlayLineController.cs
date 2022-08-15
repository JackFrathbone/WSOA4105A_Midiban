using System.Collections.Generic;
using UnityEngine;

public class PlayLineController : MonoBehaviour
{
    [Header("Settings")]
    public bool muted;
    //How many seconds before moving to next line
    [SerializeField] float _playTime;

    [Header("References")]
    //Notes are from lowest to high, lowest note at [0]
    [SerializeField] List<AudioClip> _shortNotes;
    //[SerializeField] List<AudioClip> _LongNotes;

    private AudioSource _camAudioSource;

    private GameManager _gameManager;

    [Header("Data")]
    private bool _resetNext;

    [Header("Data")]
    private Vector2 _startPos;
    private float _timer;

    private void Start()
    {
        _camAudioSource = Camera.main.GetComponent<AudioSource>();

        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        _startPos = transform.position;
        _timer = 1;
    }

    private void Update()
    {
        _timer -= 1 * Time.deltaTime;

        if(_timer <= 0)
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            _timer = _playTime;
        }

        if (_resetNext)
        {
            ResetPlayLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            _camAudioSource.PlayOneShot(_shortNotes[GetNoteFromHeight(collision.transform.position.y)]);
        }

        if (collision.CompareTag("NoteMuteable") && !muted)
        {
            _camAudioSource.PlayOneShot(_shortNotes[GetNoteFromHeight(collision.transform.position.y)]);
        }

        if (collision.CompareTag("PlayLine"))
        {
            _resetNext = true;
        }
    }

    private int GetNoteFromHeight(float y)
    {
        switch (y)
        {
            case -3.5f:
                return 0;
            case -2.5f:
                return 1;
            case -1.5f:
                return 2;
            case -0.5f:
                return 3;
            case 0.5f:
                return 4;
            case 1.5f:
                return 5;
            case 2.5f:
                return 6;
            case 3.5f:
                return 7;
        }

        return 0;
    }

    public void ResetPlayLine()
    {
        _resetNext = false;
        _gameManager.CheckGoals();
        transform.position = _startPos;
    }
}
