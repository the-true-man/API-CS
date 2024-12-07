namespace JSONAPI;

public class Product
{
    public int Id {get; set;}
    public required string UrlPhotoProduct {get; set;}
    public required string NameProduct {get; set;}
    public string? DescribeProduct {get; set;}
    public required int PriceProduct {get; set;}
}
