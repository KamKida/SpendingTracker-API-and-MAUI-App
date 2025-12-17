using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Interfaces.ContextServices
{
    public interface IUserContextService
    {
        Guid? GetUserId();
    }
}
