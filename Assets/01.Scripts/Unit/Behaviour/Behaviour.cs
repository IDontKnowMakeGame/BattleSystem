namespace Unit
{
    public class Behaviour
    {
        // The Unit that this behaviour belongs to
        protected Unit thisBase = null;
        
        // This Behaviour is enabled
        public bool IsEnabled { get; set; }

        public virtual void OnEnable()
        {
            // Do stuff when the Base is disabled
        }
        public virtual void Awake()
        {
            if (IsEnabled)
            {
                // Init stuff
            }
        }
        
        public virtual void Start()
        {
            if (IsEnabled)
            {
                // Cashe stuff
            }
        }
        
        public virtual void Update()
        {
            if (IsEnabled)
            {
                // Update stuff
            }
        }
        
        public virtual void LateUpdate()
        {
            if (IsEnabled)
            {
                // Update stuff after Update()
            }
        }
        
        public virtual void OnDisable()
        {
            // Do stuff when the Base is disabled
        }
        
        public void SetBase(Unit _base)
        {
            thisBase = _base;
        }
    }
}