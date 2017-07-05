using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadedJob
{
    //public System.Action<object> OnJobComplete;
    public System.Action OnJobComplete;

    //private bool _isDone = false;
    private object _handle = new object();
    private System.Threading.Thread _thread = null;

    //protected object _data = null;

    //public bool IsDone
    //{
    //    get
    //    {
    //        bool temp;
    //        lock (_handle) { temp = _isDone; }
    //        return temp;
    //    }

    //    set
    //    {
    //        lock (_handle) { _isDone = value; }
    //    }
    //}

    public virtual void Start()
    {
        _thread = new System.Threading.Thread(Run);
        _thread.Start();
    }

    public virtual void Abort()
    {
        _thread.Abort();
    }

    // threaded task. Don't use Unity API here
    protected virtual void ThreadFunction() { }

    //public virtual bool Update()
    //{
    //    if (IsDone)
    //    {
    //        if (OnJobComplete != null)
    //        {
    //            OnJobComplete(_data);
    //        }
    //        return true;
    //    }

    //    return false;
    //}

    private void Run()
    {
        ThreadFunction();
        //OnJobComplete(_data);
        OnJobComplete();
        //IsDone = true;
    }
}
