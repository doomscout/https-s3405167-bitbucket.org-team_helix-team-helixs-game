using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ProjectileManager {

	public static bool FinishedAnimation = false;
	private static List<GameObject> projectiles = new List<GameObject>();
	private static bool hasFired = true;

	private static Vector3 minV = new Vector3();
	private static Vector3 maxV = new Vector3();

	public static void fireProjectiles() {
		if (!hasFired) {
			initCoOrdinates();
			for (int i = 0; i < projectiles.Count; i++) {
				Projectile script = projectiles[i].GetComponent<Projectile>();
				script.shootIn((float)i * 0.5f);
				minV = Vector3.Min(minV, Vector3.Min(script.initPos, script.destPos));
				maxV = Vector3.Max(maxV, Vector3.Max(script.initPos, script.destPos));
			}
			GameTools.GameCamera.moveCameraProjectiles(minV, maxV);
			hasFired = true;
		}
		if (projectiles.Count == 0) {
			projectiles = new List<GameObject>();
			FinishedAnimation = true;
			GameTools.GameCamera.moveCameraNormal();
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

	private static void initCoOrdinates() {
		minV.x = float.MaxValue;
		minV.y = float.MaxValue;
		minV.z = float.MaxValue;
		maxV.x = float.MinValue;
		maxV.y = float.MinValue;
		maxV.z = float.MinValue;
	}
}

