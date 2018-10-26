using Db.DbContext.Db_Models.CarModels;

namespace Db.DbContext.Db_Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public virtual City City { get; set; }
        public virtual Brand Brand { get; set; }
        public bool IsNew { get; set; }
        public virtual Country Country { get; set; }
        public virtual Engine Engine { get; set; }
        public virtual Transmission Transmission { get; set; }
        public virtual Color Color { get; set; }
        public string Distance { get; set; }
        public virtual PayType PayType { get; set; }
        public int MadeYear { get; set; }
        public virtual Fule Fule { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}