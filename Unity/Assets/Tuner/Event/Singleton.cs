/*
   Tuner Data -  Read Static Data in Game Development.
   e-mail : dongliang17@126.com
*/

using System.Diagnostics;
using UnityEngine;

namespace Tuner
{
    public class Singleton<T> :MonoBehaviour where T : MonoBehaviour
    {
        protected Singleton()
        {
            //Debug.Assert(null == instance);
        }

        protected static T me;

        public static T Me
        {
            get
            {
                if (me == null)
                {
                    me = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                return me;
            }
        }
    }
}