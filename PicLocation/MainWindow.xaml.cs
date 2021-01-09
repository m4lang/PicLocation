using Microsoft.Maps.MapControl.WPF;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;


namespace PicLocation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        private String fileName = "PinLocation.txt";

        //private List<Pushpin> mpinList = new List<Pushpin>(); // Overall list of pins that are placed in the map
        private List<map_pins> savedPinsList = new List<map_pins>();  // List of pins that have been previously saved so that there isnt a double save of a pin
        private List<map_pins> unSavedPinsList = new List<map_pins>(); // Pins that have been added but not saved
        private List<map_pins> mMapPinsList = new List<map_pins>(); // Keep track of the mapPin objects created that hold the name, description, type, and pin data

        public MainWindow()
        {

            InitializeComponent();
            loadSavedLocations();

        }


        private void savePins()
        {
            for (int i = 0; i < mMapPinsList.Count; i++)
            {
                if (!savedPinsList.Contains(mMapPinsList[i]))
                {
                    unSavedPinsList.Add(mMapPinsList[i]);
                }
            }

            foreach (map_pins MapPin in unSavedPinsList)
            {
                String unsavedPins = MapPin.toString();

                //This will save some text to a file in the same folder as your project exe file
                if (File.Exists(fileName))
                {
                    using (StreamWriter sw = new StreamWriter(fileName, true))
                        sw.WriteLine(unsavedPins);
                }
                else
                {
                    File.Create(fileName);
                    using (StreamWriter sw = new StreamWriter(fileName))
                        sw.WriteLine(unsavedPins);
                }

            }

        }

        private void loadSavedLocations()
        {
            string pinStringData = null;//Gets the string from file in format Name,Description,latitude,longitude,altitude,Type

            string pinName = null;// var to get name
            string pinDescription;// var to get description
            string pinLatitude = null;// var to get latitude
            string pinLongitude = null;// var to get longitude
            string pinAltitude = null;// var to get altitude
            string pinType = null;// var to get type
            double latitude = 0, longitude = 0, altitude = 0;

            using (StreamReader sr = File.OpenText(fileName))
                while (!sr.EndOfStream)
                {
                    //////////////////////////////////////////////////////////////////////
                    /// Read data from file as string
                    /// format of string -> latitude,longitude,altitude
                    //////////////////////////////////////////////////////////////////////

                    pinStringData = sr.ReadLine();
                    //if (maplocation.Equals("")) { }
                    // else
                    // {

                    //////////////////////////////////////////////////////////////////////
                    /// split dat retrieved in previous lines into usable parts
                    /// i.e. separate into longitdue, latitude, and altitude using
                    /// String.split and the comma as the delimenter
                    //////////////////////////////////////////////////////////////////////

                    String[] words = pinStringData.Split(',');

                    if (words.Length != 6)
                    {
                        Console.WriteLine("Not enough arguments to fulfill object of class mapPin");
                    }
                    else
                    {
                        pinName = words[0];
                        pinDescription = words[1];
                        pinLatitude = words[2];
                        pinLongitude = words[3];
                        pinAltitude = words[4];
                        pinType = words[5];

                        //////////////////////////////////////////////////////////////////////
                        /// Convert string represented locations to double
                        /// Save to the overall list of pins and the list of pins that have
                        ///     already been saved so there is no double saving
                        /// Add the pins to the map
                        //////////////////////////////////////////////////////////////////////
                        Double.TryParse(pinLatitude, out latitude);
                        Double.TryParse(pinLongitude, out longitude);
                        Double.TryParse(pinAltitude, out altitude);

                        Location pinLocation = new Location(latitude, longitude, altitude);

                        //////////////////////////////////////////////////////////////////////
                        /// Convert string to enum
                        //////////////////////////////////////////////////////////////////////

                        map_pins.pinTypes type = (map_pins.pinTypes)Enum.Parse(typeof(map_pins.pinTypes), pinType);

                        //////////////////////////////////////////////////////////////////////
                        /// Create new mapPin object and add to save list, 
                        /// total list(mMapPinlist) and the map
                        //////////////////////////////////////////////////////////////////////
                        map_pins newMapPin = new map_pins(pinName, pinDescription, type);
                        newMapPin.setPinLocation(pinLocation);

                        savedPinsList.Add(newMapPin);
                        mMapPinsList.Add(newMapPin);

                        myMap.Children.Add(newMapPin.Pin);
                    }
                }
        }

        public void request()
        {
            string location = "Grand Canyon";
            int numResults = 1;
            string apiKey = "AggKLZqv990poSD4weJaenncKhTmvbE6Pp8dcYuks6JvQpeSRHTFJDVtIjAobuYG";
            string queryURL = "http://dev.virtualearth.net/";

            var client = new RestClient(queryURL);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(apiKey);

        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);

            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            UserPinInput window = new UserPinInput();
            window.Show();
            window.Focus();

            // The pushpin to add to the map.
            map_pins newPin = new map_pins(window.NameOfPin.Text, window.DescOfPin.Text, map_pins.pinTypes.picturePin);

            newPin.setPinLocation(pinLocation);

            mMapPinsList.Add(newPin);
            //Console.WriteLine(newPin.toString());
            // Adds the pushpin to the map.
            myMap.Children.Add(newPin.Pin);

        }

        private void CloseOperation(object sender, CancelEventArgs e)
        {
            savePins();
        }
    }

}