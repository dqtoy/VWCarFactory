using UnityEngine;
using UnityEngine.UI;
using System;

namespace AdvancedColorPicker
{
    /// <summary>
    /// A slider that can edit one of the color values of a ColorPicker
    /// </summary>
    [RequireComponent(typeof(Slider)), ExecuteInEditMode]
    public class ColorSlider : ColorTypeComponent
    {
        private Slider slider;
        private bool dontListenToSlider;

        protected override void Awake()
        {
            base.Awake();
            slider = GetComponent<Slider>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            DisplayNewColor();

            slider.onValueChanged.AddListener(SliderChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            slider.onValueChanged.RemoveListener(SliderChanged);
        }

        private void SliderChanged(float newValue)
        {
            if (!isActiveAndEnabled)
                return;

            if (Picker != null && !dontListenToSlider)
            {
                Picker.SetValueNormalized(Type, slider.normalizedValue);
            }
        }

        protected override void DisplayNewColor()
        {
            if (!isActiveAndEnabled)
                return;

            float newValue = Picker != null ? Picker.GetValueNormalized(Type) : 0;

            if (!Mathf.Approximately(newValue, slider.normalizedValue))
            {
                dontListenToSlider = true;
                slider.normalizedValue = newValue;
                dontListenToSlider = false;
            }
        }
    }
}