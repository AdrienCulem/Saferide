using PocketSphinx;

namespace Saferide.Droid.SpeechToText
{
    public class SpeechResultEvent
    {
        public SpeechResultEvent(Hypothesis hypothesis, bool finalResult)
        {
            Hypothesis = hypothesis;
            FinalResult = finalResult;
        }

        public Hypothesis Hypothesis { get; set; }
        public bool FinalResult { get; set; }
    }
}