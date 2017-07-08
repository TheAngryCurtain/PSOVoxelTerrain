using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadedJob
{
    private bool _isDone = false;
    private object _handle = new object();
    private System.Threading.Thread _thread = null;

    public bool IsDone
    {
        get
        {
            bool temp;
            lock (_handle) { temp = _isDone; }
            return temp;
        }

        set
        {
            lock (_handle) { _isDone = value; }
        }
    }

    public virtual void Start()
    {
        _thread = new System.Threading.Thread(Run);
        _thread.Start();

        _isDone = false;
    }

    public virtual void Abort()
    {
        _thread.Abort();
    }

    public virtual void OnFinished()
    {

    }

    // threaded task. Don't use Unity API here
    protected virtual void ThreadFunction() { }

    public virtual bool Update()
    {
        if (IsDone)
        {
            OnFinished();
            return true;
        }

        return false;
    }

    private void Run()
    {
        ThreadFunction();
        IsDone = true;
    }
}
