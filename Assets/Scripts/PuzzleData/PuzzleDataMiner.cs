using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

using PuzzleHen.Data;

namespace PuzzleHen
{
    public class PuzzleDataMiner : MonoBehaviour
    {
        [SerializeField] private List<PuzzleDataGirl> girls;

        private static PuzzleDataMiner _instance;

        public static PuzzleData Current { 
            private set
            {
                _instance.current = value;
            }
            get
            {
                if (_instance.current == null)
                {
                    return GetGirls(0).GirlList[0].List[0];
                }

                return _instance.current;
            }
        }

        private PuzzleData current;

        private static int currGirl;
        private static int currList;
        private static int currData;

        public static void SetCurrent(PuzzleData _current, int _currGirl, int _currList, int _currData)
        {
            Current = _current;
            currGirl = _currGirl;
            currList = _currList;
            currData = _currData;
        }

        public static void SetupCompleteNextChallenge()
        {
            Current.IsCompleted = true;

            // Cache local copies for incrementing
            int g = currGirl;
            int l = currList;
            int d = currData + 1;

            var girl = GetGirls(g);

            // Level 1: Check Data Index
            if (d >= girl.GirlList[l].Count)
            {
                d = 0;
                l++;

                // Level 2: Check List Index
                if (l >= girl.Count)
                {
                    l = 0;
                    g++;

                    // Level 3: Check Girl Index (Loop back to start if end of game)
                    if (g >= Count)
                    {
                        g = 0;
                    }
                }
            }

            // Now that indices are finalized, fetch the single item
            PuzzleData nextItem = GetGirls(g).GirlList[l].List[d];

            nextItem.IsUnlock = true;

            // Centralized update and save
            SetCurrent(nextItem, g, l, d);
            SaveData();
        }

        private void Awake()
        {
            // 1. Check if an instance already exists
            if (_instance != null && _instance != this)
            {
                // Destroy this copy because another one already exists
                Destroy(gameObject);
                return;
            }

            // 2. Set the instance and make it persistent
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // 3. Initialize your data
            LoadData();
        }

        #region SAVE_LOAD
        public static void SaveData() => _instance.PrivSaveData();
        public static void InjectDataFirstProgression()
        {
            var item = _instance.girls[0].GirlList[0].List[0];
            string key = _instance.girls[0].ID + "##" + _instance.girls[0].GirlList[0].ID + "##" + item.ID;
            SPrefs.SetBool(key + "##Unlock", true);
            SPrefs.SetBool(key + "##Completed", false);
        }
        private void PrivSaveData()
        {
            for (int i = 0; i < girls.Count; i++)
            {
                for (int j = 0; j < girls[i].GirlList.Count; j++)
                {
                    foreach (var item in girls[i].GirlList[j].List)
                    {
                        string key = girls[i].ID + "##" + girls[i].GirlList[j].ID + "##" + item.ID;
                        SPrefs.SetBool(key + "##Unlock", item.IsUnlock);
                        SPrefs.SetBool(key + "##Completed", item.IsCompleted);
                    }
                }
            }

            SPrefs.Save();
        }
        private void LoadData()
        {
            SPrefs.Load();

            for (int i = 0; i < girls.Count; i++)
            {
                for (int j = 0; j < girls[i].GirlList.Count; j++)
                {
                    foreach (var item in girls[i].GirlList[j].List)
                    {
                        string key = girls[i].ID + "##" + girls[i].GirlList[j].ID + "##" + item.ID;
                        item.IsUnlock = SPrefs.GetBool(key + "##Unlock", false);
                        item.IsCompleted = SPrefs.GetBool(key + "##Completed", false);
                    }
                }
            }
        }
        #endregion

        public static int Count => _instance.girls.Count;

        public static IReadOnlyList<PuzzleDataGirl> GetGirls()
        {
            if (_instance == null)
                Debug.LogError("PuzzleDataMiner not found in scene!");

            return _instance.girls;
        }

        public static PuzzleDataGirl GetGirls(int _index)
        {
            if (_instance == null)
                Debug.LogError("PuzzleDataGirl not found in scene!");

            return GetGirls()[_index];
        }

        public static string GetOneGirlProgression(int _index) => GetGirls()[_index].GetProgress();

        //public static PuzzleDataGirl GetById(string id)
        //{
        //    if (_instance == null)
        //        return null;

        //    return _instance.girls.Find(g => g.Id == id);
        //}
    }
}