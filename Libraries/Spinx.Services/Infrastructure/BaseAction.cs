using Spinx.Core;
using System.Collections.Generic;

namespace Spinx.Services.Infrastructure
{
    public abstract class BaseAction
    {
        public virtual Result Apply(IEnumerable<int> ids)
        {
            return null;
        }

        public virtual Result Apply(int siteId, IEnumerable<int> ids)
        {
            return null;
        }
    }
}