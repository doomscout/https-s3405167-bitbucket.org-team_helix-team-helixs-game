using UnityEngine;
using System.Collections;

public class CameraInitMove : MonoBehaviour {

	public readonly float scrollBorderX = 30.0f;
	public readonly float scrollBorderY = 30.0f;
	public readonly float scrollSpeed = 10.0f;
	public readonly float moveCameraSpeed = 3.0f;
	public readonly float scrollWheelSpeed = 200.0f;

	private Vector3 slerpPosition;
	private Vector3 oldPosition;
	private Vector3 newPosition;
	private float totalDistance;
	private float traveledDistance;
	private float timeTravelled;

	private bool moveCamera = false;

	// Use this for initialization
	void Start () {
		GameTools.GameCamera = this;
		transform.position = new Vector3(12f, 10f, 12f);
		oldPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (moveCamera) {
			float distCovered = timeTravelled * moveCameraSpeed;
			slerpPosition = transform.position;
			transform.position = Vector3.Slerp(slerpPosition, newPosition, distCovered/totalDistance);
			timeTravelled += Time.deltaTime;
			if (transform.position == newPosition) {
				moveCamera = false;
			}
		} else {
			Vector3 mousePos = Input.mousePosition;
			if (mousePos.x < scrollBorderX) {
				this.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
				oldPosition = transform.position;
			}
			if (mousePos.x > Screen.width - scrollBorderX) {
				this.transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
				oldPosition = transform.position;
			}
			if (mousePos.y < scrollBorderY) {
				this.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
				oldPosition = transform.position;
			}
			if (mousePos.y > Screen.height - scrollBorderY) {
				this.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
				oldPosition = transform.position;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0) {
				if (camera.orthographicSize < 15.0f) {
					camera.orthographicSize -=(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollWheelSpeed);
				}
			} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
				if (camera.orthographicSize > 5.0f) {
					camera.orthographicSize -=(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollWheelSpeed);

				}
			}
		}
	}

	public void MoveCameraToPlayer() {
		transform.position = new Vector3(GameTools.Player.Map_position_x, 10f, GameTools.Player.Map_position_y);
	}

	public void moveCameraProjectiles(Vector3 minV, Vector3 maxV) {
		newPosition = (minV + maxV)/2.0f;
		float y = (maxV.z-newPosition.z)/ Mathf.Tan((camera.fieldOfView/2.0f) * Mathf.PI / 180.0f);
		if (y > oldPosition.y) {
			newPosition.y = y + 2.0f;
		} else {
			newPosition.y = oldPosition.y;
		}
		if (newPosition.y < 5.0f) {
			newPosition.y = 5.0f;
		}		      
		moveCamera = true;
		oldPosition = transform.position;
		totalDistance = Vector3.Distance(newPosition, transform.position);
		timeTravelled = 0.0f;
	}

	public void moveCameraNormal() {
		if (oldPosition != transform.position) {
			newPosition = oldPosition;
			totalDistance = Vector3.Distance(newPosition, transform.position);
			timeTravelled = 0.0f;
			moveCamera = true;
		}
	}
}
