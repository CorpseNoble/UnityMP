using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Assets.Scripts
{
    public class ControllerButtonPanel_Script : MonoBehaviour
    {
        [SerializeField] private MyButton_Script _prefabMyButton;
        [SerializeField] private MyLabel_Script _prefabMyTitle;
        [SerializeField] private GameObject _cubeRef;

        [SerializeField] private string _dayTitle = "Day: ";
        [SerializeField] private string _startUpButTitle = "Start Session";
        [SerializeField] private string _stabilizeButTitle = "Stabilize Cube";
        [SerializeField] private string _speedUpButTitle = "Speed Up Time";
        [SerializeField] private string _endButTitle = "End Session";
        [SerializeField, Range(1, 10)] private int _stabilizeUseDelay = 1;
        [SerializeField, Min(0)] private int _stabilizeCurrentDelay = 0;
        [SerializeField, Range(1, 10)] private int _speedUpUseDelay = 5;
        [SerializeField, Min(0)] private int _speedUpCurrentDelay = 0;
        private Transform _myTransform;
        private Text _dayText;
        private MyButton_Script _stabilizeBut;
        private MyButton_Script _speedUpBut;

        private int _currentDay = 0;
        public int CurrentDay
        {
            get => _currentDay;
            set
            {
                if (value < 0)
                    value = 0;

                if (_currentDay == value)
                    return;

                _currentDay = value;
                _dayText.text = _dayTitle + value;
                ButtonDelayCheck();
            }
        }

        public UnityEvent StartSessionEvent;
        public UnityEvent EndSessionEvent;
        public UnityEvent StabilizeCubeEvent;
        public UnityEvent SpeedUpTimeEvent;
        public static event Action<string> LogEvent;

        private void Start()
        {
            _myTransform = this.transform;

            if (PlayerPrefsManager.Session)
            {
                Sesion_Continue();
            }
            else
            {
                Sesion_StartUp();
            }
        }
        private void ButtonDelayCheck()
        {
            _speedUpCurrentDelay--;
            _stabilizeCurrentDelay--;


            if (_speedUpCurrentDelay <= 0)
            {
                if (!_speedUpBut.button.interactable)
                    LogEvent?.Invoke("Room Speed Up Ready to Use");

                _speedUpBut.button.interactable = true;
            }
            else
            {
                LogEvent?.Invoke(string.Format("Room Speed Up Delay {0} Day", _speedUpCurrentDelay));
            }



            if (_stabilizeCurrentDelay <= 0)
            {
                if (!_stabilizeBut.button.interactable)
                    LogEvent?.Invoke("Cube Stabilize Ready to Use");

                _stabilizeBut.button.interactable = true;

            }
            else
            {
                LogEvent?.Invoke(string.Format("Cube Stabilized Delay {0} Day", _stabilizeUseDelay));
            }

        }
        private void DestroyAllChild(Transform transform)
        {
            for (int x = 0; x < transform.childCount; x++)
            {
                Destroy(transform.GetChild(x).gameObject);
            }
            var myTitle = Instantiate(_prefabMyTitle, _myTransform);
            _dayText = myTitle.text;
        }

        private void Sesion_StartUp()
        {
            DestroyAllChild(_myTransform);
            CurrentDay = 0;

            SetUpButton(_startUpButTitle, StartSession_Click);
        }

        private MyButton_Script SetUpButton(string buttonText, Action action)
        {
            var myButton = Instantiate(_prefabMyButton, _myTransform);
            myButton.text.text = buttonText;
            myButton.button.onClick.AddListener(() => action.Invoke());

            return myButton;
        }

        private void Sesion_Continue()
        {
            DestroyAllChild(_myTransform);

            _stabilizeBut = SetUpButton(_stabilizeButTitle, StabilizeCube_Click);
            _speedUpBut = SetUpButton(_speedUpButTitle, SpeedUpTime_Click);
            SetUpButton(_endButTitle, EndSession_Click);
        }

        private void StartSession_Click()
        {
            PlayerPrefsManager.Session = true;
            Sesion_Continue();

            _cubeRef.SetActive(true);
            if (StartSessionEvent.GetPersistentEventCount() > 0)
                StartSessionEvent.Invoke();

            LogEvent?.Invoke("Unduracted Cube Analize Start");
        }

        private void StabilizeCube_Click()
        {
            _stabilizeCurrentDelay = _stabilizeUseDelay;
            _stabilizeBut.button.interactable = false;
            if (StabilizeCubeEvent.GetPersistentEventCount() > 0)
                StabilizeCubeEvent.Invoke();

            LogEvent?.Invoke(string.Format("Cube Stabilized Delay {0} Day", _stabilizeUseDelay));
        }

        private void SpeedUpTime_Click()
        {
            _speedUpCurrentDelay = _speedUpUseDelay;
            _speedUpBut.button.interactable = false;
            if (SpeedUpTimeEvent.GetPersistentEventCount() > 0)
                SpeedUpTimeEvent.Invoke();

            LogEvent?.Invoke(string.Format("Room Speed Up Delay {0} Day", _speedUpCurrentDelay));
        }

        public void EndSession_Click()
        {
            if (EndSessionEvent.GetPersistentEventCount() > 0)
                EndSessionEvent.Invoke();
        }

        public void SessionEnd()
        {
            LogEvent?.Invoke(string.Format("Day Passed {0}", CurrentDay));

            PlayerPrefsManager.Session = false;
            Sesion_StartUp();

            _cubeRef.SetActive(false);
        }

        public void EndDay()
        {
            CurrentDay++;
        }
    }
}
