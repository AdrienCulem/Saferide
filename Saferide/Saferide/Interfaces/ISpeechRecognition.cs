using System;
using System.Threading.Tasks;

namespace Saferide.Interfaces
{
    public interface ISpeechRecognition
    {
        /// <summary>
        /// Starts listening
        /// </summary>
        /// <returns>
        /// What the user said
        /// </returns>
        Task<String> Listen();
        /// <summary>
        /// Talks the texts to say
        /// </summary>
        /// <param name="textToSay">
        /// The text to say
        /// </param>
        void Talk(string textToSay);
    }
}
