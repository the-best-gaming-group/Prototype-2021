using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Platformer.Mechanics
{
    [Serializable]
    public class Checkpoint 
    {
        public int playerHealth;
        public SpawnsDict spawns = new ();
        public PlayDoorSoundDict playDoorSound = new();
        public PlayerPosDict playerPos = new();
        public AvailableSpellsDict spells = new();
        public string SceneName;
		public float coins;
		public int[,] shopItems = new int[5, 5];
        public string[] itemNames = new string[5];

        public Checkpoint(
            int playerHealth,
            SpawnsDict Spawns,
            PlayDoorSoundDict PlayDoorSound,
            PlayerPosDict PlayerPos,
            AvailableSpellsDict spells,
            string SceneName,
            float coins,
		    int[,] shopItems,
            string[] itemNames
            )
        {
			//this.playerHealth = Math.Min(100, playerHealth + 30);        
			this.playerHealth = playerHealth;
			spawns = Spawns.GetCopy();
            playDoorSound = PlayDoorSound.GetCopy();
            playerPos = PlayerPos.GetCopy();
            this.spells = spells.GetCopy();
            this.SceneName = SceneName;
			this.coins = coins;
			this.shopItems = shopItems;
            this.itemNames = itemNames;
		}

        [Serializable]
        public class SceneSpawnsDict : SerializedDictionary<string,int> {
            public SceneSpawnsDict GetCopy()
            {
                var ret = new SceneSpawnsDict();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }
        [Serializable]
        public class SpawnsDict : SerializedDictionary<string,SceneSpawnsDict>
        {
            public SpawnsDict GetCopy()
            {
                var ret = new SpawnsDict();
                foreach (var (key, val) in this)
                {
                    ret[key] = val.GetCopy();
                }
                return ret;
            }
        }
        [Serializable]
        public class PlayDoorSoundDict : SerializedDictionary<string,bool>{
            public PlayDoorSoundDict GetCopy()
            {
                var ret = new PlayDoorSoundDict();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }

        [Serializable]
        public class PlayerPosDict : SerializedDictionary<string, Vector3>{
            public PlayerPosDict GetCopy()
            {
                var ret = new PlayerPosDict();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }
        [Serializable]
        public class AvailableSpellsDict : SerializedDictionary<string, bool>{
            public AvailableSpellsDict GetCopy()
            {
                var ret = new AvailableSpellsDict();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }

    }
}