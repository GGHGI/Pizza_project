using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace projet_pizza

{
    class PizzaPersonnalisee : Pizza
    {

        static int nbPizzasPersonnalisee = 0;

        public PizzaPersonnalisee() : base("Personnalisée", 5, false, null)
        {
            nbPizzasPersonnalisee++;
            nom = "Personnalisée" + nbPizzasPersonnalisee;


            ingredients = new List<string>();

            while (true)
            {
                Console.Write("Rentrez un ingrédient pour la pizza personnalisée  : " + nbPizzasPersonnalisee + "(Enter pour terminer)");
                var ingredient = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ingredient))
                {
                    break;
                }
                if (ingredients.Contains(ingredient))
                {
                    Console.WriteLine("Erreur, cet ingrédient est deja dans la liste ");

                }
                else
                {
                    ingredients.Add(ingredient);
                    Console.WriteLine(string.Join(", ", ingredients));


                }

                Console.WriteLine();

            }

            prix = 5 + ingredients.Count * 1.5f;


        }
    }




    class Pizza
    {
        public string nom { get; protected set; }
        public float prix { get; protected set; }
        public bool vegetarienne { get; private set; }
        public List<string> ingredients { get; protected set; }

        public Pizza(string nom, float prix, bool vegetarienne, List<string> ingredients)
        {
            this.nom = nom;
            this.prix = prix;
            this.vegetarienne = vegetarienne;
            this.ingredients = ingredients;
        }

        public void Afficher()
        {

            string nomAfficher = FormatPremiereMajuscules(nom);
            List<string> ingredientAfficher = ingredients.Select(i => FormatPremiereMajuscules(i)).ToList();


            if (vegetarienne)
            {
                Console.WriteLine(nomAfficher + " (V) - " + prix + "€");
                Console.WriteLine(string.Join(",", ingredientAfficher));
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(nomAfficher + " - " + prix + "€");
                Console.WriteLine(string.Join(",", ingredientAfficher));
                Console.WriteLine();
            }



        }

        private static string FormatPremiereMajuscules(string s)
        {

            if (string.IsNullOrEmpty(s))
                return s;


            string Minusucule = s.ToLower();
            string Majuscule = s.ToUpper();
            string resultat = Majuscule[0] + Minusucule.Substring(1);

            return resultat;
        }

        class program
        {

            static List<Pizza> GetPizzasFromCode()
            {
                var pizzas = new List<Pizza>()
                {

                    new Pizza("4 fromages", 11.5f, true, new List<string>{"cantal", "bleu", "mozzarella", "tomates", "gruyère"}),
                    new Pizza("mexicaine", 13f, false , new List < string > { "cantal", "poivrons", "mozzarella", "épices", "gruyère" }),
                    new Pizza("calzone", 12f, false , new List < string > { "cantal", "sauce tomate", "mozzarella", "jambon", "gruyère" }),
                    new Pizza("margarita", 8f, true, new List<string>{"cantal", "oignon", "tomates", "conté", "gruyère","basilic"}),
                    new Pizza("indienne", 10.5f, false, new List<string> { "mozzarella", "poulet", "mozzarella", "curry", "gruyère" }),
                    new Pizza("complète", 12f, false, new List<string> { "cantal", "tomates", "poulet", "conté", "gruyère" }),
                    //new PizzaPersonnalisee(),
                    //new PizzaPersonnalisee()
                    };

                return pizzas;
            }

            static List<Pizza> GetPizzasFromFile(string filename)
            {

                string json = null;
                try
                {
                    json = File.ReadAllText(filename);
                }
                catch
                {
                    Console.WriteLine("Erreur de lecture du fichier : " + filename);
                    return null;
                }

                List<Pizza> pizzas = null;
                try
                {
                    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);

                }
                catch
                {
                    Console.WriteLine("Erreur : les données json ne sont pas valides");
                    return null;
                }
                return pizzas;
            }

            static void GenerateJsonFile(List<Pizza> pizzas, string filename)
            {
                string json = JsonConvert.SerializeObject(pizzas);
                File.WriteAllText(filename, json);
            }

            static List<Pizza> GetPizzasFromurl(string url)
            {
                var webclient = new WebClient();
                string json = null;

                try
                {
                    json = webclient.DownloadString(url);
                }
                catch
                {
                    Console.WriteLine("Erreur réseau ");
                    return null;
                }





                List<Pizza> pizzas = null;
                try
                {
                    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);

                }
                catch
                {
                    Console.WriteLine("Erreur : les données json ne sont pas valides");
                    return null;
                }
                return pizzas;
            }

            static void Main(string[] args)
            {
                Console.OutputEncoding = Encoding.UTF8;

                var filename = "pizzas.json";
                var pizzas = GetPizzasFromurl("https://codeavecjonathan.com/res/pizzas2.json");

                //var pizzas = GetPizzasFromFile(filename);


                if (pizzas != null)
                {
                    foreach (var pizza in pizzas)
                    {
                        pizza.Afficher();
                    }

                }
            }
        }
    }
}






//pizzas = pizzas.OrderBy(p => p.prix).ToList();
//pizzas = pizzas.Where(p => p.vegetarienne).ToList();

//pizzas = pizzas.Where(p => p.ingredients.Where(i => i.ToLower().Contains("tomate")).ToList().Count > 0).ToList();


//var pizza1 = new Pizza("4 fromages", 11.5f, true);
//pizza1.Afficher();

