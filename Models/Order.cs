using System.Net;
using System.Net.Http.Headers;

namespace JSONAPI;

public class Order
{
    public int Id {get; set;}
    public int TotalCost {get; set;}
    public DateTime DateTimeOrder {get; set;} = DateTime.Now;
    public required List<Product> Products {get; set;}
}
