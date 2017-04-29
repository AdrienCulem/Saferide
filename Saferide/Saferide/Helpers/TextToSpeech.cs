using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Interfaces;
using Xamarin.Forms;

namespace Saferide.Helpers
{
    public static class TextToSpeech
    {
        public static void Talk(string text)
        {
            DependencyService.Get<ISpeechRecognition>().Talk(text);
        }
    }
}
