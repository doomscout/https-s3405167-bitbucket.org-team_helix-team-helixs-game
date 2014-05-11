using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : Cleanable{

	private static ProjectileManager pm;

	public bool FinishedAnimation;

	private List<GameObject> projectiles;
	private bool hasFired;

	private Vector3 minV;
	private Vector3 maxV;

	private ProjectileManager() {
		init();

		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	private void init() {
		FinishedAnimation = true;
		projectiles = new List<GameObject>();
		hasFired = true;
		
		minV = new Vector3();
		maxV = new Vector3();
	}

	public static ProjectileManager getInstance() {
		if (pm == null) {
			pm = new ProjectileManager();
		}
		return pm;
	}

	public void CleanUp() {
		int count = projectiles.Count;
		for (int i = 0; i < count; i++) {
			GameObject.Destroy(projectiles[i]);
		}
		GameTools.GameCamera.moveCameraNormal();

		init();
	}

	public void fireProjectiles() {
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
			init();
		}
	}

	public void signalCompletion(GameObject o) {
		if (projectiles.Contains(o)) {
			projectiles.Remove(o);
		} else {
			Debug.LogError("projectiles does not contain gameobject");
		}
	}

	public void queueProjectile(Spell s, Vector3 initPos, Vector3 destPos) {
		GameObject projectile = Object.Instantiate(Resources.Load("Prefabs/ProjectilePrefab", typeof(GameObject))) as GameObject;
		Projectile script = projectile.GetComponent<Projectile>();
		script.init(s, initPos, destPos);
		projectiles.Add(projectile);
		hasFired = false;
		FinishedAnimation = false;
	}

	private void initCoOrdinates() {
		minV.x = float.MaxValue;
		minV.y = float.MaxValue;
		minV.z = float.MaxValue;
		maxV.x = float.MinValue;
		maxV.y = float.MinValue;
		maxV.z = float.MinValue;
	}
}

