using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace BSS {
    public class Mover : MonoBehaviour  {
        public enum MoveType {
            None,Goal,Follow
        }

        public float speed;
        public float stopDistance = 0.1f;
        public MoveType moveType;
        
        [ShowIf("moveType", MoveType.Goal)]
        public Vector3 destination;
        [ShowIf("moveType", MoveType.Follow)]
        public GameObject target;

        private List<Vector3> nextGoals = new List<Vector3>();
        private event Action OnCompleted;
        private event Action OnFailed;


        public bool IsMoving() {
            return moveType != MoveType.None;
        }


        public void ToGoal(Vector3 _destination,Action completeAct=null) {
            destination = _destination;
            OnCompleted += completeAct;
            moveType = MoveType.Goal;
        }

        public void ToGoal(List<Vector3> desList,Action completeAct= null) {
            if (desList.Count == 0) return;
            ToGoal(desList[0], completeAct);
            desList.RemoveAt(0);
            nextGoals = desList;
        }

        public void ToFollow(GameObject _target) {
            ToFollow(_target, null, null);
        }
        public void ToFollow(GameObject _target, Action completeAct) {
            ToFollow(_target, completeAct, null);
        }
        public void ToFollow(GameObject _target, Action completeAct, Action failAct) {
            OnCompleted += completeAct;
            OnFailed += failAct;
            target = _target;
            moveType = MoveType.Follow;
        }



        public void Stop() {
            ResetState();
        }

        private void Update() {
            if (moveType == MoveType.None) return;
            
            if (moveType == MoveType.Goal) {//목표지점까지 이동
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destination) < stopDistance) {
                    if (nextGoals.Count == 0) {
                        transform.position = destination;
                        OnCompleted?.Invoke();
                        ResetState();
                        return;
                    } else {
                        destination = nextGoals[0];
                        nextGoals.RemoveAt(0);
                        return;
                    }
                }
            } else if (moveType == MoveType.Follow) {//목표 타겟한테 이동
                if (target == null) {
                    OnFailed?.Invoke();
                    ResetState();
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.transform.position) < stopDistance) {
                    transform.position = target.transform.position;
                    OnCompleted?.Invoke();
                    ResetState();
                }
            } 
        }


        private void ResetState() {
            moveType = MoveType.None;
            OnFailed = null;
            OnCompleted = null;
        }
    }
}