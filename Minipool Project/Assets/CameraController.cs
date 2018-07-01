using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using NetworkManagement;
using BallPool.Mechanics;
using BallPool.AI;
using BallPool;

namespace BallPool
{
	public class CameraController : MonoBehaviour
	{
		public enum CameraMode
		{
			FollowCueBall = 0,
			Locked,
			Standard
		};


		public CameraMode cameraMode;
		public ShotController shotController;
		public GameObject cueBall;

		[SerializeField] private Transform cameraStandardPosition;
		[SerializeField] private Transform cameraLockedPosition;
		[SerializeField] private Transform cameraInShotPosition;

		public float cameraOffsetY;
		public float cameraOffsetX;

		private Vector3 desiredPosition;


		private const float Y_ANGLE_MIN = 0.0f;
		private const float Y_ANGLE_MAX = 50.0f;

		public float distance = 10.0f;

		private float currentX = 0.0f;
		private float currentY = 45.0f;
		private float sensitivityX = 4.0f;
		private float sensitivityY = 1.0f;

		private Quaternion rotation;
		private Quaternion startRotation;
		private Quaternion desiredRotation;


		void Start(){
			startRotation = transform.rotation;
		}

		void FixedUpdate()
		{
			cueBall = shotController.cueBall.gameObject;

			DetermineCameraMode ();
		}

		void LateUpdate(){

			Vector3 offsetPosition = new Vector3 (cueBall.transform.position.x + cameraOffsetX, cueBall.transform.position.y + cameraOffsetY, cueBall.transform.position.z);


			if (cameraMode == CameraMode.Standard) {
				currentX += -InputOutput.xrotationDir;
				Vector3 dir = new Vector3(cameraOffsetX, cameraOffsetY, 0);
				Quaternion rotation = Quaternion.Euler(0, currentX, 0);
				transform.position = cueBall.transform.position + rotation * dir;
				transform.LookAt(cueBall.transform.position);




//				rotation = Quaternion.Euler(0, currentX, 0);
//			
//				desiredPosition = rotation * transform.position;
//
//				transform.LookAt(cueBall.transform.position);
//
//				desiredRotation = transform.rotation;

			} else if (cameraMode == CameraMode.Locked) {

				rotation = Quaternion.Euler (0, currentX, 0);
				desiredPosition = transform.position;
				//desiredRotation = transform.rotation;

			} else if (cameraMode == CameraMode.FollowCueBall) {
				desiredPosition = offsetPosition;
				//desiredRotation = startRotation;
			}

			transform.LookAt (cueBall.transform);
			//transform.position = Vector3.Lerp (transform.position, desiredPosition, 10.0f * Time.deltaTime);
			//transform.rotation = Quaternion.Lerp (transform.rotation, desiredRotation, 10.0f * Time.deltaTime);
			StartCoroutine (MoveCameraToStandartPosition ());

		}



        IEnumerator MoveCameraToStandartPosition()
        {
			yield return new WaitForFixedUpdate();
			transform.position = Vector3.Lerp (transform.position, desiredPosition, 50.0f * Time.deltaTime);
			//transform.rotation = Quaternion.Lerp (transform.rotation, desiredRotation, 20.0f * Time.deltaTime);
        }



		void DetermineCameraMode()
		{
			if (shotController.cueStateType == ShotController.CueStateType.RotationMode) {
				cameraMode = CameraMode.Standard;
			} else if (shotController.cueStateType == ShotController.CueStateType.TargetingAtTargetBall) {
				cameraMode = CameraMode.Locked;
			} else if (shotController.cueStateType == ShotController.CueStateType.Non){
				cameraMode = CameraMode.FollowCueBall;
//				Debug.Log ("NON");
			}
		}
	}
}
