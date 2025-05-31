using System.Text;
using System.Windows;
using PokeApiNet;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace Vivi_s_Pokemon_Stat_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int numberOfStats = 6; //hp, at, def, sp-at, sp-def, spd

        private class PokemonData
        {
            public Pokemon pokemon;
            public Exception exception;
            public PokemonData(Pokemon inputPokemon, Exception inputException) 
            {
                pokemon = inputPokemon;
                exception = inputException;
            }
        }

        private readonly PokeApiClient pokeClient = new();
        public MainWindow()
        {
            InitializeComponent();
        }


        /*
        * Function: inputBox_KeyDown 
        * Description: User searches for a pokemon
        * Parameters: object sender, KeyEventArgs e
        * Returns: void
        */
        private async void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //inform user of loading
                OutputBox.Text = "Loading";

                //get our pokemon data
                PokemonData pokemonData = await GetPokemonAsync(InputBox.Text);

                if (pokemonData.exception == null) //if an exception did not occur
                {
                    //clear the text box
                    OutputBox.Text = "";

                    //fill with data
                    for (int iterator = 0; iterator < numberOfStats; iterator++)
                    {
                        int statValue = pokemonData.pokemon.Stats[iterator].BaseStat;
                        String statName = pokemonData.pokemon.Stats[iterator].Stat.Name;
                        OutputBox.Text += $"{statName}: {statValue}\n";
                    }
                                       
                }
                else //pokemon not found
                {
                    OutputBox.Text = pokemonData.exception.Message.ToString();
                }
            }
        }


        /*
        * Function: GetPokemonAsync 
        * Description: Get pokemon data and return it
        * Parameters: String pokemonName
        * Returns: Pokemon data
        */
        private async Task<PokemonData> GetPokemonAsync(String pokemonName)
        {
            Pokemon pokemon = null; //start null
            Exception exception = null;
            try
            {
                pokemon = await pokeClient.GetResourceAsync<Pokemon>(pokemonName); //search for pokemon
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            PokemonData data = new(pokemon, exception);

            return data;
        }

        /*
        * Function: Window_Closed 
        * Description: Shutdown the program when the main window is closed
        * Parameters: object sender, EventArgs e
        * Returns: void
        */
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}