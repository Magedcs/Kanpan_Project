using System;
namespace BackendTests
{
    [Serializable]
    public class test
    {

        private string id;
        public string Id { get { return id; } set { id = value; Console.Write("eyy"); } }

        private string title;
        public string Title { get { return title; } set { title = value; Console.Write("eyy"); } }

        private string description;
        public string Description { get { return description; } set { description = value; Console.Write("eyy"); } }


        public test(string a, string b, string c)
        {
            this.title = b;
            this.id = a;
            this.description = c;
        }

        public string gettt()
        {
            return title;
        }

        public void save()
        {
            Title = title;
            Description = description;
            Id = id;
        }

    }
}

