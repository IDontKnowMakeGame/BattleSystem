using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;
using Managements;
using Core;
using Managements.Managers;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerAttack : UnitAttack
    {
        private AttackCollider attackColParent;
        public AttackCollider AttackColParent
        {
            get
            { 
                if (attackColParent == null)
                {
                    Debug.LogError("attackColParent?? NULL????.");
                    return new AttackCollider();
                }
                return attackColParent;
            }
        }

        [SerializeField]
        private CameraZoom cameraZoom;


        private float attackDelay;


        private bool isInit = false;

        private float timer;

        public bool isAttack = false;
        public bool IsAttack => isAttack;
        
        private UnitAnimation unitAnimation;

        private PlayerBuff playerBuff;

        private Transform sprite;

        public override void Start()
        {
            base.Start();
            attackColParent = GameObject.FindObjectOfType<AttackCollider>();

            unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();
            playerBuff = ThisBase.GetBehaviour<PlayerBuff>();
            sprite = ThisBase.GetComponentInChildren<MeshRenderer>().transform;

            SetAnimation();
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.TestChangeWeapon, InputStatus.Press, SetAnimation);
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, SetAnimation);
        }

        private void SetAnimation()
        {
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.UpAttack, InputStatus.Press, () => ChangeAnimation(Vector3.forward));
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.DownAttack, InputStatus.Press, () => ChangeAnimation(Vector3.back));
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.LeftAttack, InputStatus.Press, () => ChangeAnimation(Vector3.left));
            Define.GetManager<InputManager>().AddInGameAction(InputTarget.RightAttack, InputStatus.Press, () => ChangeAnimation(Vector3.right));
        }

        public override void Update()
        {
            base.Update();
            Timer();
            // To Do InputManager?? ???
        }

        public bool HasEnemy()
		{
            List<EnemyBase> enemys = new List<EnemyBase>();
            enemys = attackColParent.AllCurrentDirEnemy();
            if (enemys.Count > 0)
                return true;
            else
                return false;
        }
        public void Attack(float damage, bool near = false)
        {
            if (!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEqiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }


            if (timer > 0 || unitAnimation.CurState() == 10) return;

            ThisBase.AddState(BaseState.Attacking);

            List<EnemyBase> enemys = new List<EnemyBase>();

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
               enemys = attackColParent.AllCurrentDirEnemy();


            if (enemys.Count > 0)
            {
                ThisBase.StartCoroutine(cameraZoom.ZoomInOut(1f));
                playerBuff.ChangeAdneraline(1);
                //Define.GetManager<EventManager>().TriggerEvent(EventFlag.CameraShake,new EventParam());
            }


            foreach (EnemyBase enemy in enemys)
            {
                enemy.ThisStat.Damaged(damage);
                GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
                obj.GetComponent<DamagePopUp>().DamageText(damage, enemy.transform.position);
                onBehaviourEnd?.Invoke();
            }

            timer = attackDelay;
        }

        public void ChangeAnimation(Vector3 dir)
        {
            if(!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEqiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }


            if (timer > 0 || unitAnimation.CurState() == 10) return;
            if (dir == Vector3.left)
            {
                unitAnimation.ChangeState(4);
                sprite.localScale = new Vector3(-1, 1, 1);
            }
            else if(dir == Vector3.right)
            {
                unitAnimation.ChangeState(4);
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if(dir == Vector3.forward)
            {
                unitAnimation.ChangeState(5);
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if(dir == Vector3.back)
            {
                unitAnimation.ChangeState(6);
                sprite.localScale = new Vector3(1, 1, 1);
            }
            isAttack = true;    
        }

        public void Timer()
        {
            if(timer > 0)
                timer -= Time.deltaTime;

            if (isAttack && unitAnimation.CurIndex() > unitAnimation.GetFPS() / 2)
            {
                isAttack = false;
                ThisBase.RemoveState(BaseState.Attacking);
            }
        }

        public void ChangeDelay(float delay)
        {
            attackDelay = delay;
            timer = 0;
        }
    }
}
