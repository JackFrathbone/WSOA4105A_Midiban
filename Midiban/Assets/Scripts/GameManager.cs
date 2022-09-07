using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _endScreen;

    [SerializeField] PlayLineController _playLineController;

    [SerializeField] Image _muteButton;
    [SerializeField] Sprite _muteSprite;
    [SerializeField] Sprite _unMuteSprite;

    [Header("Data")]
    [SerializeField] List<NoteGoal> _noteGoals;
    private bool _gamePassed;
    private bool _menuOpen;

    public void ToggleMute()
    {
        _playLineController.muted = !_playLineController.muted;

        if (_playLineController.muted)
        {
            _muteButton.sprite = _muteSprite;
        }
        else
        {
            _muteButton.sprite = _unMuteSprite;
        }
    }

    public void ToggleMenu()
    {
        _menuOpen = !_menuOpen;

        if (_menuOpen)
        {
            _menuOpen = true;
            _menu.SetActive(true);
        }
        else
        {
            _menuOpen = false;
            _menu.SetActive(false);
        }
    }

    public void CheckGoals()
    {
        if (_gamePassed)
        {
            //You win
            _endScreen.SetActive(true);
        }

        bool passed = true;

        foreach(NoteGoal goal in _noteGoals)
        {
            if (!goal.GetGoalMet())
            {
                passed = false;
            }
        }

        if (passed)
        {
            _gamePassed = true;
        }
        else
        {
            _gamePassed = false;
            return;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextScene);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
