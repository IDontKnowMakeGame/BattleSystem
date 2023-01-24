namespace Units.Behaviours.Base
{
    public class Behaviour
    {
        public bool IsEnabled { get; set; } = true;
        public Units.Base.Units ThisBase { get; set; }
        public virtual void Awake()
        {
            
        }

        public virtual void Start()
        {

        }

        public virtual void Init()
		{

		}

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }

        public virtual void OnDestroy()
        {
            
        }
    }
}