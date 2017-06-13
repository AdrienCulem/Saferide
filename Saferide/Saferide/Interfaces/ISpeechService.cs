﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Interfaces
{
    public interface ISpeechService
    {
        void StartListening(string mode);
        Task StopListening();
        Task Setup();
        void ChangeKeyPhrase(string keyPhrase);
    }
}