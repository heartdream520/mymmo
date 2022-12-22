using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    void Awake()
    {
        if(global)
        {
            if(instance!=null && instance!=this.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;

            }
            instance = this.GetComponent<T>();
            DontDestroyOnLoad(this.gameObject);
        }
        this.OnAwake();
        
        
    }
    private void Start()
    {
        this.OnStart();
    }
    public virtual void OnStart()
    {

    }
    public virtual void OnAwake()
    {

    }
}