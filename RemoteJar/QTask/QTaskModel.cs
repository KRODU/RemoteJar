using RemoteJar.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteJar.QTask
{
    public abstract class QTaskModel : ICloneable
    {
        public Client Client { get; set; }
        public virtual int CurRetryCnt { get; private set; }
        public virtual int MaxRetryCnt { get; private set; } = 5;
        public virtual bool AddRetryCnt()
        {
            if (++CurRetryCnt <= MaxRetryCnt)
                return true;
            else
                return false;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
