using UnityEngine;
using UnityEditor;
using System.Collections;
using AdvancedColorPicker;

namespace ColorPickerEditor
{
    [CustomEditor(typeof(ColorSlider)), CanEditMultipleObjects]
    public class ColorSliderInspector : ColorComponentInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            SerializedProperty type = serializedObject.FindProperty("type");

            EditorGUILayout.PropertyField(type);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
