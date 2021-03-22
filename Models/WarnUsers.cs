namespace kevintrinh1227.Models {
    public class WarnUsers : Entity {

        public int warningNumber { get; set; }

        public ulong memberId { get; set; }

        public string reason { get; set; }

    }
}
