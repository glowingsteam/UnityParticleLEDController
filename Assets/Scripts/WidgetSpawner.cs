using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hello.Cutie
{
    public class WidgetSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _startingWidget;
        [SerializeField] Transform _spawnTransform;

        private GameObject _spawnedWidget;

        private void Start()
        {
            //Screen.fullScreenMode = ;
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                Application.targetFrameRate = 60;
                Screen.SetResolution(540, 960, FullScreenMode.Windowed);
            }

            if (_startingWidget)
                _spawnedWidget = Instantiate(_startingWidget, _spawnTransform);
        }
    }
}

