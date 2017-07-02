﻿namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
    using Common.ContinueObjects;
    using System;

    /// <summary>
    // Performs the autoattack on input.
    /// </summary>
    [RequireComponent(typeof(ActionStateComponent))]
    public class AutoAttackController : MonoBehaviour {

        private AOEHintCreator m_AOEHintCreator;
        private ActionStateComponent m_ActionState;
        private RotationController m_RotationController;
        private ActionCode m_AutoAttackAction;
        private bool m_ContinueAutoAttack;

        private static readonly TimeSpan AADURATION = new TimeSpan(0, 0, 1);

        private void Awake() {
            m_RotationController = GetComponent<RotationController>();
            m_ActionState = GetComponent<ActionStateComponent>();
            m_AOEHintCreator = GetComponent<AOEHintCreator>();
            SetAutoAttackType();
        }

        private void SetAutoAttackType() {
            var weapon = GameData.characterSetting.Weapon;
            switch ((WeaponCode)weapon) {
                case WeaponCode.Axe:
                    m_AutoAttackAction = ActionCode.AxeAutoAttack;
                    return;
                case WeaponCode.Bow:
                    m_AutoAttackAction = ActionCode.BowAutoAttack;
                    return;
            }
        }

        private void Update() {
            if (m_ActionState.ActionState != ActionCode.Idle) { return; }

            if (Input.GetButtonDown("AutoAttack")) {
                var mousePoint = InputExtension.GetMouseHitGroundPoint();
                if (mousePoint == null) { return; }
                var forwardVec = mousePoint.Value - transform.position;
                forwardVec = forwardVec.normalized;
                PerformAutoAttack(forwardVec);
            }

            if (m_ContinueAutoAttack) {
                if (!TargetInRange()) {
                    m_ContinueAutoAttack = false;
                    return;
                }

                var forwardVec = GameData.Target.transform.position - transform.position;
                forwardVec = forwardVec.normalized;
                PerformAutoAttack(forwardVec);
            }
        }
        
        private void PerformAutoAttack(Vector3 lookDirection) {
            m_ActionState.ActionState = m_AutoAttackAction;
            m_RotationController.LookAt(lookDirection, AADURATION);

            switch (m_AutoAttackAction) {
                case ActionCode.AxeAutoAttack:
                    RequestOperations.AxeAutoAttackRequest(lookDirection);
                    break;
                case ActionCode.BowAutoAttack:
                    RequestOperations.BowAutoAttackRequest(lookDirection);
                    break;
            }

            var setIdleCondition = new TimedContinueCondition(AADURATION);
            setIdleCondition.ContinueEvent += (CallReason cr) => m_ActionState.ActionState = ActionCode.Idle;
            setIdleCondition.ContinueEvent += FinishedAutoAttack;
            setIdleCondition.Start();
        }

        private void FinishedAutoAttack(CallReason cr) {
            if (cr == CallReason.Interupted) { return; }
            m_ContinueAutoAttack = true;
        }

        private bool TargetInRange() {
            if(GameData.Target == null) { return false; }
            if(GetAARange() < GetDistanceToTarget()) { return false; }
            return true;
        }

        private float GetAARange() {
            return m_AutoAttackAction == ActionCode.AxeAutoAttack ? 3.0f : 8.0f;
        }

        private float GetDistanceToTarget() {
            if(GameData.Target == null) { return -1f; }
            return Vector3.Distance(transform.position, GameData.Target.transform.position);
        }
    }
}
