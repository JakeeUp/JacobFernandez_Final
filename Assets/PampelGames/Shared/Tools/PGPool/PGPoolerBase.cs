// ---------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ---------------------------------------------------

using UnityEngine;
using UnityEngine.Pool;

namespace PampelGames.Shared.Tools
{
    /// <summary>
    ///     Abstract class for creating <see cref="ObjectPool{T}" />s in <see cref="PGPool" />.
    /// </summary>
    public abstract class PGPoolerBase : MonoBehaviour
    {
        
        public void Release(GameObject obj)
        {
            PGPool.Release(obj);
        }

        #region Protected
        protected GameObject[] InitializePool(GameObject prefab, int preloadAmount, bool limited)
        {
            var pool = PGPool.TryGetExistingPool(prefab) ?? new ObjectPool<GameObject>(
                () => CreateSetup(prefab),
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                true,
                preloadAmount,
                preloadAmount);
            return PGPool.Preload(prefab, pool, preloadAmount, limited);
        }
        

        #endregion

        /********************************************************************************************************************************/

        #region Virtual

        protected virtual GameObject CreateSetup(GameObject prefab)
        {
            var obj = Instantiate(prefab);
            return obj;
        }

        protected virtual void GetSetup(GameObject obj)
        {
            obj.SetActive(true);
        }

        protected virtual void ReleaseSetup(GameObject obj)
        {
            obj.SetActive(false);
        }

        protected virtual void DestroySetup(GameObject obj)
        {
            Destroy(obj);
        }

        #endregion
    }
}