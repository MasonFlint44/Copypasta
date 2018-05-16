using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copypasta.Models.Interfaces
{
    public interface ISimpleNotificationModel
    {
        string Title { get; }
        string Content { get; }
    }
}
