using System.Collections.Generic;
using FMODUnity;
using FMOD;
using FMODUnityResonance;
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

   // private StudioEventEmitter _audioSource;

    private GameManager _gameManager;

    [Header("Data")]
    private bool _resetNext;

    [Header("Data")]
    private Vector2 _startPos;
    private float _timer;

    private void Start()
    {
        //_audioSource = GameObject.FindGameObjectWithTag("AudioController").GetComponent<StudioEventEmitter>();

        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        _startPos = transform.position;
        _timer = 1;

        MusicManager.beatUpdated += AdvanceLine;
    }

    private void Update()
    {
        /* 
        _timer -= 1 * Time.deltaTime;

       if (_timer <= 0 && !_resetNext)
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            _timer = _playTime;
        }

        else if (_timer <= 0 && _resetNext)
        {
            ResetPlayLine();
            _timer = _playTime;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(GetNoteFromHeight(collision.transform.position.y));
            //_audioSource.PlayOneShot(_shortNotes[GetNoteFromHeight(collision.transform.position.y)]);
        }

        if (collision.CompareTag("NoteMuteable") && !muted)
        {
            FMODUnity.RuntimeManager.PlayOneShot(GetNoteFromHeight(collision.transform.position.y));
            //_audioSource.PlayOneShot(_shortNotes[GetNoteFromHeight(collision.transform.position.y)]);
        }

        if (collision.CompareTag("PlayLine"))
        {
            _resetNext = true;
        }
    }

    private string GetNoteFromHeight(float y)
    {
        switch (y)
        {
            case -3.5f:
                return "event:/CNotePlay";
            case -2.5f:
                return "event:/DNotePlay";
            case -1.5f:
                return "event:/ENotePlay";
            case -0.5f:
                return "event:/FNotePlay";
            case 0.5f:
                return "event:/GNotePlay";
            case 1.5f:
                return "event:/ANotePlay";
            case 2.5f:
                return "event:/BNotePlay";
            case 3.5f:
                return "event:/C2NotePlay";
        }

        return "event:/CNotePlay";
    }

    public void ResetPlayLine()
    {
        _resetNext = false;
        _gameManager.CheckGoals();
        transform.position = _startPos;
    }

    public void AdvanceLine()
    {
        if (_resetNext)
        {
            ResetPlayLine();
            return;
        }

        transform.position = new Vector2(transform.position.x + 1, transform.position.y);
    }
}
