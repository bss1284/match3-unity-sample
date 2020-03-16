using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace BSS.UI {
    public class UISystem : Singleton<UISystem> {
        private List<BaseView> baseViewAll = new List<BaseView>();
        private bool isLoad;


        private void Awake() {
            LoadDataOnce();
        }

        public static T Find<T>() where T : BaseView {
            instance.LoadDataOnce();
            return instance.baseViewAll.Find(x => x is T) as T;
        }
        public static T Find<T>(string _ID) where T : BaseView {
            instance.LoadDataOnce();
            return instance.baseViewAll.Find(x =>x is T && x.ID == _ID) as T;
        }
        public static T Find<T>(Func<T,bool> predicate) where T : BaseView {
            instance.LoadDataOnce();
            return instance.baseViewAll.Find(x => x is T && predicate(x as T)) as T;
        }

        public static List<T> FindAll<T>() where T : BaseView {
            instance.LoadDataOnce();
            return instance.baseViewAll.FindAll(x => x is T).ConvertAll(x => x as T);
        }

        private void LoadDataOnce() {
            if (isLoad) return;
            baseViewAll = FindObjectsOfType<BaseView>().ToList();
            isLoad = true;
        }
    }
}
