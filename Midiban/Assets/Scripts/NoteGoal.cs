using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGoal : MonoBehaviour
{
    [Header("Data")]
    private bool _goalMet;

    public bool GetGoalMet()
    {
        return _goalMet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NoteMuteable"))
        {
            _goalMet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NoteMuteable") && _goalMet == true)
        {
            _goalMet = false;
        }
    }
}
