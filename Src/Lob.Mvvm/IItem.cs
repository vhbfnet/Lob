using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface définissant un élément d'une liste
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    public interface IItem<T>
    {
        /// <summary>
        /// L'item courant
        /// </summary>
        T CurrentItem { get; set; }
    }
}
