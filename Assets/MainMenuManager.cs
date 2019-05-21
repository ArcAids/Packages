using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using levelManagement;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        LevelLoadEvent loader;
        [SerializeField]
        bool showPromptBeforeExit=true;
        [SerializeField]
        GameObject exitPrompt;

        public void LoadGame()
        {
            loader.Load(1);
        }

        public void AttemptExitGame()
        {
            if (showPromptBeforeExit)
                exitPrompt.SetActive(true);
            else
                QuitGame();
        }

        public void QuitGame()
        {
                Application.Quit();
        }
    }
}
