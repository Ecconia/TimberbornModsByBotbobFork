using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockSystem;
using Timberborn.PrefabSystem;
using TimberbornMods;
using UnityEngine.InputSystem;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  public class CopySettingsUtil
  {
    /**
     *  Given two objects with the same component, this class allows you to only define the copying process, what settings to copy, which methods to call etc.
     */
    public class CopyFunctionWrapper: IBotBobPluginLogger
    {
      private readonly BlockObject _savedObject; // object we will be copying from
      private readonly BlockObject _targetObject; // object we will copy settings to
      private Nullable<bool> _objectsValid = null;
      private BaseComponent _savedComponent = null;
      private BaseComponent _targetComponent = null;
      private Nullable<bool> _componentsValid = null;

      public CopyFunctionWrapper(BlockObject savedObject, BlockObject targetObject)
      {
        _savedObject = savedObject;
        _targetObject = targetObject;
      }

      public bool ValidateBlockObjects()
      {
        if (_savedObject == null || _targetObject == null)
        {
          LogWarn("Saved or target object missing, saved='{0}' target='{1}'", _savedObject, _targetObject);
          return false;
        }

        // check that objects are of same type
        Prefab savedBuildingPrefab = _savedObject.GetComponentFast<Prefab>();
        Prefab targetPrefab = _targetObject.GetComponentFast<Prefab>();
        if (!savedBuildingPrefab.IsNamed(targetPrefab.PrefabName) && !Keyboard.current.shiftKey.isPressed)
        {
          LogDebug(savedBuildingPrefab.PrefabName + " != " + targetPrefab.PrefabName);
          return false;
        }

        return true;
      }

      private bool ValidateComponents<T>() where T : BaseComponent
      {
        // get components
        Type _componentType = typeof(T);
        _savedComponent = _savedObject.GetComponentFast<T>();
        _targetComponent = _targetObject.GetComponentFast<T>();
        if (_savedComponent == null && _targetComponent == null)
        {
          LogTrace("< > Objects don't have component type '{0}'", _componentType);
          return false;
        }
        if (_savedComponent == null)
        {
          LogTrace("< > Saved object doesnt't have component type '{0}'", _componentType);
          return false;
        }
        if (_targetComponent == null)
        {
          LogTrace("< > Target object doesnt't have component type '{0}'", _componentType);
          return false;
        }

        return true;
      }

      // define which type of component we will be copying, by providing T
      // returns a method that accepts a copy function as a parameter
      // (copy function being a function that copies settings from one component to some other component)
      public Action<Action<T, T>> WithComponentType<T>() where T : BaseComponent
      {
        _componentsValid = null;
        return WithValidation;
      }


      private void WithValidation<T>(Action<T, T> copyFunction) where T : BaseComponent
      {

        // objects
        _objectsValid ??= ValidateBlockObjects();
        if(!_objectsValid ?? !false)
        {
          return;
        }

        // components
        Type _componentType = typeof(T);
        _componentsValid ??= ValidateComponents<T>();
        if (!_componentsValid ?? !false)
        {
          return;
        }

        // run the copy function on components
        copyFunction((T)_savedComponent, (T)_targetComponent);
        LogDebug("<x> Component '{0}' copied.", _componentType);

      }
    }
  }
}