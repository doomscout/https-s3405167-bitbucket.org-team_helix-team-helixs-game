using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastRangeIndicator {

    public static CastRangeIndicator instance;

    private List<GameObject> Pool;
    private List<GameObject> PlayerPool;
    private Dictionary<Pair, int> CoOrds;
    private Dictionary<Pair, int> PlayerCoOrds;
    private List<Unit> ToggledUnits;

    private CastRangeIndicator() {
        this.CoOrds = new Dictionary<Pair, int>();
        this.PlayerCoOrds = new Dictionary<Pair, int>();
        this.Pool = new List<GameObject>();
        this.PlayerPool = new List<GameObject>();
        this.ToggledUnits = new List<Unit>();
    }

    public static CastRangeIndicator GetInstance() {
        if (instance == null) {
            instance = new CastRangeIndicator();
        }
        return instance;
    }

    public void ShowIndicators() {
        int count = 0;
        foreach (KeyValuePair<Pair, int> kvp in CoOrds) {
            if (kvp.Value > 0) {
                if (count >= Pool.Count) {
                    doublePool();
                }
                Pool[count].transform.position = new Vector3(kvp.Key.First, 0, kvp.Key.Second);
            } else {
                Pool[count].transform.position = new Vector3(-110, count, 0);
            }
            count++;
        }
    }

    public void ShowPlayerIndicators() {
        int count = 0;
        foreach (KeyValuePair<Pair, int> kvp in PlayerCoOrds) {
            if (kvp.Value > 0) {
                if (count >= PlayerPool.Count) {
                    doublePlayerPool();
                }
                PlayerPool[count].transform.position = new Vector3(kvp.Key.First, 0, kvp.Key.Second);
            } else {
                PlayerPool[count].transform.position = new Vector3(-110, count, 0);
            }
            count++;
        }
    }

    public void loadPlayerInformation(Spell s, Player p) {
        int castRange = s.CastRange;
        if (castRange < 1) {
            return;
        }
        for (int i = -castRange; i <= castRange; i++) {
            for (int j = castRange - Mathf.Abs(i); j >= -(castRange - Mathf.Abs(i)); j--) {
                Pair coord = new Pair(p.Map_position_x + i, p.Map_position_y + j);
                if (PlayerCoOrds.ContainsKey(coord)) {
                    PlayerCoOrds[coord] = PlayerCoOrds[coord] + 1;
                } else {
                    PlayerCoOrds.Add(coord, 1);
                }
            }
        }
    }

    private void doublePool() {
        int size = Pool.Count;
        for (int i = 0; i < size + 1; i++) {
            GameObject o = Object.Instantiate(Resources.Load("Prefabs/Cube4", typeof(GameObject))) as GameObject;
            Pool.Add(o);
            o.transform.position = new Vector3(-110, size + i, 0);
            o.name = "RangeIndicator " + (size + i);
        }
    }

    private void doublePlayerPool() {
        int size = PlayerPool.Count;
        for (int i = 0; i < size + 1; i++) {
            GameObject o = Object.Instantiate(Resources.Load("Prefabs/Cube4", typeof(GameObject))) as GameObject;
            PlayerPool.Add(o);
            o.transform.position = new Vector3(-110, size + i, 0);
            o.name = "PlayerRangeIndicator " + (size + i);
        }
    }

    public void ResetIndicators() {
        CoOrds = new Dictionary<Pair, int>();
        PlayerCoOrds = new Dictionary<Pair, int>();
        ToggledUnits = new List<Unit>();
        for (int i = 0; i < Pool.Count; i++) {
            Pool[i].transform.position = new Vector3(-110, i, 0);
        }
        for (int i = 0; i < PlayerPool.Count; i++) {
            PlayerPool[i].transform.position = new Vector3(-110, i, 0);
        }
    }

    public void ToggleUnit(Unit u) {
        if (!ToggledUnits.Contains(u)) {
            addCastRange(u);
            ToggledUnits.Add(u);
        } else {
            removeCastRange(u);
            ToggledUnits.Remove(u);
        }
    }

    private bool addCastRange(Unit u) {
        int castRange = u.MainSpell.CastRange;
        Debug.Log("Cast range is " + castRange);
        if (castRange < 1) {
            return false;
        }
        for (int i = -castRange; i <= castRange; i++) {
            for (int j = castRange - Mathf.Abs(i); j >= -(castRange - Mathf.Abs(i)); j--) {
                Pair coord = new Pair(u.Map_position_x + i, u.Map_position_y + j);
                if (CoOrds.ContainsKey(coord)) {
                    CoOrds[coord] = CoOrds[coord] + 1;
                } else {
                    CoOrds.Add(coord, 1);
                }
            }
        }
        return true;
    }

    private bool removeCastRange(Unit u) {
        int castRange = u.MainSpell.CastRange;
        if (castRange < 1) {
            return false;
        }
        for (int i = -castRange; i <= castRange; i++) {
            for (int j = castRange - Mathf.Abs(i); j >= -(castRange - Mathf.Abs(i)); j--) {
                Pair coord = new Pair(u.Map_position_x + i, u.Map_position_y + j);
                if (!CoOrds.ContainsKey(coord)) {
                    Debug.LogError("Removing non-existent unit cast range indictor: " + coord.First + ", " + coord.Second);
                } else {
                    CoOrds[coord] = CoOrds[coord] - 1;
                }
            }
        }
        return true;
    }

}
