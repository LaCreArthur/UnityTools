using System;
using UnityEngine;

namespace AS.Toolbox.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HideIfAttribute : PropertyAttribute
    {
        public string ConditionExpression { get; private set; }

        public HideIfAttribute(string conditionExpression)
        {
            ConditionExpression = conditionExpression;
        }
    }
}
