using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Assets.Scripts
{
    public class ControllerSliderPanel_Script : MonoBehaviour
    {
        [SerializeField] private MySlider_Script _stabilizeCubeRef;
        [SerializeField] private MySlider_Script _timeSpeedUpRef;
        [SerializeField] private MySlider_Script _dayRef;

        [SerializeField, Range(0, 60)] private float _secMulti = 0;

        public UnityEvent EndSessionEvent;
        public UnityEvent EndDayEvent;
        public static event Action<string> LogEvent;

        public float TimeSpeedUp
        {
            get { return _timeSpeedUpRef.CurrenValue; }
            set
            {
                _timeSpeedUpRef.CurrenValue = value;
                _secMulti = 1 + (value);
            }
        }

        public float CurrentSec { get => _dayRef.CurrenValue; set => _dayRef.CurrenValue = value; }
        public float SecPerDay { get => _dayRef.MaxValue; set => _dayRef.MaxValue = value; }

        public void FixedUpdate()
        {
            CurrentSec += _secMulti * Time.fixedDeltaTime;
            if (CurrentSec >= SecPerDay)
            {
                CurrentSec = 0;
                if (_stabilizeCubeRef.CurrenValue > 0)
                    _stabilizeCubeRef.CurrenValue -= 1;
                if (TimeSpeedUp > 0)
                    TimeSpeedUp -= 1;

                if (EndDayEvent.GetPersistentEventCount() > 0)
                    EndDayEvent.Invoke();


                if (_stabilizeCubeRef.CurrenValue <= 0)
                {
                   
                    SessionEnd();
                }
            }
        }
        public void StabilizeUp()
        {
            _stabilizeCubeRef.CurrenValue += 5;
        }

        public void SpeedUp()
        {
            TimeSpeedUp += 10;

        }
        public void SessionStart()
        {
            _stabilizeCubeRef.CurrenValue = _stabilizeCubeRef.MaxValue;
            _secMulti = 1;
            CurrentSec = 0;
        }

        public void SessionEnd()
        {
            if (EndSessionEvent.GetPersistentEventCount() > 0)
                EndSessionEvent.Invoke();

            LogEvent?.Invoke("Cube Destroyed");
            TimeSpeedUp = 0;
            _secMulti = 0;
        }
    }
}
