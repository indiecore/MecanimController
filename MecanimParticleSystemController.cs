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
using System.Collections.Generic;
using UnityEngine;

namespace KGTools.Animation
{
	/// <summary>
	/// This class acts as a go between for simple particle system control via animation events in Unity's Mecanim System.
	/// It addresses particle systems by name and will apply all activity to all the sub-emitters of the controller as well.
	/// </summary>
	public class MecanimParticleSystemController : MonoBehaviour
	{

		#region Data

		/// <summary>
		/// List of controlled particle systems.
		/// </summary>
		[SerializeField]
		private List<ParticleSystem> controlledSystemList = null;

		/// <summary>
		/// Dictionary containing the particle systems referenced by their game object name.
		/// </summary>
		private Dictionary<string, ParticleSystem> particleSystems = null;

		/// <summary>
		/// Flag indicating that this particle system has been set up.
		/// </summary>
		private bool initialized = false;

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// The particle system controller needs to be able to reference particle system by name.
		/// This is achieved by using the name of the gameobject they are attached to.
		/// </summary>
		protected virtual void Awake()
		{
			if (!this.initialized)
			{

				if (this.controlledSystemList == null)
				{
					this.controlledSystemList = new List<ParticleSystem>();
				}

				this.particleSystems = new Dictionary<string, ParticleSystem>(this.controlledSystemList.Count);
				for (int i = 0; i < this.controlledSystemList.Count; ++i)
				{
					this.particleSystems.Add(this.controlledSystemList[i].gameObject.name, this.controlledSystemList[i]);
				}

				this.initialized = true;
			}
		}

		#endregion

		#region Logic

		/// <summary>
		/// Sets controlled particle system to play.
		/// </summary>
		/// <param name="particleSystemName">The name of the particle system to play.</param>
		public void PlayParticleSystem(string particleSystemName = null)
		{
			if (!this.initialized)
			{
				Debug.LogWarningFormat("MecanimParticleSystemController {0} has not been initialized. It must be active before calling functions.", this.name);
				return;
			}

			if (string.IsNullOrEmpty(particleSystemName))
			{
				for (int i = 0; i < this.controlledSystemList.Count; ++i)
				{
					this.controlledSystemList[i].Play(true);
				}
			}
			else
			{
				ParticleSystem activeSystem = null;
				if (this.particleSystems.TryGetValue(particleSystemName, out activeSystem))
				{
					activeSystem.Play(true);
				}
				else
				{
					Debug.LogErrorFormat("No particle system attached to gameobject named {0} is controlled by MecanimParticleSystemController {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Sets controlled particle system to paused.
		/// </summary>
		/// <param name="particleSystemName">The name of the particle system to paused.</param>
		public void PauseParticleSystem(string particleSystemName = null)
		{
			if (!this.initialized)
			{
				Debug.LogWarningFormat("MecanimParticleSystemController {0} has not been initialized. It must be active before calling functions.", this.name);
				return;
			}

			if (string.IsNullOrEmpty(particleSystemName))
			{
				for (int i = 0; i < this.controlledSystemList.Count; ++i)
				{
					this.controlledSystemList[i].Pause(true);
				}
			}
			else
			{
				ParticleSystem activeSystem = null;
				if (this.particleSystems.TryGetValue(particleSystemName, out activeSystem))
				{
					activeSystem.Pause(true);
				}
				else
				{
					Debug.LogErrorFormat("No particle system attached to gameobject named {0} is controlled by MecanimParticleSystemController {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Sets controlled particle system to stopped.
		/// </summary>
		/// <param name="particleSystemName">The name of the particle system to stopped.</param>
		public void StopParticleSystem(string particleSystemName = null)
		{
			if (!this.initialized)
			{
				Debug.LogWarningFormat("MecanimParticleSystemController {0} has not been initialized. It must be active before calling functions.", this.name);
				return;
			}

			if (string.IsNullOrEmpty(particleSystemName))
			{
				for (int i = 0; i < this.controlledSystemList.Count; ++i)
				{
					this.controlledSystemList[i].Stop(true);
				}
			}
			else
			{
				ParticleSystem activeSystem = null;
				if (this.particleSystems.TryGetValue(particleSystemName, out activeSystem))
				{
					activeSystem.Stop(true);
				}
				else
				{
					Debug.LogErrorFormat("No particle system attached to gameobject named {0} is controlled by MecanimParticleSystemController {1}", particleSystemName, this.name);
				}
			}
		}

		/// <summary>
		/// Clears all particles from a particular particle system.
		/// </summary>
		/// <param name="particleSystemName">The name of the particle system to clear.</param>
		public void ClearParticleSystem(string particleSystemName = null)
		{
			if (!this.initialized)
			{
				Debug.LogWarningFormat("MecanimParticleSystemController {0} has not been initialized. It must be active before calling functions.", this.name);
				return;
			}
			
			if (string.IsNullOrEmpty(particleSystemName))
			{
				for (int i = 0; i < this.controlledSystemList.Count; ++i)
				{
					this.controlledSystemList[i].Clear(true);
				}
			}
			else
			{
				ParticleSystem activeSystem = null;
				if (this.particleSystems.TryGetValue(particleSystemName, out activeSystem))
				{
					activeSystem.Clear(true);
				}
				else
				{
					Debug.LogErrorFormat("No particle system attached to gameobject named {0} is controlled by MecanimParticleSystemController {1}", particleSystemName, this.name);
				}
			}
		}

		#endregion

	}
}