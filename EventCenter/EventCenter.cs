using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 通用事件中心
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventCenter<T> {
    protected Dictionary<T, Delegate> _EventList = new Dictionary<T, Delegate> ();
    protected Dictionary<T, Delegate> _OnceEventList = new Dictionary<T, Delegate> ();

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args"></param>
    public void Emit (T key, params object[] args) {
        if (_EventList.ContainsKey (key)) {
            Delegate list;
            bool result = _EventList.TryGetValue (key, out list);
            if (result) {
                if (list != null) {
                    list.DynamicInvoke (args);
                }
            }
        }

        // 一次监听
        if (_OnceEventList.ContainsKey (key)) {
            Delegate list;
            bool result = _OnceEventList.TryGetValue (key, out list);
            if (result) {
                if (list != null) {
                    list.DynamicInvoke (args);
                    _OnceEventList.Remove (key);
                }
            }
        }
    }

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    public void On (T key, Delegate callback) {
        if (_EventList.ContainsKey (key)) {
            Delegate list;
            bool result = _EventList.TryGetValue (key, out list);
            if (result) {
                if (list != null) {
                    list = Delegate.Combine (list, callback);
                } else {
                    list = callback;
                }
                _EventList.Remove (key);
                _EventList.Add (key, list);
            }
        } else {
            _EventList.Add (key, callback);
        }
    }

    /// <summary>
    /// 监听一次消息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    public void Once (T key, Delegate callback) {
        if (_OnceEventList.ContainsKey (key)) {
            Delegate list;
            bool result = _OnceEventList.TryGetValue (key, out list);
            if (result) {
                if (list != null) {
                    list = Delegate.Combine (list, callback);
                } else {
                    list = callback;
                }
                _OnceEventList.Remove (key);
                _OnceEventList.Add (key, list);
            }
        } else {
            _OnceEventList.Add (key, callback);
        }
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    public void Off (T key, Delegate callback) {
        if (_EventList.ContainsKey (key)) {
            Delegate list;
            bool result = _EventList.TryGetValue (key, out list);
            if (result && list != null) {
                list = Delegate.Remove (list, callback);
                _EventList.Remove (key);
                if (list != null) {
                    _EventList.Add (key, list);
                }
            }
        }

        if (_OnceEventList.ContainsKey (key)) {
            Delegate list;
            bool result = _OnceEventList.TryGetValue (key, out list);
            if (result && list != null) {
                list = Delegate.Remove (list, callback);
                _OnceEventList.Remove (key);
                if (list != null) {
                    _OnceEventList.Add (key, list);
                }
            }
        }
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="key"></param>
    public void Off (T key) {
        if (_EventList.ContainsKey (key)) {
            _EventList.Remove (key);
        }

        if (_OnceEventList.ContainsKey (key)) {
            _OnceEventList.Remove (key);
        }
    }

    /// <summary>
    /// 重置事件中心
    /// </summary>
    public void Reset () {
        _EventList.Clear ();
        _OnceEventList.Clear ();
    }
}

/// <summary>
/// 事件中心
/// </summary>
public class EventCenter {

    /// <summary>
    /// 事件
    /// </summary>
    public abstract class IEvent { };

    private EventCenter<object> _TypeEvent = new EventCenter<object> ();
    private EventCenter<string> _StringEvent = new EventCenter<string> ();
    private EventCenter<int> _IntEvent = new EventCenter<int> ();

    /// <summary>
    /// 发送消息
    /// </summary>
    public void Emit<T> (T args) where T : IEvent {
        _TypeEvent.Emit (typeof (T), args);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="args"></param>
    public void Emit (string eventName, params object[] args) {
        _StringEvent.Emit (eventName, new object[] { args });
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="eventNumber"></param>
    /// <param name="args"></param>
    public void Emit (int eventNumber, params object[] args) {
        _IntEvent.Emit (eventNumber, new object[] { args });
    }

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    public void On<T> (Action<T> callback) where T : IEvent {
        _TypeEvent.On (typeof (T), callback);
    }

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void On (string eventName, Action<object[]> callback) {
        _StringEvent.On (eventName, callback);
    }

    /// <summary>
    /// 监听消息
    /// </summary>
    /// <param name="eventNumber"></param>
    /// <param name="callback"></param>
    public void On (int eventNumber, Action<object[]> callback) {
        _IntEvent.On (eventNumber, callback);
    }

    /// <summary>
    /// 监听一次消息
    /// </summary>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    public void Once<T> (Action<T> callback) where T : IEvent {
        _TypeEvent.Once (typeof (T), callback);
    }

    /// <summary>
    /// 监听一次消息
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void Once (string eventName, Action<object[]> callback) {
        _StringEvent.Once (eventName, callback);
    }

    /// <summary>
    /// 监听一次消息
    /// </summary>
    /// <param name="eventNumber"></param>
    /// <param name="callback"></param>
    public void Once (int eventNumber, Action<object[]> callback) {
        _IntEvent.Once (eventNumber, callback);
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    public void Off<T> (Action<T> callback) where T : IEvent {
        _TypeEvent.Off (typeof (T), callback);
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public void Off (string eventName, Action<object[]> callback) {
        _StringEvent.Off (eventName, callback);
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="eventNumber"></param>
    /// <param name="callback"></param>
    public void Off (int eventNumber, Action<object[]> callback) {
        _IntEvent.Off (eventNumber, callback);
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Off<T> () where T : IEvent {
        _TypeEvent.Off (typeof (T));
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="eventName"></param>
    public void Off (string eventName) {
        _StringEvent.Off (eventName);
    }

    /// <summary>
    /// 移除监听消息
    /// </summary>
    /// <param name="eventNumber"></param>
    public void Off (int eventNumber) {
        _IntEvent.Off (eventNumber);
    }

    /// <summary>
    /// 对象函数解析执行事件中心方法
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="methodName"></param>
    /// <typeparam name="T"></typeparam>
    private void ByObject<T> (object obj, string methodName) where T : IEvent {
        MethodInfo[] methodInfos = obj.GetType ().GetMethods (BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (MethodInfo method in methodInfos) {
            ParameterInfo[] parameterInfos = method.GetParameters ();
            if (parameterInfos.Length == 1 && parameterInfos[0].ParameterType.IsSubclassOf (typeof (T))) {
                try {
                    Delegate _delegate = Delegate.CreateDelegate (typeof (Action<>).MakeGenericType (parameterInfos[0].ParameterType), obj, method);
                    GetType ().GetMethod (methodName, new Type[] { typeof (Action<IEvent>) }).MakeGenericMethod (parameterInfos[0].ParameterType).Invoke (this, new object[] { _delegate });
                } catch (System.Exception error) {
                    throw error;
                }
            }
        }
    }

    /// <summary>
    /// 对象所有对应事件监听
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public void OnByObject<T> (object obj) where T : IEvent {
        ByObject<T> (obj, "On");
    }

    /// <summary>
    /// 对象所有对应事件取消监听
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public void OffByObject<T> (object obj) where T : IEvent {
        ByObject<T> (obj, "Off");
    }

    /// <summary>
    /// 对象所有对应事件一次监听
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    public void OnceByObject<T> (object obj) where T : IEvent {
        ByObject<T> (obj, "Once");
    }

    /// <summary>
    /// 重置事件中心
    /// </summary>
    public void Reset () {
        _TypeEvent.Reset ();
        _StringEvent.Reset ();
        _IntEvent.Reset ();
    }

}