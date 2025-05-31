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


        PokeApiClient pokeClient = new PokeApiClient();
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
                outputBox.Text = "Loading";

                //get our pokemon data
                Pokemon pokemon = await GetPokemonAsync(inputBox.Text);

                if (pokemon != null) //if null it was never populated and an exception occured
                {
                    //clear the text box
                    outputBox.Text = "";

                    //fill with data
                    for (int iterator = 0; iterator < numberOfStats; iterator++)
                    {
                        int statValue = pokemon.Stats[iterator].BaseStat;
                        String statName = pokemon.Stats[iterator].Stat.Name;
                        outputBox.Text += $"{statName}: {statValue}\n";
                    }
                                       
                }
                else //pokemon not found
                {
                    outputBox.Text = "Pokemon not found";
                }
            }
        }


        /*
        * Function: GetPokemonAsync 
        * Description: Get pokemon data and return it
        * Parameters: String pokemonName
        * Returns: Pokemon data
        */
        private async Task<Pokemon> GetPokemonAsync(String pokemonName)
        {
            Pokemon pokemon = null; //start null
            try
            {
                pokemon = await pokeClient.GetResourceAsync<Pokemon>(pokemonName); //search for pokemon
            }
            catch(Exception ex)
            {
                
            }
            
            return pokemon;
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