using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastRangeIndicator : Cleanable{

    private List<GameObject> Pool;
    private Dictionary<Pair, int> CoOrds;
	private List<Entity> ToggledEntities;
	private int[,] SpellCoOrds;

    public CastRangeIndicator() {
        this.CoOrds = new Dictionary<Pair, int>();
        this.Pool = new List<GameObject>();
		this.ToggledEntities = new List<Entity>();

		CleanTools.GetInstance().SubscribeCleanable(this, true);
    }

    public void ShowIndicators() {
        int count = 0;
        foreach (KeyValuePair<Pair, int> kvp in CoOrds) {
			if (kvp.Value > 0) {
				bool isAble = true;
				if (SpellCoOrds != null) {
					for (int i = 0; i < SpellCoOrds.GetLength(0); i++) {
						if (SpellCoOrds[i, 0] == kvp.Key.First && SpellCoOrds[i, 1] == kvp.Key.Second) {
							isAble = false;
						}
					}
				}
				if (count >= Pool.Count) {
					doublePool();
				}
				if (isAble) {
                	Pool[count].transform.position = new Vector3(kvp.Key.First, 0, kvp.Key.Second);
				} else {
					Pool[count].transform.position = new Vector3(-110, count, 0);
				}
            } else {
                Pool[count].transform.position = new Vector3(-110, count, 0);
            }
            count++;
        }
    }

	public void CleanUp() {
		for (int i = 0; i < Pool.Count; i++) {
			GameObject.Destroy(Pool[i]);
		}
	}

	public void updateSpellCoOrds(int[,] sc) {
		if (sc.GetLength(0) != 0) {
			SpellCoOrds = sc;
		}
	}

    private void doublePool() {
        int size = Pool.Count;
        for (int i = 0; i < size + 1; i++) {
			GameObject o = Object.Instantiate(Resources.Load("Prefabs/CastRangeIndicatorPrefab", typeof(GameObject))) as GameObject;
            Pool.Add(o);
            o.transform.position = new Vector3(-110, size + i, 0);
            o.name = "RangeIndicator " + (size + i);
        }
    }

    public void ResetIndicators() {
        CoOrds = new Dictionary<Pair, int>();
		ToggledEntities = new List<Entity>();
        for (int i = 0; i < Pool.Count; i++) {
            Pool[i].transform.position = new Vector3(-110, i, 0);
        }
		SpellCoOrds = null;
    }

	public void ToggleUnit(Entity u) {
		if (!ToggledEntities.Contains(u)) {
            addCastRange(u);
			ToggledEntities.Add(u);
        } else {
            removeCastRange(u);
			ToggledEntities.Remove(u);
        }
    }

	private bool addCastRange(Entity u) {
        int castRange = u.MainSpell.CastRange;
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

	private bool removeCastRange(Entity u) {
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
