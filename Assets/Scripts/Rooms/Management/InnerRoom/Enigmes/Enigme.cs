using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enigme : MonoBehaviour
{
    protected bool isResolved = false;
    protected bool isStarted = false;



    public System.Action OnSuccess; //Event enigme succeeded
    public System.Action OnFail; // Event enigme failed


    /// <summary>
    /// This function must be called first. It will be used for the enigme setup.
    /// </summary>
    public virtual void Initialize()
    {
        
        isStarted = true;
    }

    public virtual void UpdateEnigme(float deltaTime)
    {
        //Code updated if needed.
    }

    /// <summary>
    /// Enigme succes instructions.
    /// </summary>
    protected virtual void Success()
    {
        isStarted = false;
        isResolved = true;
        OnSuccess?.Invoke();
    }

    

    /// <summary>
    /// Enigme failed instructions.
    /// </summary>
    protected virtual void Fail()
    {
        isStarted = false;
        OnFail?.Invoke();
    }

    /// <summary>
    /// Getter for the resolution state
    /// </summary>
    /// <returns></returns>
    public bool GetIsResolved()
    {
        return isResolved;
    }
}
