using System.ComponentModel.DataAnnotations;

namespace Order.API.Models.Entities
{
    public class OrderOutbox
    {
        [Key]
        //bir mesajın tekil olarak tanımlanması için kullanılır
        public Guid IdempotentToken { get; set; }
        public DateTime OccuredOn { get; set; }
        //ProcessedDate flag kolon olarak kullanılabilir işleme alındı ise kullanılmış olarak değerlendirilebilir
        public DateTime? ProcessedDate { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
