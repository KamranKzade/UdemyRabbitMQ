using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyRabbitMQWeb.Watermark.Models;

public class Product
{
	[Key] // Primary Key oldugunu deyirik
	public int Id { get; set; }
	[StringLength(100)] // Uzunlugu max 100 edirik
	public string Name { get; set; }
	[Column(TypeName = "decimal(18,2)")] // Toplam 18 xarakter uzunlugunda ola biler, vergulden sonra 2 xarakter olacaq
	public decimal Price { get; set; }
	[Range(1, 100)] // Araliq 1-100 arasi ola biler Stock
	public int Stock { get; set; }
	[StringLength(100)] // Uzunlugu max 100 edirik
	public string ImageName { get; set; }
}
