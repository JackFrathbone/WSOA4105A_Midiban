using System.Collections.Generic;
using UnityEngine;

public class PlayLineController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _playSpeed;

    [Header("References")]
    //Notes are from lowest to high, lowest note at [0]
    [SerializeField] List<AudioClip> _shortNotes;
    //[SerializeField] List<AudioClip> _LongNotes;

    private AudioSource _camAudioSource;

    [Header("Data")]
    private Vector2 _startPos;

    private void Start()
    {
        _camAudioSource = Camera.main.GetComponent<AudioSource>();

        _startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * _playSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            _camAudioSource.PlayOneShot(_shortNotes[GetNoteFromHeight(collision.transform.position.y)]);
        }

        if (collision.CompareTag("PlayLine"))
        {
            ResetPlayLine();
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
        transform.position = _startPos;
    }
}
