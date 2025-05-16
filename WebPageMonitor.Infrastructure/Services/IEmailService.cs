using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Infrastructure.Services
{
    public interface IEmailService
    {
        string SendMsg(string reply);
    }
}
