﻿namespace Clover
{
    public abstract class KernelUnit
    {
        public static T Create<T>() where T : KernelUnit, new()
        {
            T unit = new T();
            return unit;
        }

        internal CloverKernel Kernel { get; set; }

        public void Pause(bool isPause)
        {
            if (isPause)
            {
                Background();
            }
            else
            {
                Foreground();
            }
        }

        public virtual void OnBuild() { }
        public virtual void Update(float deltaTime) { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
        public virtual void Reset() { }
        public virtual void Destroy() { }
        public virtual void Quit() { }
        public virtual void OnLowMemory() { }
        public virtual void Dump(VarList args) { }
        protected virtual void Background() { }
        protected virtual void Foreground() { }
    }
}