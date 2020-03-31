﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Cyberevolver.Unity;
using Cyberevolver;

namespace Cyberevolver.EditorUnity
{
    
    public abstract class BaseValueDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
           
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_Value"),label);         
            EditorGUI.EndProperty();
        }
    }
    [CustomPropertyDrawer(typeof(Cint))]
    public class CintDrawer: BaseValueDrawer { }
    [CustomPropertyDrawer(typeof(Percent))]
    public class PercentDrawer: BaseValueDrawer { }
  


}
   

