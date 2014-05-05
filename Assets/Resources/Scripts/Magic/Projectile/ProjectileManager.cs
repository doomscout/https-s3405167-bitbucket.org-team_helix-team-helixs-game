using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ProjectileManager {

	public static bool FinishedAnimation = false;
	private static List<GameObject> projectiles = new List<GameObject>();
	private static bool hasFired = false;

	public static void fireProjectiles() {
		if (!hasFired) {
			for (int i = 0; i < projectiles.Count; i++) {
				projectiles[i].GetComponent<Projectile>().shootIn((float)i * 0.5f);
			}
			hasFired = true;
		}
		if (projectiles.Count == 0) {
			projectiles = new List<GameObject>();
			FinishedAnimation = true;
		}
	}

	public static void signalCompletion(GameObject o) {
		if (projectiles.Contains(o)) {
			projectiles.Remove(o);
		} else {
			Debug.LogError("projectiles does not contain gameobject");
		}
	}

	public static void queueProjectile(Spell s, Vector3 initPos, Vector3 destPos) {
		GameObject projectile = Object.Instantiate(Resources.Load("Prefabs/ProjectilePrefab", typeof(GameObject))) as GameObject;
		Projectile script = projectile.GetComponent<Projectile>();
		script.init(s, initPos, destPos);
		projectiles.Add(projectile);
		hasFired = false;
		FinishedAnimation = false;
	}

}

