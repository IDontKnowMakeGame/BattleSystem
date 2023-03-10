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


        public bool isInit = false;

        private float timer;

        private bool isAttack = false;
        public bool IsAttack => isAttack;

        private bool canAttack = false;

        public bool CanAttack => canAttack;

        
        private PlayerAnimation playerAnimation;

        private PlayerBuff playerBuff;

        private Transform sprite;

        private Vector3 curDir;

        public override void Start()
        {
            base.Start();
            attackColParent = GameObject.FindObjectOfType<AttackCollider>();

            playerAnimation = ThisBase.GetBehaviour<PlayerAnimation>();
            playerBuff = ThisBase.GetBehaviour<PlayerBuff>();
            sprite = ThisBase.GetComponentInChildren<MeshRenderer>().transform;

            SetAnimation();

            InputManager.OnChangePress += SetAnimation;
            InputManager.OnTestChangePress += SetAnimation;
        }

        private void SetAnimation()
        {
            InputManager.OnAttackPress += SetDir;
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
            if (ThisBase.State.HasFlag(BaseState.Moving))
            {
                ThisBase.GetBehaviour<PlayerMove>().stop = false;
                return;
            }

            if (!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEquiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }

            ThisBase.RemoveState(BaseState.Attacking);
            ThisBase.GetBehaviour<PlayerMove>().stop = false;

            if (!canAttack || !playerAnimation.CurWeaponAnimator.LastChange || ThisBase.GetBehaviour<PlayerItem>().PlayerPortion.UsePortion || IsAttack)
            {
                ThisBase.GetBehaviour<PlayerMove>().stop = false;
                return;
            }

            List<EnemyBase> enemys = new List<EnemyBase>();

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
               enemys = attackColParent.AllCurrentDirEnemy();


            if (enemys.Count > 0)
            {
                //ThisBase.StartCoroutine(cameraZoom.ZoomInOut(1f));
                playerBuff.ChangeAdneraline(1);

                EventParam param = new EventParam();
                param.intParam = 1;
                Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, param);
            }


            foreach (EnemyBase enemy in enemys)
            {
                enemy.ThisStat.Damaged(damage, ThisBase);
                GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
                obj.GetComponent<DamagePopUp>().DamageText(damage, enemy.transform.position);
                onBehaviourEnd?.Invoke();
            }
            timer = attackDelay;
            canAttack = false;
        }

        private void SetDir(Vector3 dir)
        {
            if (IsAttack) return;
            curDir = dir;
            isInit = true;
        }

        public void ChangeAnimation(Vector3 dir)
        {
            if(!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEquiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }


            if (timer > 0 || !playerAnimation.CurWeaponAnimator.LastChange || isAttack || 
                ThisBase.GetBehaviour<PlayerItem>().PlayerPortion.UsePortion) return;
            if (dir == Vector3.left)
            {
                sprite.localScale = new Vector3(-1, 1, 1);
            }
            else if (dir == Vector3.right)
            {
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if (dir == Vector3.forward)
            {
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if (dir == Vector3.back)
            {
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else
                return;

/*            playerAnimation.CurWeaponAnimator.SetDir = dir;
            playerAnimation.CurWeaponAnimator.Attack = true;
            playerAnimation.SetAnmation();*/
            isAttack = true;
        }

        public void Timer()
        {
            if (!canAttack)
            {
                timer -= Time.deltaTime;
            }

            if(timer <= 0)
            {
                canAttack = true;
            }

            if (ThisBase.GetBehaviour<PlayerEquiq>().WeaponAnimation() == 1)
            {
                if (isAttack)
                {
                    isAttack = false;
                    ThisBase.RemoveState(BaseState.Attacking);
                }
            }
            else
            {
/*                if (isAttack && playerAnimation.CurIndex() > playerAnimation.GetFPS() / 2)
                {
                    isAttack = false;
                    ThisBase.GetBehaviour<PlayerMove>().stop = false;
                    ThisBase.RemoveState(BaseState.Attacking);
                }*/
            }
        }

        public void ChargeAnimation(Vector3 dir)
        {
            playerAnimation.CurWeaponAnimator.SetDir = dir;
            playerAnimation.CurWeaponAnimator.Charge = true;
            playerAnimation.SetAnmation();
        }

        public void ChangeDelay(float delay)
        {
            attackDelay = delay;
            timer = 0;
        }

        public override void OnDisable()
        {
            InputManager.OnChangePress -= SetAnimation;
            InputManager.OnTestChangePress -= SetAnimation;
            base.OnDisable();
        }
    }
}
