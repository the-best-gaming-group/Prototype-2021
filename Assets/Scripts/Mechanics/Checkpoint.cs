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
        public EnemySpawns enemySpawns = new ();
        public PlayDoorSound playDoorSound = new();
        public PlayerPos playerPos = new();
        public string SceneName;
        
        public Checkpoint(
            int playerHealth,
            EnemySpawns EnemySpawns,
            PlayDoorSound PlayDoorSound,
            PlayerPos PlayerPos,
            string SceneName
            )
        {
            this.playerHealth = Math.Min(100, playerHealth + 30);        

            enemySpawns = EnemySpawns.GetCopy();
            playDoorSound = PlayDoorSound.GetCopy();
            playerPos = PlayerPos.GetCopy();
            this.SceneName = SceneName;
        }

        [Serializable]
        public class SceneEnemySpawns : SerializedDictionary<string,bool> {
            public SceneEnemySpawns GetCopy()
            {
                var ret = new SceneEnemySpawns();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }
        [Serializable]
        public class EnemySpawns : SerializedDictionary<string,SceneEnemySpawns>
        {
            public EnemySpawns GetCopy()
            {
                var ret = new EnemySpawns();
                foreach (var (key, val) in this)
                {
                    ret[key] = val.GetCopy();
                }
                return ret;
            }
        }
        [Serializable]
        public class PlayDoorSound : SerializedDictionary<string,bool>{
            public PlayDoorSound GetCopy()
            {
                var ret = new PlayDoorSound();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }

        [Serializable]
        public class PlayerPos : SerializedDictionary<string, Vector3>{
            public PlayerPos GetCopy()
            {
                var ret = new PlayerPos();
                foreach (var (key, val) in this)
                {
                    ret[key] = val;
                }
                return ret;
            }
        }

    }
}