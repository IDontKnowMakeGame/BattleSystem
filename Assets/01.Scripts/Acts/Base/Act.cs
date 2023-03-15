using Actors.Bases;

namespace Acts.Base
{
    public class Act
    {
        public bool IsEnabled { get; set; } = true;
        public Actor ThisActor { get; set; }
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

        public virtual void OnApplicationQuit()
        {

        }
    }
}