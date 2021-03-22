using System.ComponentModel.DataAnnotations;

namespace kevintrinh1227.Models {
    public abstract class Entity {

        [Key] public int Id { get; set; }

    }
}
