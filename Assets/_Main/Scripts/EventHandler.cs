using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts {
    abstract class InvokableActionBase {
    }

    class InvokableAction : InvokableActionBase {
        private event Action m_Action;

        public void Initialize(Action action) => this.m_Action = action;

        public void Invoke() => this.m_Action();

        public bool IsAction(Action action) => this.m_Action == action;
    }
    class InvokableAction<T1> : InvokableActionBase {
        private event Action<T1> m_Action;

        public void Initialize(Action<T1> action) => this.m_Action = action;

        public void Invoke(T1 arg1) =>
            this.m_Action(arg1);

        public bool IsAction(Action<T1> action) => this.m_Action == action;
    }

    class InvokableAction<T1, T2> : InvokableActionBase {
        private event Action<T1, T2> m_Action;

        public void Initialize(Action<T1, T2> action) => this.m_Action = action;

        public void Invoke(T1 arg1, T2 arg2) =>
            this.m_Action(arg1, arg2);

        public bool IsAction(Action<T1, T2> action) => this.m_Action == action;
    }

    public static class GenericObjectPool {
        private static Dictionary<Type, object> s_GenericPool = new Dictionary<Type, object>();

        public static T Get<T>() {
            object obj;
            if (GenericObjectPool.s_GenericPool.TryGetValue(typeof(T), out obj)) {
                Stack<T> objStack = obj as Stack<T>;
                if (objStack.Count > 0)
                    return objStack.Pop();
            }

            if (!typeof(T).IsArray)
                return Activator.CreateInstance<T>();
            return (T)Activator.CreateInstance(typeof(T), (object)0);
        }

        public static void Return<T>(T obj) {
            object obj1;
            if (GenericObjectPool.s_GenericPool.TryGetValue(typeof(T), out obj1)) {
                (obj1 as Stack<T>).Push(obj);
            }
            else {
                Stack<T> objStack = new Stack<T>();
                objStack.Push(obj);
                GenericObjectPool.s_GenericPool.Add(typeof(T), (object)objStack);
            }
        }
    }

    public class EventHandler : MonoBehaviour {
        private static Dictionary<object, Dictionary<string, List<InvokableActionBase>>> s_EventTable =
            new Dictionary<object, Dictionary<string, List<InvokableActionBase>>>();

        private static Dictionary<string, List<InvokableActionBase>> s_GlobalEventTable =
            new Dictionary<string, List<InvokableActionBase>>();

        private static void RegisterEvent(string eventName, InvokableActionBase invokableAction) {
            List<InvokableActionBase> invokableActionBaseList;
            if (EventHandler.s_GlobalEventTable.TryGetValue(eventName, out invokableActionBaseList))
                invokableActionBaseList.Add(invokableAction);
            else
                EventHandler.s_GlobalEventTable.Add(eventName, new List<InvokableActionBase>() {
                    invokableAction
                });
        }

        private static void RegisterEvent(
            object obj,
            string eventName,
            InvokableActionBase invokableAction) {
            Dictionary<string, List<InvokableActionBase>> dictionary;
            if (!EventHandler.s_EventTable.TryGetValue(obj, out dictionary)) {
                dictionary = new Dictionary<string, List<InvokableActionBase>>();
                EventHandler.s_EventTable.Add(obj, dictionary);
            }

            List<InvokableActionBase> invokableActionBaseList;
            if (dictionary.TryGetValue(eventName, out invokableActionBaseList))
                invokableActionBaseList.Add(invokableAction);
            else
                dictionary.Add(eventName, new List<InvokableActionBase>() {
                    invokableAction
                });
        }

        private static List<InvokableActionBase> GetActionList(string eventName) {
            List<InvokableActionBase> invokableActionBaseList;
            return EventHandler.s_GlobalEventTable.TryGetValue(eventName, out invokableActionBaseList)
                       ? invokableActionBaseList
                       : (List<InvokableActionBase>)null;
        }

        private static void CheckForEventRemoval(string eventName, List<InvokableActionBase> actionList) {
            if (actionList.Count != 0)
                return;
            EventHandler.s_GlobalEventTable.Remove(eventName);
        }

        private static List<InvokableActionBase> GetActionList(
            object obj,
            string eventName) {
            Dictionary<string, List<InvokableActionBase>> dictionary;
            List<InvokableActionBase> invokableActionBaseList;
            return EventHandler.s_EventTable.TryGetValue(obj, out dictionary) &&
                   dictionary.TryGetValue(eventName, out invokableActionBaseList)
                       ? invokableActionBaseList
                       : (List<InvokableActionBase>)null;
        }

        private static void CheckForEventRemoval(
            object obj,
            string eventName,
            List<InvokableActionBase> actionList) {
            Dictionary<string, List<InvokableActionBase>> dictionary;
            if (actionList.Count != 0 || !EventHandler.s_EventTable.TryGetValue(obj, out dictionary))
                return;
            dictionary.Remove(eventName);
            if (dictionary.Count != 0)
                return;
            EventHandler.s_EventTable.Remove(obj);
        }

        public static void RegisterEvent(string eventName, Action action) {
            InvokableAction invokableAction = GenericObjectPool.Get<InvokableAction>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(eventName, (InvokableActionBase)invokableAction);
        }

        public static void RegisterEvent(object obj, string eventName, Action action) {
            InvokableAction invokableAction = GenericObjectPool.Get<InvokableAction>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(obj, eventName, (InvokableActionBase)invokableAction);
        }

        public static void RegisterEvent<T1>(string eventName, Action<T1> action) {
            InvokableAction<T1> invokableAction = GenericObjectPool.Get<InvokableAction<T1>>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(eventName, (InvokableActionBase)invokableAction);
        }

        public static void RegisterEvent<T1>(object obj, string eventName, Action<T1> action) {
            InvokableAction<T1> invokableAction = GenericObjectPool.Get<InvokableAction<T1>>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(obj, eventName, (InvokableActionBase)invokableAction);
        }

        public static void RegisterEvent<T1, T2>(string eventName, Action<T1, T2> action) {
            InvokableAction<T1, T2> invokableAction = GenericObjectPool.Get<InvokableAction<T1, T2>>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(eventName, (InvokableActionBase)invokableAction);
        }

        public static void RegisterEvent<T1, T2>(object obj, string eventName, Action<T1, T2> action) {
            InvokableAction<T1, T2> invokableAction = GenericObjectPool.Get<InvokableAction<T1, T2>>();
            invokableAction.Initialize(action);
            EventHandler.RegisterEvent(obj, eventName, (InvokableActionBase)invokableAction);
        }

        public static void ExecuteEvent(string eventName) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction).Invoke();
            }
        }

        public static void ExecuteEvent(object obj, string eventName) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction).Invoke();
            }
        }

        public static void ExecuteEvent<T1>(string eventName, T1 arg1) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction<T1>).Invoke(arg1);
            }
        }

        public static void ExecuteEvent<T1>(object obj, string eventName, T1 arg1) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction<T1>).Invoke(arg1);
            }
        }

        public static void ExecuteEvent<T1, T2>(string eventName, T1 arg1, T2 arg2) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction<T1, T2>).Invoke(arg1, arg2);
            }
        }

        public static void ExecuteEvent<T1, T2>(object obj, string eventName, T1 arg1, T2 arg2) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            int num = 0;
            int count = actionList.Count;
            while (num < count) {
                if (count != actionList.Count) {
                    num += actionList.Count - count;
                    count = actionList.Count;
                }

                int index = actionList.Count - num - 1;
                ++num;
                (actionList[index] as InvokableAction<T1, T2>).Invoke(arg1, arg2);
            }
        }

        public static void UnregisterEvent(string eventName, Action action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction invokableAction = actionList[index] as InvokableAction;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(eventName, actionList);
        }

        public static void UnregisterEvent(object obj, string eventName, Action action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction invokableAction = actionList[index] as InvokableAction;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(obj, eventName, actionList);
        }

        public static void UnregisterEvent<T1>(string eventName, Action<T1> action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction<T1> invokableAction = actionList[index] as InvokableAction<T1>;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction<T1>>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(eventName, actionList);
        }

        public static void UnregisterEvent<T1>(object obj, string eventName, Action<T1> action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction<T1> invokableAction = actionList[index] as InvokableAction<T1>;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction<T1>>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(obj, eventName, actionList);
        }

        public static void UnregisterEvent<T1, T2>(string eventName, Action<T1, T2> action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction<T1, T2> invokableAction = actionList[index] as InvokableAction<T1, T2>;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction<T1, T2>>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(eventName, actionList);
        }

        public static void UnregisterEvent<T1, T2>(object obj, string eventName, Action<T1, T2> action) {
            List<InvokableActionBase> actionList = EventHandler.GetActionList(obj, eventName);
            if (actionList == null)
                return;
            for (int index = 0; index < actionList.Count; ++index) {
                InvokableAction<T1, T2> invokableAction = actionList[index] as InvokableAction<T1, T2>;
                if (invokableAction.IsAction(action)) {
                    GenericObjectPool.Return<InvokableAction<T1, T2>>(invokableAction);
                    actionList.RemoveAt(index);
                    break;
                }
            }

            EventHandler.CheckForEventRemoval(obj, eventName, actionList);
        }

        private void OnDisable() {
            if ((UnityEngine.Object)this.gameObject != (UnityEngine.Object)null && !this.gameObject.activeSelf)
                return;
            this.ClearTable();
        }

        private void OnDestroy() => this.ClearTable();

        private void ClearTable() => EventHandler.s_EventTable.Clear();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void DomainReset() {
            if (EventHandler.s_EventTable != null)
                EventHandler.s_EventTable.Clear();
            if (EventHandler.s_GlobalEventTable == null)
                return;
            EventHandler.s_GlobalEventTable.Clear();
        }
    }
}