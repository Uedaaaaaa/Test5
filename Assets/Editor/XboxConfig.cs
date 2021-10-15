using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class XboxConfig { 

    static XboxConfig() {

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        SerializedProperty childElement;

        if (axesProperty.arraySize > 18) {
            return;
        }


        int count = axesProperty.arraySize;
        axesProperty.arraySize += 18; //さらに18個を追加する
        serializedObject.ApplyModifiedProperties();

        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "HorizontalR";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 3; // 4th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "VerticalR";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = true; // Invert
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 4; // 5th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "HorizontalPad";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 5; // 6th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "VerticalPad";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 6; // 7th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "TriggerL";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 8; // 9th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "TriggerR";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "";
        GetChildProperty(childElement, "gravity").floatValue = 0f;
        GetChildProperty(childElement, "dead").floatValue = .5f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 2;
        GetChildProperty(childElement, "axis").intValue = 9; // 10th axis
        GetChildProperty(childElement, "joyNum").intValue = 0;


        count++;
        childElement = axesProperty.GetArrayElementAtIndex( count );
        GetChildProperty( childElement, "m_Name" ).stringValue = "TriggerL"; //キーボードでTriggerL
        GetChildProperty( childElement, "descriptiveName" ).stringValue = "";
        GetChildProperty( childElement, "descriptiveNegativeName" ).stringValue = "";
        GetChildProperty( childElement, "negativeButton" ).stringValue = "";
        GetChildProperty( childElement, "positiveButton" ).stringValue = "left alt";
        GetChildProperty( childElement, "altNegativeButton" ).stringValue = "";
        GetChildProperty( childElement, "altPositiveButton" ).stringValue = "";
        GetChildProperty( childElement, "gravity" ).floatValue = 3f;
        GetChildProperty( childElement, "dead" ).floatValue = .001f;
        GetChildProperty( childElement, "sensitivity" ).floatValue = 3f;
        GetChildProperty( childElement, "snap" ).boolValue = true;
        GetChildProperty( childElement, "invert" ).boolValue = false;
        GetChildProperty( childElement, "type" ).intValue = 0;
        GetChildProperty( childElement, "axis" ).intValue = 0; 
        GetChildProperty( childElement, "joyNum" ).intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex( count );
        GetChildProperty( childElement, "m_Name" ).stringValue = "TriggerR"; //キーボードでTriggerR
        GetChildProperty( childElement, "descriptiveName" ).stringValue = "";
        GetChildProperty( childElement, "descriptiveNegativeName" ).stringValue = "";
        GetChildProperty( childElement, "negativeButton" ).stringValue = "";
        GetChildProperty( childElement, "positiveButton" ).stringValue = "right alt";
        GetChildProperty( childElement, "altNegativeButton" ).stringValue = "";
        GetChildProperty( childElement, "altPositiveButton" ).stringValue = "";
        GetChildProperty( childElement, "gravity" ).floatValue = 3f;
        GetChildProperty( childElement, "dead" ).floatValue = .001f;
        GetChildProperty( childElement, "sensitivity" ).floatValue = 3f;
        GetChildProperty( childElement, "snap" ).boolValue = true;
        GetChildProperty( childElement, "invert" ).boolValue = false;
        GetChildProperty( childElement, "type" ).intValue = 0;
        GetChildProperty( childElement, "axis" ).intValue = 0;
        GetChildProperty( childElement, "joyNum" ).intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnA";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 0";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "mouse 0";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnB";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 1";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "mouse 1";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnX";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 2";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "mouse 2";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnY";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 3";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "y";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BumperL";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 4";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "l";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BumperR";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 5";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "r";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnBack";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 6";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "k";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "BtnStart";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 7";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "t";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "JoyBtnL";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 8";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "left shift";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        count++;
        childElement = axesProperty.GetArrayElementAtIndex(count);
        GetChildProperty(childElement, "m_Name").stringValue = "JoyBtnR";
        GetChildProperty(childElement, "descriptiveName").stringValue = "";
        GetChildProperty(childElement, "descriptiveNegativeName").stringValue = "";
        GetChildProperty(childElement, "negativeButton").stringValue = "";
        GetChildProperty(childElement, "positiveButton").stringValue = "joystick button 9";
        GetChildProperty(childElement, "altNegativeButton").stringValue = "";
        GetChildProperty(childElement, "altPositiveButton").stringValue = "right shift";
        GetChildProperty(childElement, "gravity").floatValue = 1000f;
        GetChildProperty(childElement, "dead").floatValue = 0.001f;
        GetChildProperty(childElement, "sensitivity").floatValue = 1000f;
        GetChildProperty(childElement, "snap").boolValue = false;
        GetChildProperty(childElement, "invert").boolValue = false;
        GetChildProperty(childElement, "type").intValue = 0; // Key or Mouse Button
        GetChildProperty(childElement, "axis").intValue = 0; // X axis
        GetChildProperty(childElement, "joyNum").intValue = 0;

        serializedObject.ApplyModifiedProperties();

    }

    public static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
        SerializedProperty copiedProperty = parent.Copy();
        bool moreChildren = true;
        copiedProperty.Next(true);
        while (moreChildren)  {
            if (copiedProperty.name.Equals(name)) {
                return copiedProperty;
            }
            moreChildren = copiedProperty.Next(false);
        }
        return null;
    }
}