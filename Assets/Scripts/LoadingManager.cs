using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadingManager : MonoBehaviour
{
    void OnEnable()
    {
        //Delete existing permanent items
        if (PersistentManager.Instance.grottosCompleted.Contains(PersistentManager.Instance.spawnpoint) && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Grotto"))
            Destroy(gameObject);
        if (name == "White Sword" && PersistentManager.Instance.maxHealth >= 32)
            GetComponent<BoxCollider2D>().enabled = true;
        if (name == "Master Sword" && PersistentManager.Instance.maxHealth >= 52)
            GetComponent<BoxCollider2D>().enabled = true;
        if (name == "Boomerang" && PersistentManager.Instance.hasBoomerang)
            Destroy(gameObject);
        if (name == "Magical Boomerang" && PersistentManager.Instance.hasBoomerang2)
            Destroy(gameObject);
        if (name == "Bow" && PersistentManager.Instance.hasBow)
            Destroy(gameObject);
        if (name == "Silver Arrows" && PersistentManager.Instance.hasBow2)
            Destroy(gameObject);
        if (name == "Red Candle" && PersistentManager.Instance.hasRedCandle)
            Destroy(gameObject);
        if (name == "Recorder" && PersistentManager.Instance.hasFlute)
            Destroy(gameObject);
        if (name == "Letter" && PersistentManager.Instance.hasLetter)
            Destroy(gameObject);
        if (name == "Wand" && PersistentManager.Instance.hasWand)
            Destroy(gameObject);
        if (name == "Raft" && PersistentManager.Instance.hasRaft)
            Destroy(gameObject);
        if (name == "Book" && PersistentManager.Instance.hasBook)
            Destroy(gameObject);
        if (name == "Red Ring" && PersistentManager.Instance.hasRing2)
            Destroy(gameObject);
        if (name == "Ladder" && PersistentManager.Instance.hasLadder)
            Destroy(gameObject);
        if (name == "Skeleton Key" && PersistentManager.Instance.hasSkeletonKey)
            Destroy(gameObject);
        if (name == "Power Bracelet" && PersistentManager.Instance.hasPowerBracelet)
            Destroy(gameObject);
        if (name == "Compass" && PersistentManager.Instance.dungeonCompasses.Contains(PersistentManager.Instance.inLevel))
            Destroy(gameObject);
        if (name == "Map" && PersistentManager.Instance.dungeonMaps.Contains(PersistentManager.Instance.inLevel))
            Destroy(gameObject);

        //Potion Shop
        if (!PersistentManager.Instance.letterDelivered && "D4E1E7H3I8L5N1".Contains(PersistentManager.Instance.spawnpoint))
        {
            if (name == "Potion")
            {
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            if (name == "Potion" || name == "Prices")
                GetComponent<SpriteRenderer>().enabled = false;
        }
        //Menu Loading
        if (name == "Menu Book" && PersistentManager.Instance.hasBook)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Ladder" && PersistentManager.Instance.hasLadder)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Raft" && PersistentManager.Instance.hasRaft)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Power Bracelet" && PersistentManager.Instance.hasPowerBracelet)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Blue Ring" && PersistentManager.Instance.hasRing)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Red Ring" && PersistentManager.Instance.hasRing2)
            gameObject.GetComponent<Renderer>().enabled = true;
        if (name == "Menu Skeleton Key" && PersistentManager.Instance.hasSkeletonKey)
            gameObject.GetComponent<Renderer>().enabled = true;
        //Menu Triforce
        if (name == "Triforce")
        {
            GameObject[] triforce = GameObject.FindGameObjectsWithTag("Menu Triforce").OrderBy(go => go.name).ToArray();
            for (var i = 0; i < PersistentManager.Instance.levelsCompleted.Count; i++)
            {
                triforce[i].GetComponent<Renderer>().enabled = true;
            }
        }

        //Secret Covers Loading
        if (PersistentManager.Instance.coversCleared.Contains(new Vector3Int(3, 5, 0)) && name == "Level 7 Cover")
            Destroy(gameObject);

        //Tilemap Covers Loading
        if (name == "Bombable Walls" || name == "Burnable Walls" || name == "Pushable Blocks")
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            for (int i = 0; i < PersistentManager.Instance.coversCleared.Count; i++)
            {
                tilemap.SetTile(PersistentManager.Instance.coversCleared[i], null);
            }
        }
        if (name == "Locked Doors")
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            for (int i = 0; i < PersistentManager.Instance.doorsOpened.Count; i++)
            {
                tilemap.SetTile(PersistentManager.Instance.doorsOpened[i], null);
            }
        }

        //Sprite Color Loading (Editor Only)
        if (name == "Player 1")
        {
#if UNITY_EDITOR
            AssetDatabase.ImportAsset("Assets/Sprites/Link Sprites.png", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
            AssetDatabase.ImportAsset("Assets/Sprites/Viking Sprites.png", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
            AssetDatabase.ImportAsset("Assets/Sprites/Pirate Sprites.png", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
            AssetDatabase.ImportAsset("Assets/Sprites/Knight Sprites.png", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
#endif
            //Change Player color
            if (GetComponent<PlayerController>().hasRing1 || PersistentManager.Instance.hasRing2)
            {
                Texture2D playerTexture = GetComponent<SpriteRenderer>().sprite.texture;
                Color32 oldColor = new Color32();
                Color32 newColor = new Color32();
                if (playerTexture.name == "Link Sprites")
                {
                    oldColor = new Color32(128, 208, 16, 255);
                    newColor = new Color32(184, 184, 248, 255);
                    if(PersistentManager.Instance.hasRing2)
                        newColor = new Color32(216, 40, 0, 255);
                }
                if (playerTexture.name == "Viking Sprites")
                {
                    oldColor = new Color32(47, 31, 5, 255);
                    newColor = new Color32(27, 58, 141, 255);
                    if (PersistentManager.Instance.hasRing2)
                        newColor = new Color32(135, 0, 0, 255);
                }
                if (playerTexture.name == "Pirate Sprites")
                {
                    oldColor = new Color32(26, 26, 26, 255);
                    newColor = new Color32(27, 58, 141, 255);
                    if (PersistentManager.Instance.hasRing2)
                        newColor = new Color32(135, 0, 0, 255);
                }
                if (playerTexture.name == "Knight Sprites")
                {
                    oldColor = new Color32(128, 208, 16, 255);
                    newColor = new Color32(184, 184, 248, 255);
                    if (PersistentManager.Instance.hasRing2)
                        newColor = new Color32(216, 40, 0, 255);
                }

                Color32[] cols = playerTexture.GetPixels32();
                for (var i = 0; i < cols.Length; ++i)
                    if (cols[i].Equals(oldColor))
                        cols[i] = newColor;

                playerTexture.SetPixels32(cols);
                playerTexture.Apply(true);
            }
        }
    }
    public void ReverseLoadUiItems()
    {
        //Menu Loading
        if (name == "Menu Book" && !PersistentManager.Instance.hasBook)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Ladder" && !PersistentManager.Instance.hasLadder)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Raft" && !PersistentManager.Instance.hasRaft)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Power Bracelet" && !PersistentManager.Instance.hasPowerBracelet)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Blue Ring" && !PersistentManager.Instance.hasRing)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Red Ring" && !PersistentManager.Instance.hasRing2)
            gameObject.GetComponent<Renderer>().enabled = false;
        if (name == "Menu Skeleton Key" && !PersistentManager.Instance.hasSkeletonKey)
            gameObject.GetComponent<Renderer>().enabled = false;
    }
}