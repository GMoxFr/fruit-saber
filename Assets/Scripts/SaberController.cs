using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SaberController : MonoBehaviour
{
	public Transform handTransform; // Assign the hand transform in the Inspector

	private Rigidbody rb;
	private Vector3[] previousPositions;
	private int positionIndex;
	private int positionCount;

	// private XRBaseController ownController;
	// private XRIDefaultInputActions inputActions;
	// private InputAction triggerAction;

	// void Awake()
	// {
	// 	inputActions = new XRIDefaultInputActions();
	// }

	// void OnEnable()
	// {
	// 	if (gameObject.CompareTag("RedSaber"))
	// 	{
	// 		inputActions.XRIRightHandInteraction.Activate.Enable();
	// 	}
	// 	else if (gameObject.CompareTag("LeftSaber"))
	// 	{
	// 		inputActions.XRILeftHandInteraction.Activate.Enable();
	// 	}

	// 	triggerAction.Enable();
	// }

	// void OnDisable()
	// {
	// 	triggerAction.Disable();
	// }

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			Debug.LogError("No Rigidbody found on the saber.");
			return;
		}

		// Initialize the previous positions array
		previousPositions = new Vector3[3]; // Tracking the last 3 positions
		positionIndex = 0;
		positionCount = 0;

		// if (gameObject.CompareTag("BlueSaber"))
		// {
		// 	ownController = GameObject.FindGameObjectWithTag("BlueController").GetComponent<XRBaseController>();
		// }
		// else if (gameObject.CompareTag("RedSaber"))
		// {
		// 	ownController = GameObject.FindGameObjectWithTag("RedController").GetComponent<XRBaseController>();
		// }
	}

	// void Update()
	// {
	// 	// Wait for input to start the game
	// 	if (GameManager.instance.gameState == GameState.WaitingForStart)
	// 	{
	// 		if (triggerAction.triggered)
	// 		{
	// 			GameManager.instance.StartGame();
	// 		}
	// 	}
	// }

	// private bool CheckTriggerButton(XRController controller)
	// {
	// 	if (controller.inputDevice.IsValid)
	// 	{
	// 		bool triggerButtonValue;
	// 		if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue))
	// 		{
	// 			return triggerButtonValue;
	// 		}
	// 	}
	// 	return false;
	// }

	void FixedUpdate()
	{
		if (handTransform != null)
		{
			// Update the previous positions array
			previousPositions[positionIndex] = handTransform.position;
			positionIndex = (positionIndex + 1) % previousPositions.Length;
			if (positionCount < previousPositions.Length)
			{
				positionCount++;
			}
		}
	}

	public Vector3 GetAverageVelocity()
	{
		if (positionCount < 2)
		{
			return Vector3.zero;
		}

		Vector3 totalDisplacement = Vector3.zero;
		for (int i = 0; i < positionCount - 1; i++)
		{
			int currentIndex = (positionIndex + i) % previousPositions.Length;
			int nextIndex = (currentIndex + 1) % previousPositions.Length;
			totalDisplacement += previousPositions[nextIndex] - previousPositions[currentIndex];
		}

		return totalDisplacement / ((positionCount - 1) * Time.fixedDeltaTime);
	}
}
