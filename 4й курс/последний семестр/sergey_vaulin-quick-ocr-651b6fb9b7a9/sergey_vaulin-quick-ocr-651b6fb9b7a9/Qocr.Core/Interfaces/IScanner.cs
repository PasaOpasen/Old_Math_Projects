using System.Collections.Generic;

using Qocr.Core.Recognition.Data;

namespace Qocr.Core.Interfaces
{
    /// <summary>
    /// Сканер изображения, даёт возможность на изображении найти все отрывки изображений.
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Получить фрагменты исходного изображения.
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <returns></returns>
        IList<QSymbol> GetFragments(IMonomap sourceImage);
    }
}