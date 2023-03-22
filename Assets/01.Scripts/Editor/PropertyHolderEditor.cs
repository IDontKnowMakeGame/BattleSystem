using System.Collections.Generic;
using System.Runtime.InteropServices;
using AI;
using DG.DemiEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
 
 [CustomEditor(typeof(AiConditionHolder), true)]
 [CanEditMultipleObjects]
 public class PropertyHolderEditor : Editor 
 {
     ReorderableList reorderableList;

     void OnEnable ()
     {
         var target = serializedObject.FindProperty("Target");
         var goal = serializedObject.FindProperty("Goal");
         var prop = serializedObject.FindProperty ("Conditions");

         reorderableList = new ReorderableList (serializedObject, prop);
         reorderableList.elementHeight = 68;
         reorderableList.drawElementCallback =
             (rect, index, isActive, isFocused) => {
                 var element = prop.GetArrayElementAtIndex (index);
                 rect.height -= 4;
                 rect.y += 2;
                 EditorGUI.PropertyField (rect, element);
             };

         var defaultColor = GUI.backgroundColor;

         reorderableList.drawHeaderCallback = (rect) =>
             EditorGUI.LabelField (rect, prop.displayName);
         reorderableList.drawElementCallback = (rect, index, active, focused) =>
         {
             rect.y += 2; // 위쪽 패딩
             EditorGUI.PropertyField(
                 new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - 50, EditorGUIUtility.singleLineHeight),
                 target, new GUIContent("From"));
             rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
             EditorGUI.PropertyField(
                 new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - 50, EditorGUIUtility.singleLineHeight),
                 goal, new GUIContent("To"));
             rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
             SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
             rect.y += 2; // 위쪽 패딩
             EditorGUI.PropertyField(
                 new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - 50, EditorGUIUtility.singleLineHeight),
                 element.FindPropertyRelative("Type"), new GUIContent("Condition"));
             rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
             EditorGUI.PropertyField(
                 new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                 element.FindPropertyRelative("IsNeeded"), new GUIContent("Is Needed"));
             rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
             EditorGUI.PropertyField(
                 new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                 element.FindPropertyRelative("IsNegative"), new GUIContent("Is Negative"));

             var conditionType = (Condition)element.FindPropertyRelative("Type").enumValueIndex;
             
             rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
             switch (conditionType)
             {
                 case Condition.TimeCondition:
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("floatParam"), new GUIContent("Goal Time"));
                     break;
                 case Condition.DistanceCondition:
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("intParam"), new GUIContent("Goal Distance"));
                     break;
                 case Condition.BesideCondition:
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("actorParam"), new GUIContent("Target"));
                     break;
                 case Condition.AttackCondition:
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("actorParam"), new GUIContent("This Actor"));
                     break;
                 case Condition.LifeCondition:
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("actorParam"), new GUIContent("This Actor"));
                     rect.y += EditorGUIUtility.singleLineHeight + 2; // 라인 간격
                     EditorGUI.PropertyField(
                         new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight),
                         element.FindPropertyRelative("floatParam"), new GUIContent("Goal Perceent"));
                     break;
             }
         };
         reorderableList.elementHeight = 150;
     }

     public override void OnInspectorGUI ()
     {
         serializedObject.Update ();
         reorderableList.DoLayoutList ();
         serializedObject.ApplyModifiedProperties ();
     }
 }