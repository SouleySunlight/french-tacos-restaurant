using System.Collections.Generic;
using UnityEngine;

public class WindowVisual : MonoBehaviour
{
    [System.Serializable]
    private class WindowEntry
    {
        public WindowsEnum window;
        public GameObject windowObject;
    }


    [SerializeField] private List<WindowEntry> windowEntries;
    private Dictionary<WindowsEnum, GameObject> windows = new();

    void Awake()
    {
        foreach (WindowEntry windowEntry in windowEntries)
        {
            windows[windowEntry.window] = windowEntry.windowObject;
        }
        DisplayWindow(WindowsEnum.GAME);
    }

    public void DisplayWindow(WindowsEnum windowToShow)
    {
        foreach (var window in windows)
        {
            window.Value.SetActive(windowToShow == window.Key);
        }
    }

}