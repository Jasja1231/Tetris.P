using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.ObserverDP
{
    /// <summary>
    /// The 'Observer' abstract class
    /// </summary>
    public interface Observer
    {
        /// <summary>
        /// Update the observer.
        /// </summary>
        /// <param name="arg">The argument for update.Specifies which part to update.</param>
        void Update(int arg);
    }
}
