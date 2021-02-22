using System;

using Qocr.Core.Interfaces;

namespace Qocr.Core.Data
{
    /// <summary>
    /// Базовый класс для монохромного изображения.
    /// </summary>
    public abstract class MonomapBase : IMonomap
    {
        /// <inheritdoc/>
        public virtual int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public virtual int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public virtual bool this[int x, int y]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}