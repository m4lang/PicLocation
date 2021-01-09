using Microsoft.Maps.MapControl.WPF;
using System;

namespace PicLocation
{

    /// <summary>
    /// Pin class
    ///     -Creates a pin and gives it a name, description, and type
    ///     -
    /// </summary>
    public class map_pins
    {
        public Pushpin Pin;
        public string Name;
        public string Description;
        public pinTypes Type;

        public map_pins()
        {
            Name = "Def_Name";
            Description = "Def_Description";
            Type = pinTypes.picturePin;
            Pin = new Pushpin();
        }

        public map_pins(string name, string description, pinTypes type)
        {
            Pin = new Pushpin();
            Name = name;
            Description = description;
            Type = type;
        }

        // Converts a map_pin object to a sting with format name,description,latitude,longitude,altitude,Type
        public String toString()
        {
            string finalString = Name + "," + Description + "," + Pin.Location.ToString() + "," + Type.ToString();

            return finalString;
        }

        #region setters/getters

        public void setPinLocation(Location location)
        {
            Pin.Location = location;
        }
        public Location getPinLocation()
        {
            return Pin.Location;
        }

        public void setPinName(string name)
        {
            Name = name;
        }

        public String getPinName()
        {
            return Name;
        }

        public void setPinDescription(string description)
        {
            Description = description;
        }

        public String getPinDescription()
        {
            return Description;
        }

        public void setPinType(pinTypes type)
        {
            Type = type;
        }

        public pinTypes getPinType()
        {
            return Type;
        }

        #endregion

        #region pinTypes

        public enum pinTypes
        {

            picturePin,
            destinationPin,
            mergedPin

        }

        #endregion
    }
}
