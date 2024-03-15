namespace OOPCar
{
    public sealed class Car
    {
        public Car(string Model = "", ushort MaxKPH = 0)
        {
            this.Model = Model;
            this.MaxKPH = MaxKPH;
        }
        public string Model { get; private set; }
        public ushort MaxKPH { get; private set; }
        public Car UpdateData(string model = null, ushort kph = 0)
        {
            this.Model = model != this.Model ? model : this.Model;
            this.MaxKPH = kph != this.MaxKPH ? kph : this.MaxKPH;
            return this;
        }
    }
}