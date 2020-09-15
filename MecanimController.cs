/*
 * Copyright (c) 2020 Kristopher Gay
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using UnityEngine;
using System.Collections.Generic;

namespace KGTools.Animation
{
	/// <summary>
	/// This component exposes an interface for interacting with unity animatione events in a 
	/// </summary>
	[RequireComponent(typeof(UnityEngine.Animator))]
	public class MecanimController : MonoBehaviour
	{

		#region Data

		[SerializeField]
		private UnityEngine.Animator animationController = null;
		/// <summary>
		/// Actual raw aniamtion controller that this MecanimController should be listening to for creating 
		/// </summary>
		public UnityEngine.Animator AnimationController
		{
			get
			{
				return this.animationController;
			}
		}

		/// <summary>
		/// List of event strings and callbacks for that event string.
		/// </summary>
		private Dictionary<string, System.Action> eventListeners = null;

		/// <summary>
		/// Secondary list of events that should be triggered only once and then discarded.
		/// </summary>
		private Dictionary<string, System.Action> oneShotEventListeners = null;

		#endregion

		#region Monobehaviour

		/// <summary>
		/// On reset grab the animator component we are controlling.
		/// </summary>
		protected virtual void Reset()
		{
			this.animationController = this.GetComponent<UnityEngine.Animator>();
		}

		#endregion

		#region Animation Events

		/// <summary>
		/// This function will cause the animation events to trigger.
		/// Generally called from an animation via the animator system.
		/// </summary>
		/// <param name="eventKey">The key for the event to trigger.</param>
		public void AnimationEvent(string eventKey)
		{
			// Trigger one shot events first, then trigger non-one shot events.
			if (this.oneShotEventListeners != null && this.oneShotEventListeners.ContainsKey(eventKey))
			{
				this.oneShotEventListeners[eventKey].Invoke();
				this.oneShotEventListeners.Remove(eventKey);
			}

			if (this.eventListeners != null && this.eventListeners.ContainsKey(eventKey))
			{
				this.eventListeners[eventKey].Invoke();
			}

		}

		/// <summary>
		/// This will automatically set a random float 
		/// </summary>
		/// <param name="floatName">The name of the float variable to set.</param>
		public void SetRandomFloat(string floatName)
		{
			this.animationController.SetFloat(floatName, UnityEngine.Random.value);
		}

		#endregion

		#region Listener Management

		/// <summary>
		/// Adds a listener to the list of repeating listeners.
		/// The callback will be called every time the triggering event is called from the animation.
		/// </summary>
		/// <param name="eventKey">The event key that this function should listen for.</param>
		/// <param name="callback">The callback function that should get called when the specified trigger event key is </param>
		public void AddListener(string eventKey, System.Action callback)
		{
			if (this.eventListeners == null)
			{
				this.eventListeners = new Dictionary<string, System.Action>();
			}

			this.AddListenerToDictionary(this.eventListeners, eventKey, callback);
		}

		/// <summary>
		/// Adds a one shot event listener.
		/// </summary>
		/// <param name="eventKey">The event to trigger the listener on.</param>
		/// <param name="oneShot">The function to call when the event triggers.</param>
		public void AddOneShotListener(string eventKey, System.Action oneShot)
		{
			if (this.eventListeners == null)
			{
				this.oneShotEventListeners = new Dictionary<string, System.Action>();
			}

			this.AddListenerToDictionary(this.oneShotEventListeners, eventKey, oneShot);
		}

		/// <summary>
		/// Removes a listener from the listing of event listeners.
		/// </summary>
		/// <param name="eventKey">The event key for the </param>
		/// <param name="callbackToRemove">The callback to remove.</param>
		public void RemoveListener(string eventKey, System.Action callbackToRemove)
		{
			if (this.eventListeners != null)
			{
				this.RemoveListenerFromDictionary(this.eventListeners, eventKey, callbackToRemove);
			}
		}

		/// <summary>
		/// Removes a one shot event listener from the list of one shot events.
		/// </summary>
		/// <param name="eventKey">The event key to remove the listener listening to.</param>
		/// <param name="oneShotToRemove">The event to remove.</param>
		public void RemoveOneShotListener(string eventKey, System.Action oneShotToRemove)
		{
			if (this.oneShotEventListeners != null)
			{
				this.RemoveListenerFromDictionary(this.oneShotEventListeners, eventKey, oneShotToRemove);
			}
		}

		/// <summary>
		/// Removes all listeners for the specified event key from both the event listener and one shot lists.
		/// </summary>
		/// <param name="eventKey">Event key to remove all events for.</param>
		public void RemoveAllListeners(string eventKey)
		{
			if (this.eventListeners != null)
			{
				this.eventListeners.Remove(eventKey);
			}

			if (this.oneShotEventListeners != null)
			{
				this.oneShotEventListeners.Remove(eventKey);
			}
		}

		/// <summary>
		/// Drop all events being tracked by this controller.
		/// </summary>
		public void ClearAllEvents()
		{
			this.eventListeners.Clear();
			this.oneShotEventListeners.Clear();
		}

		/// <summary>
		/// Adds the provided listener to a dictionary.
		/// </summary>
		/// <param name="dict">Dictionary to add the event to.</param>
		/// <param name="eventKey">The event key to listen for.</param>
		/// <param name="callback">Callback to add.</param>
		private void AddListenerToDictionary(Dictionary<string, System.Action> dict, string eventKey, System.Action callback)
		{
			if (dict.ContainsKey(eventKey))
			{
				dict[eventKey] += callback;
			}
			else
			{
				dict[eventKey] = callback;
			}
		}

		/// <summary>
		/// Removes a listener callback from the provided dictionary.
		/// </summary>
		/// <param name="dict">The dictionary to remove the callback from.</param>
		/// <param name="eventKey">The event key to remove the event from listening to.</param>
		/// <param name="callbackToRemove">Callback to remove.</param>
		private void RemoveListenerFromDictionary(Dictionary<string, System.Action> dict, string eventKey, System.Action callbackToRemove)
		{
			if (dict.ContainsKey(eventKey))
			{
				dict[eventKey] -= callbackToRemove;

				if (dict[eventKey] == null || dict[eventKey].GetInvocationList().Length == 0)
				{
					dict.Remove(eventKey);
				}
			}
		}

		#endregion

		#region Passthrough Fuctions

		/// <summary>
		/// Trigger the trigger.
		/// </summary>
		/// <param name="trigger">ID of the trigger to call on the animator.</param>
		public void Trigger(string trigger)
		{
			this.animationController.SetTrigger(trigger);
		}

		/// <summary>
		/// Manually reset the trigger with the provided ID.
		/// </summary>
		/// <param name="triggerName">ID of the tigger to call on the animator.</param>
		public void ResetTrigger(string triggerName)
		{
			this.animationController.ResetTrigger(triggerName);
		}

		/// <summary>
		/// Sets a boolean value on the animator.
		/// </summary>
		/// <param name="boolID">The ID of the boolean value to set.</param>
		/// <param name="boolValue">The new value for the boolean.</param>
		public void SetBool(string boolID, bool boolValue)
		{
			this.animationController.SetBool(boolID, boolValue);
		}

		/// <summary>
		/// Sets the float value for the 
		/// </summary>
		/// <param name="floatID">The ID of the float value to set.</param>
		/// <param name="floatValue">The new value for the float variable.</param>
		public void SetFloat(string floatID, float floatValue)
		{
			this.animationController.SetFloat(floatID, floatValue);
		}

		/// <summary>
		/// Sets an integer on the animator that this controller is controlling.
		/// </summary>
		/// <param name="intID">The name of the int to set.</param>
		/// <param name="intValue">New value for the int.</param>
		public void SetInteger(string intID, int intValue)
		{
			this.animationController.SetInteger(intID, intValue);
		}

		#endregion

	}
}
