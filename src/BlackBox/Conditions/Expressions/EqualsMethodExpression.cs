using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackBox.Conditions.Expressions
{
    [MethodExpression("equals", 2)]
    internal class EqualsMethodExpression : MethodExpression
    {
        internal EqualsMethodExpression(ConditionExpression[] arguments)
            : base(arguments)
        {
        }

        internal override object Evaluate(ILogEntry context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string actual = this.Arguments[0].Evaluate(context) as string;
            string expected = this.Arguments[1].Evaluate(context) as string;

            if (actual != null && expected != null)
            {
                // Check if the actual string equals the expected one.
                return actual.Equals(expected, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
