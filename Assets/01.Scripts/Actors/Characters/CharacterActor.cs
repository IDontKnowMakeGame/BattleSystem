using Actors.Bases;
using Acts.Characters;
using Core;

namespace Actors.Characters
{
    public class CharacterActor : Actor
    {
        protected override void Init()
        {
            AddAct<CharacterMove>();
            AddAct<CharacterRender>();
        }

        protected override void Awake()
        {
            InGame.AddActor(this);
            base.Awake();
        }

        protected override void Start()
        {
            InGame.SetActorOnBlock(this, Position);
            base.Start();
        }
    }
}