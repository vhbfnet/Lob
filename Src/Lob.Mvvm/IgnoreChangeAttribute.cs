using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// This attribute can be used with a property to ignore changes in a view model
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreChangeAttribute : Attribute
    {
    }
}
