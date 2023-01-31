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

        
        private UnitAnimation unitAnimation;

        public override void Start()
        {
            base.Start();
            attackColParent = GameObject.FindObjectOfType<AttackCollider>();

            unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();

            SetAnimation();
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.TestChangeWeapon, InputStatus.Press, SetAnimation);
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, SetAnimation);
        }

        private void SetAnimation()
        {
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.UpAttack, InputStatus.Press, () => ChangeAnimation(Vector3.forward));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.DownAttack, InputStatus.Press, () => ChangeAnimation(Vector3.back));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Press, () => ChangeAnimation(Vector3.left));
            Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.RightAttack, InputStatus.Press, () => ChangeAnimation(Vector3.right));
        }

        public override void Update()
        {
            base.Update();
            // To Do InputManager?? ???
        }

        public void Attack(float damage, bool near = false)
        {
            List<EnemyBase> enemys = new List<EnemyBase>();

            if (near)
               enemys.Add(attackColParent.CurrntDirNearEnemy());
            else
               enemys = attackColParent.AllCurrentDirEnemy();

            Debug.Log(enemys.Count + "입니다..");


            if (enemys.Count > 0)
                ThisBase.StartCoroutine(cameraZoom.ZoomInOut(1f));


            foreach(EnemyBase enemy in enemys)
            {
                enemy.ThisStat.Damaged(damage);
                GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
                obj.GetComponent<DamagePopUp>().DamageText(damage, enemy.transform.position);
                onBehaviourEnd?.Invoke();
            }
        }

        public void ChangeAnimation(Vector3 dir)
        {
            Debug.Log("오");

            if(dir == Vector3.left)
            {
                unitAnimation.state = 4;
                ThisBase.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(dir == Vector3.right)
            {
                unitAnimation.state = 4;
                ThisBase.transform.localScale = new Vector3(1, 1, 1);
            }
            else if(dir == Vector3.forward)
            {
                unitAnimation.state = 5;
                ThisBase.transform.localScale = new Vector3(1, 1, 1);
            }
            else if(dir == Vector3.back)
            {
                unitAnimation.state = 6;
                ThisBase.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
