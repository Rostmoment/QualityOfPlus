using System;
using UnityEngine;
using UnityEngine.Events;

namespace QualityOfPlus.Helpers
{
    class MonoBehaviourBuilder : MonoBehaviour
    {
        #region Awake
        private UnityAction<MonoBehaviourBuilder> onAwake;
        public MonoBehaviourBuilder SetOnAwake(UnityAction action) => SetOnAwake(x => action.Invoke());
        public MonoBehaviourBuilder SetOnAwake(UnityAction<MonoBehaviourBuilder> action)
        {
            onAwake = action;
            return this;
        }
        private void Awake()
        {
            onAwake?.Invoke(this);
        }
        #endregion

        #region Start
        private UnityAction<MonoBehaviourBuilder> onStart;
        public MonoBehaviourBuilder SetOnStart(UnityAction action) => SetOnStart(x => action.Invoke());
        public MonoBehaviourBuilder SetOnStart(UnityAction<MonoBehaviourBuilder> action)
        {
            onStart = action;
            return this;
        }
        private void Start()
        {
            onStart?.Invoke(this);
        }
        #endregion

        #region Update
        private UnityAction<MonoBehaviourBuilder> onUpdate;
        public MonoBehaviourBuilder SetOnUpdate(UnityAction action) => SetOnUpdate(x => action.Invoke());
        public MonoBehaviourBuilder SetOnUpdate(UnityAction<MonoBehaviourBuilder> action)
        {
            onUpdate = action;
            return this;
        }
        private void Update()
        {
            onUpdate?.Invoke(this);
        }
        #endregion

        #region FixedUpdate
        private UnityAction<MonoBehaviourBuilder> onFixedUpdate;
        public MonoBehaviourBuilder SetOnFixedUpdate(UnityAction action) => SetOnFixedUpdate(x => action.Invoke());
        public MonoBehaviourBuilder SetOnFixedUpdate(UnityAction<MonoBehaviourBuilder> action)
        {
            onFixedUpdate = action;
            return this;
        }
        private void FixedUpdate()
        {
            onFixedUpdate?.Invoke(this);
        }
        #endregion

        #region LateUpdate
        private UnityAction<MonoBehaviourBuilder> onLateUpdate;
        public MonoBehaviourBuilder SetOnLateUpdate(UnityAction action) => SetOnLateUpdate(x => action.Invoke());
        public MonoBehaviourBuilder SetOnLateUpdate(UnityAction<MonoBehaviourBuilder> action)
        {
            onLateUpdate = action;
            return this;
        }
        private void LateUpdate()
        {
            onLateUpdate?.Invoke(this);
        }
        #endregion

        #region OnEnable
        private UnityAction<MonoBehaviourBuilder> onEnable;
        public MonoBehaviourBuilder SetOnEnable(UnityAction action) => SetOnEnable(x => action.Invoke());
        public MonoBehaviourBuilder SetOnEnable(UnityAction<MonoBehaviourBuilder> action)
        {
            onEnable = action;
            return this;
        }
        private void OnEnable()
        {
            onEnable?.Invoke(this);
        }
        #endregion

        #region OnDisable
        private UnityAction<MonoBehaviourBuilder> onDisable;
        public MonoBehaviourBuilder SetOnDisable(UnityAction action) => SetOnDisable(x => action.Invoke());
        public MonoBehaviourBuilder SetOnDisable(UnityAction<MonoBehaviourBuilder> action)
        {
            onDisable = action;
            return this;
        }
        private void OnDisable()
        {
            onDisable?.Invoke(this);
        }
        #endregion

        #region OnDestroy
        private UnityAction<MonoBehaviourBuilder> onDestroy;
        public MonoBehaviourBuilder SetOnDestroy(UnityAction action) => SetOnDestroy(x => action.Invoke());
        public MonoBehaviourBuilder SetOnDestroy(UnityAction<MonoBehaviourBuilder> action)
        {
            onDestroy = action;
            return this;
        }
        private void OnDestroy()
        {
            onDestroy?.Invoke(this);
        }
        #endregion
    }
}
