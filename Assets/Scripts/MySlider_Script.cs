using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MySlider_Script : MonoBehaviour
    {
        [SerializeField] public Slider slider;
        public float CurrenValue
        {
            get
            {
                return slider.value;
            }
            set
            {
                slider.value = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return slider.maxValue;
            }
            set 
            { 
                slider.maxValue = value; 
            }
        }
    }
}
