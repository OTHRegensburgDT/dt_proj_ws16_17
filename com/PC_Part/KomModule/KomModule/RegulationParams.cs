namespace KomModule
{
    public enum ReguTarget { Angle, Temperature, Velocity };
    public class RegulationParams
    {
        public ReguTarget RegTarget { get; set; }

        public float TargetVal { get; set; }

        public float ParamD { get; set; }

        public float ParamI { get; set; }

        public float ParamP { get; set; }
    }
}
