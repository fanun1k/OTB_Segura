using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
