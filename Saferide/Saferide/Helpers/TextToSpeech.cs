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
        /// <summary>
        /// Calling the platform specific code to speak the text
        /// </summary>
        /// <param name="text">The text to be spoken</param>
        public static void Talk(string text)
        {
            DependencyService.Get<ISpeechRecognition>().Talk(text);
        }
    }
}
