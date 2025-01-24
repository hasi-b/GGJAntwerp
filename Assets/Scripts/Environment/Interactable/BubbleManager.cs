using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private Bubble _bubblePrefab;
	[SerializeField] private string _test = "this is a test string";
	#endregion

	#region Fields
	private List<Bubble> _bubbles = new List<Bubble>();
	private readonly Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

	private bool _correctKeyPressed;
	private bool _doOnce = true;
	#endregion

	#region Properties

	#endregion

	#region Events
	#endregion

	#region Mono
	private void Awake()
	{
		foreach (char c in _test)
		{
			var bubble = Instantiate(_bubblePrefab);
			bubble.KeyToPress = GetKeyCode(c);
			bubble.BubbleManager = this;

			bubble.transform.position = Random.insideUnitCircle * 5;
			_bubbles.Add(bubble);

			bubble.Destroyed += OnBubbleDestroyed;
		}
	}


	private void Update()
	{
		_correctKeyPressed = false;
		_doOnce = true;

		// don't check for input of there are no bubbles
		if (_bubbles.Count <= 0) return;

		// if no input then don't check for the specific keys
		if (!Input.anyKeyDown) return;

		// go over all the bubbles to see if the any of the correct keys have been pressed
		foreach (var bubble in _bubbles)
		{
			// check if any of the keys pressed correspond with any of the bubbles currently on screen
			if (Input.GetKeyDown(bubble.KeyToPress))
			{
				bubble.Pop();
				if (!_correctKeyPressed)
				{
					// only execute this once
					Debug.Log($"the key {bubble.KeyToPress} was pressed");
					_correctKeyPressed = true;
				}
			}
		}

		// if the correct key was pressed then stop doing checks
		if (_correctKeyPressed) return;

		if (_doOnce)
		{
			_doOnce = false;
			Debug.LogError("WRONG KEY PRESSED");
		}

	}
	#endregion

	#region Methods

	private KeyCode GetKeyCode(char character)
	{
		// Get from cache if it was taken before to prevent unnecessary enum parse
		KeyCode code;
		if (_keycodeCache.TryGetValue(character, out code)) return code;

		// Cast to its integer value
		int alphaValue = character;
		code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
		_keycodeCache.Add(character, code);
		return code;
	}

	private void OnBubbleDestroyed(object sender, EventArgs e)
	{
		var bubble = sender as Bubble;

		_bubbles.Remove(bubble);
		bubble.Destroyed -= OnBubbleDestroyed;
	}
	#endregion

	#region EventHandlers
	#endregion
}
