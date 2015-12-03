using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace com.kuxiong.battlemodule
{
    /// 索敌(包括索敌前的冲锋)
    public class WalkStateV3 : FsmState
    {
        #region Model
        ///extern
        public NormalControl selfControl;
        public ArmyControlV2 amryControlV2
        {
            get
            {
                return selfControl.armyControlV2;
            }
        }
        ///inside
        public bool chargeSign = false;
        #endregion

        #region Controller
        public NormlWalkSubState curState = NormlWalkSubState.none;
        public void SwitchState(NormlWalkSubState state)
        {
            if (this.curState != state)
            {
                switch (this.curState)
                {
                    case NormlWalkSubState.none: OnLeave_none();
                        break;
                    case NormlWalkSubState.beControl: OnLeave_beControl();
                        break;
                    case NormlWalkSubState.move: OnLeave_move();
                        break;
                    case NormlWalkSubState.charge: OnLeave_charge();
                        break;
                    default:
                        break;
                }

                this.curState = state;

                switch (this.curState)
                {
                    case NormlWalkSubState.none: OnBefore_none();
                        break;
                    case NormlWalkSubState.beControl: OnBefore_beControl();
                        break;
                    case NormlWalkSubState.move: OnBefore_move();
                        break;
                    case NormlWalkSubState.charge: OnBefore_charge();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        public WalkStateV3(
            string stateName, NormalControl selfControl)
        {
            this.stateName = stateName;
            this.selfControl = selfControl;
        }

        public override void DoBeforeEntering()
        {
            this.curState = NormlWalkSubState.none;
        }


        public override void Reason()
        {
            ///变羊 眩晕 冰冻 等控制类 
            if (BeControl())
            {
                SwitchState(NormlWalkSubState.none);
            }
            else
            {
                if (chargeSign)
                {
                    SwitchState(NormlWalkSubState.charge);
                }
                else
                {
                    SwitchState(NormlWalkSubState.move);
                }
            }
        }

        public override void Act()
        {
            switch (this.curState)
            {
                case NormlWalkSubState.none: On_none();
                    break;
                case NormlWalkSubState.beControl: On_beControl();
                    break;
                case NormlWalkSubState.move: On_move();
                    break;
                case NormlWalkSubState.charge: On_charge();
                    break;
                default:
                    break;
            }
        }

        #region none
        public void OnBefore_none() { }
        public void On_none() { }
        public void OnLeave_none() { }
        #endregion

        #region beControl
        public void OnBefore_beControl()
        {
            selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.stand, null, null);
        }
        public void On_beControl() { }
        public void OnLeave_beControl() { }
        #endregion

        #region charge
        public void OnBefore_charge()
        {
            selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.charge, null, null);
            selfControl.steering.MAX_VELOCITY
                = selfControl.fightData.cur_moveSpeed + CeHua_FightConfig.moveSpeedAdd_WhenCharge;
        }
        public void On_charge()
        {
            selfControl.steering.reset();
            // fresh cur velocity depend on maxVelocityadd
            selfControl.velocity = this.curGroupMoveNormalizeDir * (selfControl.fightData.cur_moveSpeed + CeHua_FightConfig.moveSpeedAdd_WhenCharge);
            //update pos
            selfControl.steering.update();

            //update facing
            selfControl.isFacingRight = this.curGroupMoveNormalizeDir.x > 0 ? true : false;
        }
        public void OnLeave_charge()
        {
            selfControl.steering.MAX_VELOCITY  = selfControl.fightData.cur_moveSpeed;
        }
        #endregion

        #region move
        private float randomDelay;
        //private float reduceRate = 0.1f;
        private Vector3 curGroupMoveNormalizeDir;
        private float cohesionAdd;

        #region legacy
        //public void OnBefore_move()
        //{
        //    randomDelay = UnityEngine.Random.Range(0.1f, 0.5f);
        //    //longer delay,  more add on maxvelocicy
        //    maxVelocityadd = randomDelay * 10 * 0.1f;
        //}

        //public void On_move()
        //{
        //    if ((randomDelay -= Time.deltaTime) < 0f)
        //    {
        //        selfControl.steering.reset();

        //        //reduce maxVelocityadd during time 
        //        if (maxVelocityadd > 0f)
        //        {
        //            maxVelocityadd -= reduceRate * Time.deltaTime;
        //            selfControl.steering.MAX_VELOCITY = selfControl.fightData.cur_moveSpeed * (1.0f + maxVelocityadd);
        //        }

        //        // fresh cur velocity depend on maxVelocityadd
        //        selfControl.velocity = this.curGroupMoveNormalizeDir * selfControl.fightData.cur_moveSpeed * (1.0f + maxVelocityadd);

        //        //sperate some too close to me
        //        selfControl.steering.flocking(
        //            this.amryControlV2.nomalControlList.Select(t => (BaseControl)t).ToList(),
        //                2.0f, 0f, 0f, 1.0f);

        //        //update pos
        //        selfControl.steering.update();
        //        //update facing
        //        selfControl.isFacingRight = this.curGroupMoveNormalizeDir.x > 0 ? true : false;
        //        //update anim
        //        selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.run, null, null);
        //    }
        //    else
        //    {
        //        selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.stand, null, null);
        //    }

        //}
        #endregion
        public void OnBefore_move()
        {
            randomDelay = UnityEngine.Random.Range(0.1f, 1.0f);
        }
        public void On_move()
        {
            if ((randomDelay -= Time.deltaTime) > 0)
            {
                selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.stand, null, null);
            }
            else
            {
                // fresh cur velocity depend on maxVelocityadd
                selfControl.velocity = this.curGroupMoveNormalizeDir * selfControl.fightData.cur_moveSpeed;

                selfControl.steering.reset();

                selfControl.steering
                    .flocking(this.amryControlV2.nomalControlList, out cohesionAdd);

                selfControl.steering.MAX_VELOCITY = selfControl.fightData.cur_moveSpeed * (1.0f + cohesionAdd);

                //update pos
                selfControl.steering.update();

                if (cohesionAdd > 0) {
                    //Debug.Log(selfControl.name + " cohesionAdd : " + cohesionAdd + " selfControl.fightData.cur_moveSpeed " + selfControl.fightData.cur_moveSpeed);
                    cohesionAdd -= (.1f * Time.deltaTime);
                } 
                
                //update facing
                selfControl.isFacingRight = this.curGroupMoveNormalizeDir.x > 0 ? true : false;

                selfControl.animatorMgr.SetAnimationClip(AnimationClipNameEnum.run, null, null);
            }


        }
        public void OnLeave_move()
        {

        }
        #endregion

        #region Util
        /// Condition
        public bool BeControl()
        {
            return selfControl.deBuffMgr.BeControl();
        }
        public bool CanCharge()
        {
            return this.chargeSign;
        }
        #endregion

        #region 外部接口
        public void SetDir(Vector3 dir, bool chargeSign)
        {
            this.curGroupMoveNormalizeDir = dir;
            this.chargeSign = chargeSign;
        }
        #endregion

        public override void DrawVisualization()
        {
            switch (this.curState)
            {
                case NormlWalkSubState.none:
                    break;
                case NormlWalkSubState.beControl:
                    break;
                case NormlWalkSubState.move:
                    break;
                case NormlWalkSubState.charge:
                    break;
                default:
                    break;
            }
        }
    }
}





