﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copypasta
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException() { }

        public InvalidStateException(string message) : base(message) { }

        public InvalidStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
