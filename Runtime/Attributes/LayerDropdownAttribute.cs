using System;
using UnityEngine;

namespace AS.Toolbox.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LayerDropdownAttribute : PropertyAttribute {}
}
