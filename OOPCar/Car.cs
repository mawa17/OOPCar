using System;

namespace OOPCar
{
    [Serializable]
    public sealed class Car
    {
        public Car(string Model = "", ushort MaxKPH = 0)
        {
            this.Model = Model;
            this.MaxKPH = MaxKPH;
        }
        public string Model { get; private set; }
        public ushort MaxKPH { get; private set; }
        public Car UpdateData(Car car) => car;
    }
}