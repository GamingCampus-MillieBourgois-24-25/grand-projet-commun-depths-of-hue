//using UnityEngine;

//public abstract class Enigme : MonoBehaviour
//{
//    public bool isResolved = false;
//    public float timeLimit = 60f;
//    protected float timer;

//    public System.Action OnSuccess;
//    public System.Action OnFail;

//    public virtual void Initialize()
//    {
//        timer = timeLimit;
//    }

//    public virtual void UpdateEnigme(float deltaTime)
//    {
//        if (isResolved) return;

//        timer -= deltaTime;
//        if (timer <= 0f)
//        {
//            Fail();
//        }
//    }

//    protected virtual void Success()
//    {
//        isResolved = true;
//        OnSuccess?.Invoke();
//    }

//    protected virtual void Fail()
//    {
//        OnFail?.Invoke();
//    }
//}
