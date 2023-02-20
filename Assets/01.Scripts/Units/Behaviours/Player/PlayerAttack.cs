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

        public bool isAttack = false;
        public bool IsAttack => isAttack;
        
        private UnitAnimation unitAnimation;
        private PlayerPortion playerPortion;

        private PlayerBuff playerBuff;

        private Transform sprite;

        private Vector3 curDir;

        public override void Start()
        {
            base.Start();
            attackColParent = GameObject.FindObjectOfType<AttackCollider>();

            unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();
            playerPortion = ThisBase.GetBehaviour<PlayerPortion>();
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
            if (!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEqiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }


            if (timer > 0 || unitAnimation.CurState() == 10 || playerPortion.UsePortion || IsAttack)
            {
                ThisBase.GetBehaviour<PlayerMove>().stop = false;
                return;
            }

            ThisBase.AddState(BaseState.Attacking);
            ChangeAnimation(curDir);

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
        }

        private void SetDir(Vector3 dir)
        {
            if (IsAttack) return;
            curDir = dir;
            isInit = true;

            if (ThisBase.GetBehaviour<PlayerEqiq>().WeaponAnimation() != 1 && unitAnimation.CurState() != 10)
                ThisBase.GetBehaviour<PlayerMove>().stop = true;
        }

        public void ChangeAnimation(Vector3 dir)
        {
            if(!isInit)
            {
                ChangeDelay(ThisBase.GetBehaviour<PlayerEqiq>().CurrentWeapon.WeaponStat.Afs);
                isInit = true;
            }


            if (timer > 0 || unitAnimation.CurState() == 10 || isAttack || playerPortion.UsePortion) return;
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

            if (ThisBase.GetBehaviour<PlayerEqiq>().WeaponAnimation() == 1)
            {
                if (isAttack)
                {
                    isAttack = false;
                    ThisBase.RemoveState(BaseState.Attacking);
                }
            }
            else
            {
                if (isAttack && unitAnimation.CurIndex() > unitAnimation.GetFPS() / 2)
                {
                    isAttack = false;
                    ThisBase.GetBehaviour<PlayerMove>().stop = false;
                    ThisBase.RemoveState(BaseState.Attacking);
                }
            }
        }

        public void SkillAnimation(Vector3 dir)
        {
            if (dir == Vector3.left)
            {
                unitAnimation.ChangeState(7);
                sprite.localScale = new Vector3(-1, 1, 1);
            }
            else if (dir == Vector3.right)
            {
                unitAnimation.ChangeState(7);
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if (dir == Vector3.forward)
            {
                unitAnimation.ChangeState(8);
                sprite.localScale = new Vector3(1, 1, 1);
            }
            else if (dir == Vector3.back)
            {
                unitAnimation.ChangeState(9);
                sprite.localScale = new Vector3(1, 1, 1);
            }
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
