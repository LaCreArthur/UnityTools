using System;
using UnityEngine;

namespace AS.Toolbox.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionExpression { get; private set; }

        public ShowIfAttribute(string conditionExpression)
        {
            ConditionExpression = conditionExpression;
        }
    }
}
