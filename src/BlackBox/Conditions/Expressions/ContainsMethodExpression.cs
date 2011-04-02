using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackBox.Conditions.Expressions
{
    [MethodExpression("contains", 2)]
    internal class ContainsMethodExpression : MethodExpression
    {
        internal ContainsMethodExpression(ConditionExpression[] arguments)
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
                // Check if the actual string contains the expected one.
                return actual.IndexOf(expected, 0, StringComparison.OrdinalIgnoreCase) > -1;
            }

            return false;
        }
    }
}
