﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tuner.Event
{
    [Serializable]
    class DelayEvent
    {
        [SerializeField] public int Id;
        [SerializeField] public System.Object Value;
        [SerializeField] public float TriggerTime;
    }

    public delegate void EventCallBack(int id, System.Object value);

    public class EventMgr : Singleton<EventMgr>
    {
        public List<string> eventCallList = new List<string>();
        Dictionary<int, List<EventCallBack>> mEventCallBackMap = new Dictionary<int, List<EventCallBack>>();
        SortedDictionary<float, DelayEvent> mDeLayEventMap = new SortedDictionary<float, DelayEvent>();

        public void addEventCallBack(int id, EventCallBack callback)
        {
            eventCallList.Add(id.ToString() + " => " +callback.Target.GetType() + "." +   callback.Method.Name);
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList == null)
            {
                tempCallbackList = new List<EventCallBack>();
                mEventCallBackMap.Add(id, tempCallbackList);
            }

            tempCallbackList.Add(callback);
        }

        public void removeEventListener(int id, EventCallBack callback)
        {
            eventCallList.Remove(id.ToString() + "__" + callback.ToString());
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList != null)
            {
                tempCallbackList.Remove(callback);
            }
        }

        public void DispatchEvent(int id, System.Object value)
        {
            List<EventCallBack> tempCallbackList = null;
            mEventCallBackMap.TryGetValue(id, out tempCallbackList);
            if (tempCallbackList != null)
            {
                foreach (EventCallBack item in tempCallbackList)
                {
                    item.Invoke(id, value);
                }
            }
        }

        public void DelayDispatchEvent(float delayTime, int id, System.Object value)
        {
            DelayEvent tempDelayEvent = new DelayEvent();
            tempDelayEvent.Id = id;
            tempDelayEvent.Value = value;
            tempDelayEvent.TriggerTime = delayTime + UnityEngine.Time.time;
            mDeLayEventMap.Add(tempDelayEvent.TriggerTime, tempDelayEvent);
        }

        public void Update()
        {
            foreach (KeyValuePair<float, DelayEvent> item in mDeLayEventMap)
            {
                if (UnityEngine.Time.time > item.Key)
                {
                    DispatchEvent(item.Value.Id, item.Value.Value);
                }
            }
        }
    }
}