using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomModule
{
    public enum reguTarget { ANGLE, TEMPERATURE, VELOCITY };
    public class RegulationParams
    {
        private float paramP;
        private float paramI;
        private float paramD;
        private float targetVal;
        private reguTarget regTarget;

        public reguTarget RegTarget
        {
            get { return regTarget; }
            set { regTarget = value; }
        }
        public float TargetVal
        {
            get { return targetVal; }
            set { targetVal = value; }
        }

        public float ParamD
        {
            get { return paramD; }
            set { paramD = value; }
        }
        public float ParamI
        {
            get { return paramI; }
            set { paramI = value; }
        }
        public float ParamP
        {
            get { return paramP; }
            set { paramP = value; }
        }
    }
}
