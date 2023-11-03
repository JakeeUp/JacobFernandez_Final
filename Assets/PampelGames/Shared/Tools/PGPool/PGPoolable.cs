// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using UnityEngine;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Attached to all <see cref="PGPool" /> spawned objects in the scene.
    ///     If this object gets destroyed, it will trigger the automatic disposal of its pool and destruction of all objects within.
    /// </summary>
    public class PGPoolable : MonoBehaviour
    {
        public GameObject prefab { get; set; }

        /* Public ***********************************************************************************************************************/

        /// <summary>
        ///     Despawn this GameObject into the pool. Optionally call PGPool.Release(obj) directly.
        /// </summary>
        public void Release()
        {
            PGPool.Release(gameObject);
        }

        /********************************************************************************************************************************/

        internal virtual void OnPoolSpawn()
        {
        }

        internal virtual void OnPoolUnSpawn()
        {
        }

        /// <summary>
        ///     Object is currently inside a pool.
        /// </summary>
        internal bool pooled = true;


    }
}