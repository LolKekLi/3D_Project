﻿#if !NOT_UNITY3D

#region

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;
using Object = UnityEngine.Object;

#endregion

namespace Zenject
{
    [NoReflectionBaking]
    public class ScriptableObjectInstanceProvider : IProvider
    {
        readonly DiContainer _container;
        readonly Type _resourceType;
        readonly List<TypeValuePair> _extraArguments;
        readonly bool _createNew;
        readonly object _concreteIdentifier;
        readonly Action<InjectContext, object> _instantiateCallback;
        readonly Object _resource;

        public ScriptableObjectInstanceProvider(
            Object resource, Type resourceType,
            DiContainer container, IEnumerable<TypeValuePair> extraArguments,
            bool createNew, object concreteIdentifier,
            Action<InjectContext, object> instantiateCallback)
        {
            _container = container;
            Assert.DerivesFromOrEqual<ScriptableObject>(resourceType);

            _resource = resource;
            _extraArguments = extraArguments.ToList();
            _resourceType = resourceType;
            _createNew = createNew;
            _concreteIdentifier = concreteIdentifier;
            _instantiateCallback = instantiateCallback;
        }

        public bool IsCached
        {
            get { return false; }
        }

        public bool TypeVariesBasedOnMemberType
        {
            get { return false; }
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _resourceType;
        }

        public void GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args, out Action injectAction, List<object> buffer)
        {
            Assert.IsNotNull(context);

            if (_createNew)
            {
                buffer.Add(ScriptableObject.Instantiate(_resource));
            }
            else
            {
                buffer.Add(_resource);
            }

            injectAction = () =>
            {
                for (int i = 0; i < buffer.Count; i++)
                {
                    var obj = buffer[i];

                    var extraArgs = ZenPools.SpawnList<TypeValuePair>();

                    extraArgs.AllocFreeAddRange(_extraArguments);
                    extraArgs.AllocFreeAddRange(args);

                    _container.InjectExplicit(
                        obj, _resourceType, extraArgs, context, _concreteIdentifier);

                    ZenPools.DespawnList(extraArgs);

                    if (_instantiateCallback != null)
                    {
                        _instantiateCallback(context, obj);
                    }
                }
            };
        }
    }
}

#endif
