// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

namespace PampelGames.Shared.Tools.PGInspector
{
    /// <summary>
    /// Interface for MonoBehaviours with PGHeaderBaseInspector.
    /// </summary>
    public interface PGIHeader
    {
        public void Execute();
        public void Pause();
        public void Resume();
        public void Stop();
        public void ReInitialize();
        public bool IsExecuting();
        public bool IsPaused();
    }
}
