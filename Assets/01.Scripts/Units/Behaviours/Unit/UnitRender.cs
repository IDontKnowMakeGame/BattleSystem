using Units.Behaviours.Base;

namespace Units.Behaviours.Unit
{
    public class UnitRender : Behaviour
    {
        public override void Update()
        {
            Render();
        }

        protected virtual void Render()
        {
            
        }
    }
}