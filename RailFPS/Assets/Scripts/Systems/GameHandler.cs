﻿using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StageSystem
{
    public enum GameState
    {
        Menu,
        Playing,
        Win,
        Lose
    }

    public class GameStateSignal
    {
        public GameState NewState { get; private set; }
        public GameStateSignal(GameState newState)
        {
            NewState = newState;
        }
    }
    
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] private GameState _state;

        private void Awake()
        {
            MessageBroker.Default.Receive<GameStateSignal>()
                .Subscribe(signal => UpdateGameState(signal.NewState));
            
            UpdateGameState(_state);
        }

        private void UpdateGameState(GameState signalNewState)
        {
            _state = signalNewState;
            switch (signalNewState)
            {
                case GameState.Menu:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
                case GameState.Playing:
                    Cursor.visible = false;
                    break;
                case GameState.Win:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
                case GameState.Lose:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(signalNewState), signalNewState, null);
            }
        }

        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}