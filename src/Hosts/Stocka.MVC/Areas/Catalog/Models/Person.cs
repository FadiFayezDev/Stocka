namespace Stocka.MVC.Areas.Catalog.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        private Person(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static Person Create(string name)
        {
            return new Person(name);
        }
    }
}