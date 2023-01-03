namespace Unit
{
    public class UnitBase : Unit
    {
        protected virtual void Init()
        {
            // Add Behaviours
        }
        protected virtual void Awake()
        {
            Init();
            foreach (var behaviour in behaviours.Values)
            {
                behaviour.Awake();
            }
        }
        
        protected virtual void Start()
        {
            foreach (var behaviour in behaviours.Values)
            {
                behaviour.Start();
            }
        }
        
        protected virtual void Update()
        {
            foreach (var behaviour in behaviours.Values)
            {
                behaviour.Update();
            }
        }
        
        protected virtual void LateUpdate()
        {
            foreach (var behaviour in behaviours.Values)
            {
                behaviour.LateUpdate();
            }
        }
        
        protected virtual void OnDisable()
        {
            foreach (var behaviour in behaviours.Values)
            {
                behaviour.OnDisable();
            }
        }
        
    }
}