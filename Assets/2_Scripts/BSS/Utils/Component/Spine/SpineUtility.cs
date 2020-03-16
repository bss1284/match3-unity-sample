using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BSS;



namespace BSS {
#if SPINE
    using Spine.Unity;
    using BSS.Extension;
    public class SpineAnimationData {
        public int ID;
        public GameObject Drawer;
        public SkeletonAnimation playerAnim;
        public int AniCount;
        public string AniName;
    }


    public class SpineUtility : Singleton<SpineUtility> {

        private Dictionary<int, SpineAnimationData> playList = new Dictionary<int, SpineAnimationData>();

        private int IDConter;

        /// <summary>
        /// Spine 애니메이션을 생성해서 재생후 제거합니다.
        /// </summary>
        public static SkeletonAnimation CreateCharacter(SkeletonAnimation spinePrefab, Vector2 pos, int sortOrder, string AniName, int aniCount = 1, bool IsLoop = false) {
            return instance.CreateSpineAnimation(spinePrefab, pos, sortOrder, AniName, aniCount, IsLoop);
        }
        /// <summary>
        /// Spine 애니메이션을 생성해서 재생후 제거합니다.
        /// </summary>
        public static SkeletonAnimation CreateCharacter(GameObject spineObject, Vector2 pos, int sortOrder, string AniName, int aniCount = 1, bool IsLoop = false) {
            var spine = spineObject.GetComponent<SkeletonAnimation>();
            return CreateCharacter(spine, pos, sortOrder, AniName, aniCount, IsLoop);
        }
        /// <summary>
        /// Spine 애니메이션을 생성해서 재생후 제거합니다.
        /// </summary>
        public static SkeletonAnimation CreateCharacter(SkeletonAnimation spinePrefab, Vector2 pos, int sortOrder, int aniIndex, int aniCount = 1, bool IsLoop = false) {
            return instance.CreateSpineAnimation(spinePrefab, pos, sortOrder, spinePrefab.GetAnimationName(aniIndex), aniCount, IsLoop);
        }
        /// <summary>
        /// Spine 애니메이션을 생성해서 재생후 제거합니다.
        /// </summary>
        public static SkeletonAnimation CreateCharacter(GameObject spineObject, Vector2 pos, int sortOrder, int aniIndex, int aniCount = 1, bool IsLoop = false) {
            var spine = spineObject.GetComponent<SkeletonAnimation>();
            return CreateCharacter(spine, pos, sortOrder, aniIndex, aniCount, IsLoop);
        }




        /// <summary>
        /// 이미 씬에 존재하는 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetSpineAnimation(SkeletonAnimation targetSpine, string animName, bool loop = false) {
            var anmations = targetSpine.SkeletonDataAsset.GetSkeletonData(true).animations;
            float Duration = targetSpine.GetAnimationDuration(animName);

            if (targetSpine.state != null)
                targetSpine.state.SetAnimation(0, animName, loop);
            else
                targetSpine.AnimationState.SetAnimation(0, animName, loop);

            return Duration;
        }
        /// <summary>
        /// 이미 씬에 존재하는 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetSpineAnimation(SkeletonAnimation targetSpine, int animIndex, bool loop = false) {
            return SetSpineAnimation(targetSpine, targetSpine.GetAnimationName(animIndex), loop);
        }

        /// <summary>
        /// 이미 씬에 존재하는 GameObject에서 SkeletonAnimation을 찾아 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetSpineAnimation(GameObject targetSpine, int animIndex, bool loop = false) {

            if (targetSpine == null) {
                DebugSystem.Log("SetSpineAnimation targetSpine null ");
                return 0.0f;
            }

            var spine = targetSpine.GetComponent<SkeletonAnimation>();

            return SetSpineAnimation(spine, animIndex, loop);
        }

        /// <summary>
        /// 이미 씬에 존재하는 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetUISpineAnimation(SkeletonGraphic targetSpine, string animName, bool loop = false) {
            var anmations = targetSpine.SkeletonDataAsset.GetSkeletonData(true).animations;
            float Duration = targetSpine.GetAnimationDuration(animName);
            targetSpine.AnimationState?.SetAnimation(0, animName, loop);

            return Duration;
        }
        /// <summary>
        /// 이미 씬에 존재하는 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetUISpineAnimation(SkeletonGraphic targetSpine, int animIndex, bool loop = false) {
            return SetUISpineAnimation(targetSpine, targetSpine.GetAnimationName(animIndex), loop);
        }

        /// <summary>
        /// 이미 씬에 존재하는 GameObject에서 SkeletonGraphic을 찾아 Spine 애니메이션을 재생합니다.
        /// </summary>
        public static float SetUISpineAnimation(GameObject targetSpine, int animIndex, bool loop = false) {
            var spine = targetSpine.GetComponent<SkeletonGraphic>();
            return SetUISpineAnimation(spine, animIndex, loop);
        }


        public bool Clear(string spineName) {
            List<int> ListDeleteID = new List<int>();
            //string CloneName = spineName +
            StringBuilder sb = new StringBuilder();

            sb.Append(spineName);
            sb.Append("(Clone)");

            foreach (var data in playList) {
                if (data.Value.Drawer.gameObject.name.CompareTo(sb.ToString()) == 0) {
                    Destroy(data.Value.Drawer);
                    ListDeleteID.Add(data.Key);
                }
            }

            foreach (var id in ListDeleteID) {
                playList.Remove(id);
                return true;
            }

            return false;
        }
        public void Clear(int id) {
            if (playList[id].Drawer != null)
                Destroy(playList[id].Drawer);

            playList.Remove(id);
        }

        public void Clear() {
            foreach (var data in playList) {
                Destroy(playList[data.Key].Drawer);
            }

            playList.Clear();
        }


        private void SetAnmation(int id, string aniName, int aniCount = 1, bool IsLoop = false) {
            if (playList.ContainsKey(id)) {
                playList[id].playerAnim.state.SetAnimation(0, aniName, IsLoop);
                playList[id].AniName = aniName;
                playList[id].AniCount = aniCount;
                playList[id].playerAnim.state.Complete += delegate {
                    if (playList[id].playerAnim.state.GetCurrent(0).loop == false) {
                        if (--playList[id].AniCount <= 0) {
                            Clear(id);
                        } else
                            playList[id].playerAnim.state.SetAnimation(0, aniName, false);
                    }

                };

            }
        }


        private SkeletonAnimation CreateSpineAnimation(SkeletonAnimation spinePrefab, Vector2 pos, int sortOrder, string AniName, int aniCount = 1, bool IsLoop = false) {
            SpineAnimationData data = new SpineAnimationData();
            data.ID = IDConter++;

            SkeletonAnimation copySpine = Instantiate(spinePrefab);
            data.Drawer = copySpine.gameObject;
            data.Drawer.transform.position = pos;
            data.playerAnim = data.Drawer.GetComponent<SkeletonAnimation>();
            data.playerAnim.SetSortingOrder(sortOrder);
            playList.Add(data.ID, data);

            SetAnmation(data.ID, AniName, aniCount, IsLoop);

            return data.playerAnim;
        }

    }
#endif
}