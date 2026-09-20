using System.Text.Json;

var file = "inventory.json";
var items = File.Exists(file) ? JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(file)) ?? [] : [];
var argsList = args.ToList();
if (argsList.Count == 0 || args[0] == "list") { foreach (var item in items) Console.WriteLine($"{item.Sku,-16} {item.Stock,4} ${item.Price:N2}"); return; }
if (args[0] == "add" && argsList.Count == 4 && int.TryParse(args[2], out var stock) && decimal.TryParse(args[3], out var price)) {
    var old = items.FindIndex(p => p.Sku.Equals(args[1], StringComparison.OrdinalIgnoreCase));
    if (old >= 0) items[old] = items[old] with { Stock = items[old].Stock + stock, Price = price }; else items.Add(new Product(args[1], stock, price));
} else if (args[0] == "remove" && argsList.Count == 3 && int.TryParse(args[2], out var units)) {
    var old = items.FindIndex(p => p.Sku.Equals(args[1], StringComparison.OrdinalIgnoreCase)); if (old >= 0) items[old] = items[old] with { Stock = Math.Max(0, items[old].Stock - units) };
} else { Console.WriteLine("Uso: add SKU STOCK PRECIO | remove SKU UNIDADES | list"); return; }
File.WriteAllText(file, JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true })); Console.WriteLine("Inventario actualizado");

record Product(string Sku, int Stock, decimal Price);
