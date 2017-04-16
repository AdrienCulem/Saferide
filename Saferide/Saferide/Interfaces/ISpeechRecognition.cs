using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Interfaces
{
    public interface ISpeechRecognition
    {
        Task<String> Listen();
        void Talk(string textToSay);
    }
}
