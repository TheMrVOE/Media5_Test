using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tools.Pool
{
    public class Pool<T> where T : Component
    {
        private List<T> _collection = new List<T>();
        private List<T> _collectionActiveElements = new List<T>();
        private string objectResourcesPath;
        private GameObject cloneableObject;
        private GameObject parentPoolGameObject;

        private PoolHelper _poolHelper;
        private PoolHelper poolHelper
        {
            get
            {
                if(_poolHelper==null)
                {
                   _poolHelper = parentPoolGameObject.AddComponent<PoolHelper>();
                }
                return _poolHelper;
            }
        }
        #region //ctors
        public Pool(string poolName,string objectResourcesPath)
        {
            this.objectResourcesPath = objectResourcesPath;
            parentPoolGameObject = new GameObject(poolName);
            FindMainPoolGameObj();
        }
        public Pool(string poolName, GameObject cloneableObject)
        {
            this.cloneableObject = cloneableObject;
            parentPoolGameObject = new GameObject(poolName);
            FindMainPoolGameObj();
        }
        #endregion

        #region//Service
        private void FindMainPoolGameObj()
        {
           var go = GameObject.Find("_Pools");
            if (go == null)
            {
                go = new GameObject("_Pools");
            }
            parentPoolGameObject.transform.SetParent(go.transform);
        }
        #endregion
        public T GetObjectFromPool()
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                if (!_collection[i].gameObject.activeInHierarchy)
                {
                    _collection[i].gameObject.SetActive(true);
                    return _collection[i];
                }
            }

            T go;
            try
            {
                if(cloneableObject!=null)
                {
                    go = MonoBehaviour.Instantiate(cloneableObject).GetComponent<T>();
                }
                else
                {
                    GameObject obj = Resources.Load(objectResourcesPath) as GameObject;
                    go = MonoBehaviour.Instantiate(obj).GetComponent<T>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Pool clone Error: "+e.Message);
                return null;
            }

            _collection.Add(go);
            go.transform.SetParent(parentPoolGameObject.transform);
            return go;
        }
        public List<T> GetActiveElements()
        {
            List<T> activeElements = new List<T>(_collection.Count);
            foreach (var item in _collection)
            {
                if (item.gameObject.activeInHierarchy) activeElements.Add(item);
            }

            activeElements.TrimExcess();
            return activeElements;
        }
        public List<T> GetDeactivatedElements()
        {
            List<T> deactivatedElements = new List<T>(_collection.Count);
            foreach (var item in _collection)
            {
                if (!item.gameObject.activeInHierarchy) deactivatedElements.Add(item);
            }

            deactivatedElements.TrimExcess();
            return deactivatedElements;
        }

        public void Disable(T component)
        {
            component.gameObject.SetActive(false);
        }
        public void DisableAfter(T component, float seconds)
        {
            poolHelper.DisableComponentAfter(component, seconds);
        }
        public void DestroyDisabled()
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                if(!_collection[i].gameObject.activeInHierarchy)
                {
                    var component = _collection[i];
                    _collection.Remove(component);
                    MonoBehaviour.Destroy(component.gameObject);
                    i--;
                }
            }
            _collection.TrimExcess();
        }

        private void SetActive(bool isActive,T item)
        {
            //item.SetActive(isActive);
            // _collectionActiveElements.Add();
            //TODO ADD cashing active elements logic
        }
    }
}