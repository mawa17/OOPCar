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
        public Car UpdateData(Car car)
        {
            this.Model = car.Model != this.Model ? car.Model : this.Model;
            this.MaxKPH = car.MaxKPH != this.MaxKPH ? car.MaxKPH : this.MaxKPH;
            return this;
        }
    }
}